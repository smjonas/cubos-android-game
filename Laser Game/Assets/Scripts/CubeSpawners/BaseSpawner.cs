using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public abstract class BaseSpawner : MonoBehaviour {

	#region variables

	public Cube cubePrefab;
	public Marker markerPrefab;
	public GameObject dotPrefab;
	public List<GameObject> invObjectPrefabs;
	[HideInInspector] public List<GameObject> invObjects;

	public List<TextMeshProUGUI> powerupMovesLeft;
	protected List<int> powerupsToRemove;
	[HideInInspector] public bool changeMoveInfo = true;

	protected float xStart, yStart;
	public static bool isShuttingDown;

	protected int[,] currentMarkerPositions, currentCubePositions;
	protected Color[] colorRange;
	public static int matchingCubes;
	#endregion variables

	protected virtual void Start () {

		xStart = -((LevelManager.instance.boardWidth * Constants.CubeSizeX + (LevelManager.instance.boardWidth - 1) * Constants.CubeGapX) / 2f) + Constants.MarkerOffset;
		yStart = -((LevelManager.instance.boardHeight * Constants.CubeSizeY + (LevelManager.instance.boardHeight - 1) * Constants.CubeGapY) / 2f) + Constants.MarkerOffset;

		if (LevelManager.instance.gameState == LevelManager.GameState.InTutorial)
			yStart -= 0.5f;

		Invoke("UpdateBoard", 0.1f);
	}

	protected void InitInfo(bool inTutorial) {
		
		if (inTutorial) {

			currentMarkerPositions = LevelData.instance.tutorials[LevelManager.instance.currentTutorial - 1].markerPositions.Mirror();
			currentCubePositions = LevelData.instance.tutorials[LevelManager.instance.currentTutorial - 1].cubePositions.Mirror();
			colorRange = LevelData.instance.tutorials[LevelManager.instance.currentTutorial - 1].colors;

		} else {
					
			currentMarkerPositions = LevelData.instance.levels [LevelManager.instance.currentLevel - 1].markerPositions.Mirror ();
			currentCubePositions = LevelData.instance.levels [LevelManager.instance.currentLevel - 1].cubePositions.Mirror ();
			colorRange = LevelData.instance.levels [LevelManager.instance.currentLevel - 1].colors;
		}

		var activeIcons = new List<Transform> ();
		for (int i = 0; i < LevelManager.instance.moveInfo.Length; i++) {
			
			if (LevelManager.instance.moveInfo [i] == 0)
				powerupMovesLeft [i].transform.parent.gameObject.SetActive (false);
			else activeIcons.Add (powerupMovesLeft[i].transform);

			powerupMovesLeft [i].text = "x " + LevelManager.instance.moveInfo[i];
		}

		for (int i = 0; i < activeIcons.Count; i++) {
			activeIcons [i].parent.localPosition = new Vector3(Constants.PowerupIconCenterXPos + i * Constants.PowerupIconGap - (Constants.PowerupIconGap * activeIcons.Count / 2), activeIcons[i].parent.localPosition.y);
		}
	}

	protected void InstInvisibleObjects() {

		invObjects.Add (Instantiate (invObjectPrefabs[0], new Vector3( xStart - 0.45f - Constants.MarkerOffset, 0f), Quaternion.identity, transform));
		invObjects.Add (Instantiate (invObjectPrefabs[1], new Vector3( xStart + LevelManager.instance.boardWidth * Constants.CubeSizeX + (LevelManager.instance.boardWidth - 1) * Constants.CubeGapX + 0.4f  - Constants.MarkerOffset, 0f), Quaternion.identity, transform));
		invObjects.Add (Instantiate (invObjectPrefabs[2], new Vector3( 0f, yStart + LevelManager.instance.boardHeight * Constants.CubeSizeY + (LevelManager.instance.boardHeight - 1) * Constants.CubeGapY + 0.45f  - Constants.MarkerOffset), Quaternion.identity, transform));
		invObjects.Add (Instantiate (invObjectPrefabs[3], new Vector3( 0f, yStart - 0.45f - Constants.MarkerOffset), Quaternion.identity, transform));

		for (int i = 0; i < invObjects.Count; i++) {
			invObjects [i].SetActive (true);
		}
	}

	public void GenerateLevel(int[,] cubePosition) {

		currentCubePositions = cubePosition;
		ResetCubes ();
	}

	public virtual void ResetCubes() {

		var gameobjects = GameObject.FindGameObjectsWithTag ("Cube");
		foreach (GameObject g in gameobjects ) {
			Destroy (g);
		}

		var levelGaps = LevelData.instance.levels [LevelManager.instance.currentLevel - 1].gaps;
		for (int x = 0; x < LevelManager.instance.boardWidth; x++) {
			for (int y = 0; y < LevelManager.instance.boardHeight; y++) {

				if (levelGaps == 0 || (levelGaps != 0 && currentCubePositions [y, x] != -1)) {
					
					Cube cube = Instantiate (cubePrefab, new Vector3 (xStart + x * Constants.CubeSpawnDistX, yStart + y * Constants.CubeSpawnDistY), Quaternion.identity, transform) as Cube;
					cube.pos = y * LevelManager.instance.boardWidth + x;

					Marker marker = Instantiate (markerPrefab, new Vector3 (xStart + x * Constants.CubeSpawnDistX, yStart + y * Constants.CubeSpawnDistY, 0.1f), Quaternion.identity, transform) as Marker;
					marker.SetColor (colorRange [currentMarkerPositions [y, x]]);

					cube.SetColor (colorRange [currentCubePositions [y, x]]);

				} else {
					Instantiate (dotPrefab, new Vector3 (xStart + x * Constants.CubeSpawnDistX, yStart + y * Constants.CubeSpawnDistY), Quaternion.identity, transform);
				}
			}
		}
	}

	public virtual void UpdateBoard() {

		matchingCubes = 0;
		var cubeObjects = new List<GameObject> ();
		var cubes = new List<Cube> ();

		cubeObjects.AddRange (GameObject.FindGameObjectsWithTag ("Cube"));
		foreach (GameObject c in cubeObjects)
			cubes.Add (c.GetComponent<Cube> ());
		
		for (int w = 0; w < currentMarkerPositions.GetLength (1); w++) {
			for (int h = 0; h < currentMarkerPositions.GetLength (0); h++) {

				var pos = h * LevelManager.instance.boardWidth + w;
				var cube = cubes.Where (x => x.GetComponent<Cube> ().pos == pos).FirstOrDefault ();

				if (cube != null) {

					cube.SetNeighbors ();
					if (colorRange [currentMarkerPositions [h, w]] == cube.color) {
						matchingCubes++;
					}
				}
			}
		}
	} 

	#region Misc

	public Cube SpawnCubeRelative(Cube cube, Swipes.Direction direction) {

		if (cube != null) {

			var x = cube.transform.position.x;
			var y = cube.transform.position.y;
			var z = cube.transform.position.z;

			var position = Vector3.zero;

			switch (direction) {
			case Swipes.Direction.UP:
				position = new Vector3 (x, y - 0.74f, z);
				break;
			case Swipes.Direction.DOWN:
				position = new Vector3 (x, y + 0.74f, z);
				break;
			case Swipes.Direction.LEFT:
				position = new Vector3 (x + 0.8f, y, z);
				break;
			case Swipes.Direction.RIGHT:
				position = new Vector3 (x - 0.8f, y, z);
				break;
			default:
				Debug.LogError ("Relative position not defined!");
				break;
			}
			if (position == Vector3.zero)
				return null;

			return SafeInstantiate (cubePrefab, position, cube.transform.rotation);
		}
		return null;
	}

	Cube SafeInstantiate(Cube original, Vector3 position, Quaternion rotation) {

		if (!isShuttingDown)
			return Instantiate (original, position, rotation);

		return null;
	}

	protected virtual void OnGUI () {
		GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = 65;
	}

	void OnApplicationQuit() {
		isShuttingDown = true;
	}
	#endregion Misc

}
