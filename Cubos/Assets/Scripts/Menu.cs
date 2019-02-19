using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
  Switching between scenes
*/

public class Menu : MonoBehaviour {

	public static Menu instance;

	void Awake() {

		if (instance == null) {
			
			DontDestroyOnLoad (transform.gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	void Update() {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			
			var scene = SceneManager.GetActiveScene ().buildIndex;

			if (scene == Constants.MenuScene)
				Exit ();
			else if (scene == Constants.LevelSelectionScene || scene == Constants.LevelEditorScene)
				ToMenu ();
			else if (scene == Constants.LevelEditorScene)
				ToLevelEditorMenu ();
			else if (LevelManager.instance.gameState == LevelManager.GameState.InLevel || LevelManager.instance.gameState == LevelManager.GameState.InTutorial)
				PauseScreen.instance.Show ();
		}
	}

    public void Play() {
		FadeOutAndLoadScene (Constants.GameScene);
    }

	public void PlayTutorial() {
		FadeOutAndLoadScene (Constants.TutorialScenes[LevelManager.instance.currentTutorial - 1]);
	}

	public void ToLevelSelection() {

		FadeOutAndLoadScene (Constants.LevelSelectionScene);		
		LevelManager.instance.gameState = LevelManager.GameState.InMenu;
	}

	public void ToLevelEditorMenu() {
		StartCoroutine (LoadScene (Constants.LevelEditorMenuScene));
	}

	public void ToLevelEditor() {
		StartCoroutine (LoadScene (Constants.LevelEditorScene));
	}

	public void ToMenu() {

		LevelManager.instance.Save ();
		LevelManager.instance.gameState = LevelManager.GameState.InMenu;
		FadeOutAndLoadScene (Constants.MenuScene);
	}

	private void FadeOutAndLoadScene(int scene) {

		var fade = GameObject.Find ("SceneFading");
		if (fade != null) {

			var fading = fade.GetComponent<SceneFading> ();
			fading.FadeBlack ();
			StartCoroutine (LoadScene (scene, fading.fadeOutTime));

			var audioFading = fade.GetComponent<AudioManager> ();
			if (audioFading != null)
				audioFading.Fade (0f, fading.fadeOutTime);
			
		} else StartCoroutine (LoadScene (scene));
	}
		
	private IEnumerator LoadScene(int scene) {

		AsyncOperation operation = SceneManager.LoadSceneAsync (scene);
		while (!operation.isDone) {
			yield return null;
		}
	}

	private IEnumerator LoadScene(int scene, float time) {

		yield return new WaitForSeconds (time);
		StartCoroutine (LoadScene (scene));
	}
		
	public void Exit() {
		Application.Quit ();
	}
}
