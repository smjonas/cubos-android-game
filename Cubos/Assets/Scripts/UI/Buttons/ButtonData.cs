using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
  Manages button states
*/

public class ButtonData : MonoBehaviour {

	public Sprite[] images;

	public enum LevelState { UNLOCKED, CURRENT, LOCKED };
	public LevelState state;

	public int levelNumber;

	void Start() {

		if (levelNumber <= LevelManager.instance.levelStates.Count) {
			state = LevelManager.instance.levelStates [levelNumber - 1];

			switch (state) {

			case LevelState.UNLOCKED:

				gameObject.GetComponent<Image> ().sprite = images [0];
				gameObject.GetComponent<Image> ().color = LevelData.instance.colors.menuColors.colors[0];//new Color(8 / 255f, 126 / 255f, 139 / 255f);
                ShowText (true);
				break;

            case LevelState.CURRENT:

                gameObject.GetComponent<Image> ().sprite = images[0];
				gameObject.GetComponent<Image> ().color = LevelData.instance.colors.menuColors.colors[1];
                ShowText(true);
                break;

			case LevelState.LOCKED:

				gameObject.GetComponent<Image> ().sprite = images [1];
				gameObject.GetComponent<Image> ().color = LevelData.instance.colors.menuColors.colors [2];
				var colorBlock = gameObject.GetComponent<Button> ().colors;
				colorBlock.pressedColor = Colors.lightGrey;
				gameObject.GetComponent<Button> ().colors = colorBlock;
                ShowText (false);
				break;
			}

			if (state != LevelState.LOCKED)
				GetComponent<Button> ().onClick.AddListener (() => DisableButton ());
		}
	}

	private void ShowText(bool show) {

		var text = gameObject.GetComponentInChildren<TextMeshProUGUI> ();
		text.alpha = show ? 1f : 0f;
	}

	private void DisableButton() {
		GetComponent<Button> ().interactable = false;
	}
}
