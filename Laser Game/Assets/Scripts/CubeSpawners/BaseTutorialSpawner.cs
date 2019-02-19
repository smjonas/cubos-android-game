using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public abstract class BaseTutorialSpawner : BaseSpawner {

	private List<Cube> cubes;
	private int[,] currentPulsePositions;
	protected string text;
	public TextMeshProUGUI tutorialText;
	public bool spawnInvObjects;

	protected int tutorialNumber, textCount;

	public TutorialAnimator animator;
	private LanguageManager langManager;

	protected override void Start() {

		Camera.main.backgroundColor = LevelData.instance.tutorials [LevelManager.instance.currentTutorial - 1].bgColor;
		base.Start ();
		base.InitInfo (true);
		if (spawnInvObjects)
			base.InstInvisibleObjects ();
		
		currentPulsePositions = LevelData.instance.tutorials[LevelManager.instance.currentTutorial - 1].pulsePositions.Mirror ();

		langManager = new LanguageManager (LevelManager.instance.language);

		text = "Tutorial " + tutorialNumber;
		tutorialText.text = text;
		tutorialText.alpha = 0f;
		ResetCubes ();	
	}

	public override void ResetCubes() {

		var gameobjects = GameObject.FindGameObjectsWithTag ("Cube");
		foreach (var g in gameobjects ) {
			Destroy (g);
		}

		cubes = new List<Cube> ();
		for (int x = 0; x < LevelManager.instance.boardWidth; x++) {

			for (int y = 0; y < LevelManager.instance.boardHeight; y++) {

				Cube cube = Instantiate (cubePrefab, new Vector3( xStart + x * Constants.CubeSpawnDistX, yStart + y * Constants.CubeSpawnDistY, 1f), Quaternion.identity, transform) as Cube;
				cube.pos = y * LevelManager.instance.boardWidth + x;
				cube.SetColor (colorRange [currentCubePositions [y, x]]);

				if (cube.pos != GetCurrentPulsePosition ())
					cube.SetMovable (false);
				else
					cube.SetPulse (true);

				cubes.Add (cube);
				Marker currentMarker = Instantiate(markerPrefab, new Vector3(xStart + x * Constants.CubeSpawnDistX, yStart + y * Constants.CubeSpawnDistY, 0.1f), Quaternion.identity, transform) as Marker;
				currentMarker.SetColor(colorRange[currentMarkerPositions[y, x]]);
			}
		}
	}

	public override void UpdateBoard() {

		base.UpdateBoard ();
		UpdatePowerupIcons ();
		UpdateTutorialProgress (true);
	}

	protected void UpdatePowerupIcons() {
	
		for (int i = 0; i < powerupMovesLeft.Count; i++) {
			powerupMovesLeft[i].text = "x " + LevelManager.instance.moveInfo[i];
		}
	}

	protected void UpdateTutorialProgress(bool updateHand) {

		var cubeObjects = new List<GameObject> ();
		var cubes = new List<Cube> ();

		cubeObjects.AddRange (GameObject.FindGameObjectsWithTag ("Cube"));
		foreach (GameObject c in cubeObjects)
			cubes.Add (c.GetComponent<Cube> ());

		foreach (var cube in cubes) {

			if (cube.pos != GetCurrentPulsePosition ()) {
				cube.SetPulse (false);
				cube.SetMovable (false);
			}
			else {
				cube.SetPulse (true);
				cube.SetMovable (true);
			}
		}
		if (updateHand)
			ValidateProgress ();
	}

	private void ValidateProgress () {

		var index = LevelManager.instance.pulseIndex;
		var lastIndex = LevelManager.instance.lastPulseIndex;

		print (index + " " + lastIndex);

		if (lastIndex == index - 1)
			StartCoroutine (UpdateTutorialHand (index));
	}

	public bool ValidProgress() {
		return LevelManager.instance.lastPulseIndex == LevelManager.instance.pulseIndex - 1;
	}

	protected abstract IEnumerator UpdateTutorialHand (int index);

	protected void DisableCubes() {
		
		var cubeObjects = new List<GameObject> ();
		var cubes = new List<Cube> ();

		cubeObjects.AddRange (GameObject.FindGameObjectsWithTag ("Cube"));
		foreach (GameObject c in cubeObjects)
			cubes.Add (c.GetComponent<Cube> ());

		foreach (var cube in cubes) {
			cube.SetMovable (false);
		}
	}

	protected int GetCurrentPulsePosition() {

		for (int x = 0; x < currentPulsePositions.GetLength (1); x++) {
			for (int y = 0; y < currentPulsePositions.GetLength (0); y++) {

				if (currentPulsePositions [y, x] == LevelManager.instance.pulseIndex)
					return y * LevelManager.instance.boardWidth + x;
			}
		}
		return -1;
	}

	protected void AnimateHand(int cubePos, Vector3 offset, Swipes.Direction moveDirection, float length, int angle) {

		StopAnimation ();
		var cube = cubes.Where (c => c.pos == cubePos).FirstOrDefault ();
		var pos = cube.transform.position + new Vector3 (offset.x * Constants.CubeSizeX, offset.y * Constants.CubeSizeY);

		animator.SetPositions (pos, moveDirection, length, angle);
		animator.Animate (true);
	}

	protected void AnimateHand(int cubePos, Vector3 offset) {

		StopAnimation ();
		var cube = cubes.Where (c => c.pos == cubePos).FirstOrDefault ();
		var pos = cube.transform.position + new Vector3 (offset.x * Constants.CubeSizeX, offset.y * Constants.CubeSizeY);

		animator.SetCircularPositions (pos);
		StartCoroutine (animator.AnimateRotate ());
	}

	protected void AnimateHand(Vector3 pos, int angle) {

		StopAnimation ();
		animator.SetPositions (pos, angle);
		animator.AnimateStill ();
	}

	public void StopAnimation() {
		animator.StopAnimation ();
	}

	public void DestroyHand() {
		animator.DestroyHand ();
	}

	protected void FadeIn() {
		tutorialText.gameObject.FadeTo<TextMeshProUGUI> (255f, 0.6f);
	}

	protected void FadeOut() {
		StartCoroutine (FadeOutRoutine ());
	}

	private IEnumerator FadeOutRoutine() {
		
		tutorialText.gameObject.FadeTo<TextMeshProUGUI> (0f, 0.6f);
		yield return new WaitForSeconds (0.6f);
		tutorialText.gameObject.SetActive (false);
	}
}
