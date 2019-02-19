using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/*
  Plays animation and loads selected level / tutorial
*/

public class LevelSelection : MonoBehaviour {

	public static LevelSelection instance;

	void Start() {
		instance = this;
	}

	public void SelectLevel() {

		var button = EventSystem.current.currentSelectedGameObject;
		if (button.GetComponent<ButtonData> ().state != ButtonData.LevelState.LOCKED) {

			var level = button.GetComponent<ButtonData> ().levelNumber;
			LoadLevelOrTutorial (level);
		}
	}

	public void NextLevel() {
		LoadLevelOrTutorial (LevelManager.instance.currentLevel + 1);
	}

	public void LoadLevelOrTutorial(int level) {

		var priorTutorial = LevelData.instance.levels[level - 1].priorTutorial;
		if (priorTutorial != 0) {

			LevelManager.instance.UpdateForNewTutorial (priorTutorial);
			LevelManager.instance.currentLevel = level;
			Menu.instance.PlayTutorial ();

		} else {
			PlayLevel (level);
		}
	}

	public void PlayLevel(int level) {

		LevelManager.instance.LoadNewLevel (level);
		Menu.instance.Play ();
	}

	public void RestartLevel() {
		PlayLevel (LevelManager.instance.currentLevel);
	}

	public void RestartTutorial() {
		LoadLevelOrTutorial (LevelManager.instance.currentLevel);
	}
}