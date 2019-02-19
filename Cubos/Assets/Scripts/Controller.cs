using UnityEngine;

public class Controller : MonoBehaviour {

	public void ToMenu() {
		Menu.instance.ToMenu ();
	}

	public void SaveLevel() {
		LevelManager.instance.Save ();
	}

    public void ToLevelSelection() {
		Menu.instance.ToLevelSelection ();
	}

	public void ShowPauseScreen() {
		PauseScreen.instance.Show ();
	}

	public void UndoMove() {

		var spawner = (LevelSpawner)Powerups.spawner;
		spawner.UndoMove ();
	}

	public void NextLevel() {
		LevelSelection.instance.NextLevel ();
	}

	public void RestartLevel() {
		LevelSelection.instance.RestartLevel ();
	}

	public void RestartTutorial() {
		LevelSelection.instance.RestartTutorial ();
	}

	public void ShowOptionsScreen(float time) {
		OptionsScreen.instance.Show (time);
	}

	public void ShowOptionsScreen() {
		OptionsScreen.instance.Show();
	}

	public void ToggleSound() {
		GameObject.Find ("SceneFading").GetComponent<AudioManager> ().ToggleSound ();
	}

	public void ShowResetScreen() {
		ResetScreen.instance.Show ();
	}

	public void ResetProgress() {
		LevelManager.instance.ResetProgress ();
	}

	public void ShowBetaInfoScreen() {
		BetaInfoScreen.instance.Show ();
	}

	public void ShowLanguageScreen() {
		LanguageScreen.instance.Show ();
	}

	public void ChangeLanguageTo(string language) {
		LevelManager.instance.language = language;
	}

	public void OpenTwitterPage() {
		Application.OpenURL ("https://twitter.com/BlueShardGames");
	}

	public void OpenInstagramPage() {
		Application.OpenURL ("https://www.instagram.com/blue_shard_games/");
	}

	public void OpenFacebookPage() {
		Application.OpenURL ("https://www.facebook.com/Blue-Shard-Games-973702822772528/");
	}

	public void OpenHomePage() {
		Application.OpenURL ("http://scurrafurra.bplaced.net/index.html");
	}

	public void OpenPlayStore() {
		Application.OpenURL("https://play.google.com/store?hl=de");
	}

	public void ToLevelEditorMenu() {
		Menu.instance.ToLevelEditorMenu ();
	}

	public void ToLevelEditor() {
		Menu.instance.ToLevelEditor ();
	}

	public void SetLevelEditorMenuData() {
		LevelEditorMenu.instance.SetData ();
	}

	public void Exit() {
		Menu.instance.Exit ();
	}
}
