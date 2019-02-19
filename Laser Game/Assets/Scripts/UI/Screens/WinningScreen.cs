using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WinningScreen : BaseScreen {

	public Button nextLevelButton;
	public static WinningScreen instance;

	protected override void Start() {

		base.Start ();
		instance = this;

		if (LevelManager.instance.gameState == LevelManager.GameState.InTutorial) {
			if (nextLevelButton != null)
				nextLevelButton.onClick.AddListener (() => LevelSelection.instance.PlayLevel (LevelManager.instance.currentLevel));
		}

		if (spawner != null) {
			for (int i = 0; i < spawner.invObjectPrefabs.Count; i++)
				spawner.invObjectPrefabs [i].SetActive (false);
		}
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
