using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class Powerups : MonoBehaviour {

	#region variables
	public static Switch switch_;
	public static Rotate rotate;
    public static SwitchRow switchRow;
	public static BaseSpawner spawner;
	private BaseTutorialSpawner tutSpawner;

	public bool finishedAnimation; 

	protected virtual void Start () {

		switch_ = gameObject.AddComponent<Switch> ();
		rotate = gameObject.AddComponent<Rotate> ();
		switchRow = gameObject.AddComponent<SwitchRow> ();
		finishedAnimation = true;

		if (SceneManager.GetActiveScene ().buildIndex == Constants.GameScene)
			spawner = (LevelSpawner)GameObject.Find ("LevelSpawner").GetComponent<LevelSpawner> ();
		else if (SceneManager.GetActiveScene ().buildIndex == Constants.LevelEditorScene)
			spawner = (LevelEditorSpawner)GameObject.Find ("EditorSpawner").GetComponent<LevelEditorSpawner> ();
		else if (SceneManager.GetActiveScene().buildIndex == Constants.TutorialScenes[0])
			spawner = (Tutorial1Spawner) GameObject.Find ("TutorialSpawner").GetComponent<Tutorial1Spawner>();
		else if (SceneManager.GetActiveScene().buildIndex == Constants.TutorialScenes[1])
			spawner = (Tutorial2Spawner) GameObject.Find ("TutorialSpawner").GetComponent<Tutorial2Spawner>();
		else if (SceneManager.GetActiveScene().buildIndex == Constants.TutorialScenes[2])
			spawner = (Tutorial3Spawner) GameObject.Find ("TutorialSpawner").GetComponent<Tutorial3Spawner>();
		else if (SceneManager.GetActiveScene().buildIndex == Constants.TutorialScenes[3])
			spawner = (Tutorial4Spawner) GameObject.Find ("TutorialSpawner").GetComponent<Tutorial4Spawner>();

		tutSpawner = GameObject.FindObjectOfType<BaseTutorialSpawner> ();
		
	}
	#endregion variables

	public virtual IEnumerator ExecutePowerup(int powerup, List<Cube> cubes, List<int> positions, Swipes.Direction direction) {

		/*if (tutSpawner != null) {
			if (!tutSpawner.ValidProgress ()) {
				LevelManager.instance.lastPulseIndex = LevelManager.instance.pulseIndex - 1;
				LevelManager.instance.pulseIndex = LevelManager.instance.lastPulseIndex + 1;
				tutSpawner.UpdateBoard ();
			}
		}*/
			
		if (LevelManager.instance.movesLeft > 0 && finishedAnimation) {

            GestureDetector.ready = false;
            if (LevelManager.instance.moveInfo[powerup] > 0) {
                finishedAnimation = false;

                switch (powerup) {

                case 0:
					switch_.TryExecute(cubes[0], direction);
                    break;
				case 1: 
					rotate.GetDirectionAndExecute (cubes, positions);
					break;
				case 2:
					switchRow.TryExecute(cubes[0], direction);
                    break;
                default:
					Debug.LogError("Powerup not defined!");
                    break;
                }
            }
            else if (LevelManager.instance.gameState == LevelManager.GameState.InLevel && spawner.powerupMovesLeft[powerup].gameObject.activeInHierarchy)
				spawner.powerupMovesLeft[powerup].GetComponent<UIAnimations>().PulseSize(Colors.powerupMissing);

			yield return new WaitForSeconds (Constants.CubeSwitchTime);
			finishedAnimation = true;
            spawner.UpdateBoard();
            GestureDetector.ready = true;
		}
	}

	public void UpdateMoveInfo(int powerup) {

		if (LevelManager.instance.gameState == LevelManager.GameState.InLevel || LevelManager.instance.gameState == LevelManager.GameState.InTutorial) {
			if (spawner.changeMoveInfo) {

				LevelManager.instance.moveInfo [powerup]--;
				LevelManager.instance.movesLeft--;

				if (spawner.powerupMovesLeft[powerup].gameObject.activeInHierarchy)
					spawner.powerupMovesLeft[powerup].GetComponent<UIAnimations>().PulseSize(Colors.powerupUsed);
			}
		}
	}

    #region Powerup methods
	public void SwitchCubes(Cube one, Cube two, Swipes.Direction direction) {

		if (one == null || two == null || (one.pos == two.pos)) {
			finishedAnimation = true;
			return;
		}
		if (one.pos - 1 == two.pos || one.pos + 1 == two.pos || one.pos + LevelManager.instance.boardWidth == two.pos || one.pos - LevelManager.instance.boardWidth == two.pos ||
		    one.pos + LevelManager.instance.boardWidth - 1 == two.pos || one.pos + LevelManager.instance.boardWidth + 1 == two.pos ||
		    one.pos - LevelManager.instance.boardWidth - 1 == two.pos || one.pos - LevelManager.instance.boardWidth + 1 == two.pos) {

			var tempPos = one.pos;
			one.pos = two.pos;
			two.pos = tempPos;

			var distance = (one.gameObject.transform.position - two.gameObject.transform.position).magnitude;
			var speed = distance / Constants.CubeSwitchTime;
			var time = distance / speed;

			iTween.MoveTo (one.gameObject, iTween.Hash ("position", two.gameObject.transform.position, "time", time, "easetype", iTween.EaseType.linear));
			iTween.MoveTo (two.gameObject, iTween.Hash ("position", one.gameObject.transform.position, "time", time, "easetype", iTween.EaseType.linear, "oncomplete", "SetFinished", "oncompletetarget", gameObject));

			UpdateMoveInfo (Constants.Powerup.Switch);

			if (LevelManager.instance.RecordMove ()) {
				
				var lvlSpawner = (LevelSpawner)spawner;
				lvlSpawner.RecordMove (new PowerupExe (Constants.Powerup.Switch, two, direction));
			}
		}
	}

	public void MoveToCube(Cube from, Cube to, float time) {
        
		if (from == null || to == null) {
			finishedAnimation = true;
			return;
		}
		iTween.MoveTo (from.gameObject, iTween.Hash ("position", to.gameObject.transform.position, "time", time, "easetype", iTween.EaseType.linear, "oncomplete", "SetFinished", "oncompletetarget", gameObject));
	}


	public void MoveToCubeRelative(Cube cube, Swipes.Direction direction, float time) {

		if (cube == null)
			return;

		var x = cube.transform.position.x;
		var y = cube.transform.position.y;
		var z = cube.transform.position.z;

		var position = Vector3.zero;

		switch (direction) {
		case Swipes.Direction.UP:
			position = new Vector3 (x, y + 0.74f, z);
			break;
		case Swipes.Direction.DOWN:
			position = new Vector3 (x, y - 0.74f, z);
			break;
		case Swipes.Direction.LEFT:
			position = new Vector3 (x - 0.8f, y, z);
			break;
		case Swipes.Direction.RIGHT:
			position = new Vector3 (x + 0.8f, y, z);
			break;
		default:
			Debug.LogError ("Relative position not defined!");
			break;
		}
		if (position == Vector3.zero)
			return;
		MoveToPosition (cube, position, time);
	}

	private void MoveToPosition(Cube from, Vector3 to, float time) {

		if (from == null) {
			finishedAnimation = true;
			return;
		}
		iTween.MoveTo (from.gameObject, iTween.Hash ("position", to, "time", time, "easetype", iTween.EaseType.linear));
	}

	public void SetFinished() {
		finishedAnimation = true;
	}

    #endregion
}