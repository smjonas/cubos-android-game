using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
  Base class for UI scoll down screens / panels
*/

public abstract class BaseScreen : MonoBehaviour {

	public bool changeInvObjects;
	public BaseSpawner spawner;

	public GameObject panel;
	public GameObject tint;
	public GameObject button;
	public Vector2 offset;
	public bool scale;

	private Vector2 topPos;
	protected float scrollTime;
	private float fadeTime;
	private bool finishedAnimation;
	private UIAnimations buttonAnim;

	private void Awake() {

		// Position panel at top
		//iTween.MoveTo (gameObject, iTween.Hash ("position", new Vector3 (Screen.width / 2, Screen.height / 2)));
	}

	protected virtual void Start() {

		finishedAnimation = true;
		topPos.x = panel.transform.position.x;
		topPos.y = panel.transform.position.y;

		offset = scale ? new Vector2 (0, offset.y / 1920 * Screen.height) : offset;
		//scrollTime = scale ? Screen.height / 1920 * Constants.ScreenScrollTime : Constants.ScreenScrollTime;
		scrollTime = Constants.ScreenScrollTime;
		fadeTime = Constants.ScreenTintFadeTime;

		if (button != null)
			buttonAnim = button.GetComponent<UIAnimations> ();
	}

	protected void Disable () {

		panel.SetActive (false);
		tint.SetActive (false);

		var onClick = panel.GetComponent<CloseOnClickOutside> ();
		if (onClick != null)
			onClick.done = false;

		RemoveScreen ();
	}

	protected abstract void RemoveScreen ();

	public void Show() {

		if (finishedAnimation) {
			if (buttonAnim != null && buttonAnim.finishedAnimation) {
				if (buttonAnim.finishedAnimation)
					AddScreen ();
			} else
				AddScreen ();
		}
	}

	protected abstract void AddScreen();

	public virtual void Animate() {

		finishedAnimation = false;
		if (!panel.activeSelf) {

			if (changeInvObjects)
			for (int i = 0; i < spawner.invObjects.Count; i++)
				spawner.invObjects[i].SetActive (false);

			panel.SetActive (true);
			tint.SetActive (true);
			tint.FadeTo<Image> (140, fadeTime);
			iTween.MoveTo (panel, iTween.Hash ("position", new Vector3 (topPos.x + offset.x, topPos.y + offset.y), "time", scrollTime, "easetype", iTween.EaseType.easeInOutBack, "oncomplete", "SetFinished", "oncompleteparams", false, "oncompletetarget", gameObject));

		} else {

			tint.SetActive (true);
			tint.FadeTo<Image> (0, fadeTime);
			iTween.MoveTo (panel, iTween.Hash ("position", new Vector3 (topPos.x, topPos.y), "time", scrollTime, "easetype", iTween.EaseType.easeInOutBack, "oncomplete", "SetFinished", "oncompleteparams", true, "oncompletetarget", gameObject));
		}
	}

	public void Show(float time) {
		Invoke ("Show", time);
	}

	private void SetFinished(bool disable) {

		finishedAnimation = true;
		if (disable) {
			Disable ();
			if (changeInvObjects && ScreenContainer.instance.activeScreens.Count == 1)
				for (int i = 0; i < spawner.invObjects.Count; i++)
					spawner.invObjects [i].SetActive (true);
		}
	}
}
