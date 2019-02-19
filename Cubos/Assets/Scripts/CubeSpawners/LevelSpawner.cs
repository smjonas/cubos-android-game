using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LevelSpawner : BaseSpawner {

	public Powerups powerups;
	private PowerupExe powExe;
	private int levelGaps;

	public GameObject text, tint;
	private bool doUndoTutorial, updated;

	protected override void Start() {

		Camera.main.backgroundColor = LevelData.instance.levels [LevelManager.instance.currentLevel - 1].bgColor;
		levelGaps = LevelData.instance.levels [LevelManager.instance.currentLevel - 1].gaps;
		base.Start ();
		base.InitInfo (false);
		base.InstInvisibleObjects ();
		base.ResetCubes ();
	}

	public override void UpdateBoard() {

		base.UpdateBoard ();
		UpdateMoves ();

		if (matchingCubes == LevelManager.instance.boardWidth * LevelManager.instance.boardHeight - levelGaps && LevelManager.instance.movesLeft >= 0) {
			
			if (LevelManager.instance.currentLevel == 24) {

				BetaCompletedScreen.instance.Show (Constants.OnLevelEndScrollDownTime);
				LevelManager.instance.Save ();

			} else {				

				WinningScreen.instance.Show (Constants.OnLevelEndScrollDownTime);
				LevelManager.instance.UnlockNextLevel ();
				LevelManager.instance.Save ();
			}

		} else if (LevelManager.instance.movesLeft <= 0)
			LosingScreen.instance.Show (Constants.OnLevelEndScrollDownTime);
	
		if (!updated && LevelManager.instance.currentLevel == 2 && LevelManager.instance.moveInfo [Constants.Powerup.Switch] == 3) {
			StartCoroutine(StartUndoButtonTutorial ());
			updated = true;
		}
	}

	private void UpdateMoves() {
		
		for (int i = 0; i < powerupMovesLeft.Count; i++) {
			powerupMovesLeft[i].text = "x " + LevelManager.instance.moveInfo[i];
		}
	}

	public void RecordMove(PowerupExe powerup) {
		this.powExe = powerup;
	}

	public void UndoMove() {

		if (powExe != null) {

			if (doUndoTutorial)
				StartCoroutine (UpdateUndoButtonTutorial ());
			
			LevelManager.instance.moveInfo [powExe.type]++;
			LevelManager.instance.movesLeft++;
			changeMoveInfo = false;

			var cube = new List<Cube> () { powExe.cube };
			var type = powExe.type;

			switch (type) {
				
			case 0:
				StartCoroutine (powerups.ExecutePowerup (type, cube, powExe.positions, powExe.direction));
				break;
			case 1:
				if (cube [0] == null)
					StartCoroutine (powerups.ExecutePowerup (type, powExe.cubes, powExe.positions, powExe.direction));
				break;
			case 2:
				StartCoroutine (powerups.ExecutePowerup (type, cube, powExe.positions, powExe.direction));
				break;
			}				
			changeMoveInfo = true;
			UpdateMoves ();
			powExe = null;
		}
	}

	private IEnumerator StartUndoButtonTutorial() {

		for (int i = 0; i < invObjects.Count; i++)
			invObjects [i].SetActive (false);

		doUndoTutorial = true;

		tint.SetActive (true);
		tint.FadeTo<Image> (145f, 0.6f);

		text.SetActive (true);
		text.FadeTo<TextMeshProUGUI> (255f, 0.6f);

		yield return new WaitForSeconds (Constants.CubeSwitchTime * 1.1f);
		GestureDetector.ready = false;
	}

	private IEnumerator UpdateUndoButtonTutorial() {

		tint.FadeTo<Image> (0f, 0.6f);
		text.FadeTo<TextMeshProUGUI> (0f, 0.6f);
		yield return new WaitForSeconds (0.6f);

		text.SetActive (false);
		tint.SetActive (false);
		GestureDetector.ready = true;
	}
}