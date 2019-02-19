using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI.Extensions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;
using System;

/*
  Stores information about current level
  Saving and loading level
  Update level info before loading new level
*/

public class LevelManager : MonoBehaviour {

	#region variables
	public enum GameState { InMenu, InLevel, InTutorial, InLevelEditor }
	public GameState gameState;
	public int levelCount;
	public int boardWidth, boardHeight;
	public int currentLevel, currentTutorial, maxLevel;
	public int currentPage;
	public string language;

	public int pulseIndex, lastPulseIndex;

	public int movesLeft;
	public Color[,] cubeInfo;
	public int[] moveInfo;

	public Swipes.Direction[] moveDirections;
	public List<ButtonData.LevelState> levelStates;

	public bool soundOn;
	private string dataPath;

    public static LevelManager instance;

    void Awake() {

		if (instance == null) {
			DontDestroyOnLoad (transform.gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		// Set default values
		levelStates = new List<ButtonData.LevelState> ();
		currentLevel = 1;

		cubeInfo = new Color[boardHeight, boardWidth];

		soundOn = true;
		language = "English";

		dataPath = Application.persistentDataPath + "/gameData.dat";
		print (dataPath);

		ResetLevelStates ();
		Load ();
	}

	#endregion variables

	public bool RecordMove() {
		return gameState == GameState.InLevel;
	}

	public void UnlockNextLevel() {

		levelStates.Add(ButtonData.LevelState.CURRENT);
		for (int i = 0; i < levelStates.Count - 1; i++) {
			levelStates [i] = ButtonData.LevelState.UNLOCKED;
		}
	}

	public void UpdateLevelGenInfoAndLoad(int[,] cubePositions) {

		boardWidth = cubePositions.GetLength (1);
		boardHeight = cubePositions.GetLength (0);
		cubeInfo = new Color[boardHeight, boardWidth];

		movesLeft = LevelGenerator.instance.movesLeft;
		moveInfo = LevelGenerator.instance.moveInfo;
	}

	public void LoadNewLevel(int level) {

		gameState = GameState.InLevel;
		currentLevel = level;

		boardWidth = LevelData.instance.levels[currentLevel - 1].width;
		boardHeight = LevelData.instance.levels[currentLevel - 1].height;

		movesLeft = LevelData.instance.levels[currentLevel - 1].movesLeft;
		moveInfo = LevelData.instance.levels[currentLevel - 1].moveInfo.Clone() as int[];
	}

	public void LoadNewLevel() {
		LoadNewLevel(currentLevel);
	}

	public void UpdateForNewTutorial(int tutorial) {

		gameState = GameState.InTutorial;
		currentTutorial = tutorial;

		boardWidth = LevelData.instance.tutorials[currentTutorial - 1].width;
		boardHeight = LevelData.instance.tutorials[currentTutorial - 1].height;

		pulseIndex = 1;
		lastPulseIndex = 0;
		moveDirections = LevelData.instance.tutorials [currentTutorial - 1].moveDirections.Clone () as Swipes.Direction[];
		movesLeft = LevelData.instance.tutorials[currentTutorial - 1].movesLeft;
		moveInfo = LevelData.instance.tutorials[currentTutorial - 1].moveInfo.Clone() as int[];
		cubeInfo = new Color[boardHeight, boardWidth];
	}

	#region Saving / Loading to file
	public void Save() {

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (dataPath);

		GameData data = new GameData ();

		data.maxLevel = maxLevel;
		data.levelStates = levelStates;
		data.soundOn = soundOn;
		data.language = language;

		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load() {

		if (File.Exists (dataPath)) {

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (dataPath, FileMode.Open);

			if (file.Length > 0) {
				GameData data = (GameData)bf.Deserialize (file);
				file.Close ();

				maxLevel = data.maxLevel;
				levelStates = data.levelStates;
				soundOn = data.soundOn;
				language = data.language;
			}
		}
	}

	public void ResetProgress() {

		File.Delete (dataPath);
		ResetLevelStates ();

		Save ();
		Load ();

		Menu.instance.ToMenu ();
	}

	private void ResetLevelStates() {

		levelStates.Clear ();
		levelStates.Add (ButtonData.LevelState.CURRENT);
	}
}

[Serializable]
class GameData {

	public int maxLevel;
	public List<ButtonData.LevelState> levelStates;
	public bool soundOn;
	public string language;
}
#endregion
