using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorSpawner : BaseSpawner {

	public Toggle fillTool, editTool;

	public static bool fillToolIsOn = true;
	public static bool editToolIsOn = false;
	public static Color color = Color.black;

	protected override void Start() {

		print ("STARTEEED");
		LevelManager.instance.gameState = LevelManager.GameState.InLevelEditor;

		xStart = -((LevelManager.instance.boardWidth * Constants.CubeSizeX + (LevelManager.instance.boardWidth - 1) * Constants.CubeGapX) / 2f)  + Constants.MarkerOffset - 0.3f;
		yStart = -((LevelManager.instance.boardHeight * Constants.CubeSizeY + (LevelManager.instance.boardHeight - 1) * Constants.CubeGapY) / 2f)  + Constants.MarkerOffset;

		base.InstInvisibleObjects ();
		this.ResetCubes ();
	}

	public override void ResetCubes () {
		
		for (int x = 0; x < LevelManager.instance.boardWidth; x++) {

			for (int y = 0; y < LevelManager.instance.boardHeight; y++) {

				Cube cube = Instantiate (cubePrefab, new Vector3 (xStart + x * Constants.CubeSpawnDistX, yStart + y * Constants.CubeSpawnDistY), Quaternion.identity, transform) as Cube;
				cube.pos = y * LevelManager.instance.boardWidth + x;
				cube.SetColor (Color.black);

				Marker marker = Instantiate (markerPrefab, new Vector3 (xStart + x * Constants.CubeSpawnDistX, yStart + y * Constants.CubeSpawnDistY, 0.1f), Quaternion.identity, transform) as Marker;
				marker.pos = y * LevelManager.instance.boardWidth + x;
				marker.SetColor (Color.black);
			}
		}
		Invoke("UpdateBoard", 0.3f);
	}

	public override void UpdateBoard () {

		var cubeObjects = new List<GameObject> ();
		var cubes = new List<Cube> ();

		cubeObjects.AddRange (GameObject.FindGameObjectsWithTag ("Cube"));
		foreach (GameObject c in cubeObjects)
			cubes.Add (c.GetComponent<Cube> ());

		foreach (var c in cubes)
			c.SetNeighbors();
	}
		
	public void UpdateTools() {
		
		fillToolIsOn = fillTool.isOn;
		editToolIsOn = editTool.isOn;
	}

	public void UpdateColorToggles(Image button) {
		color = button.color;
	}
}
