using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseScreen : BaseScreen {

	public TextMeshProUGUI levelName;
	public static PauseScreen instance;

	protected override void Start () {

		base.Start();
		instance = this;

		var state = LevelManager.instance.gameState;
		if (state == LevelManager.GameState.InLevel)
			levelName.text = "Level " + LevelManager.instance.currentLevel;
		else if (state == LevelManager.GameState.InTutorial)
			levelName.text = "Tutorial " + LevelManager.instance.currentTutorial;
	}

	protected override void AddScreen () {

		if (!panel.activeSelf) {
			ScreenContainer.instance.AddScreen (instance);
		}
		base.Animate ();
	}

	protected override void RemoveScreen() {
		ScreenContainer.instance.RemoveScreen (instance);
	}

}
