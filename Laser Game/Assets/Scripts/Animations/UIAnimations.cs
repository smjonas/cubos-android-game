using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIAnimations : MonoBehaviour {
	
	private bool clicked;
	public bool pulse;
	[HideInInspector] public bool finishedAnimation = true;

	private Color color;

	void Start() {
		if (pulse)
			StartCoroutine (FadeIn ());
	}

	public void Rotate180() {

		if (finishedAnimation) {
			finishedAnimation = false;
			if (!clicked) {
				iTween.RotateBy (gameObject, iTween.Hash ("z", 0.5f, "time", 0.7f, "easetype", iTween.EaseType.easeOutQuint, "oncomplete", "SetFinished"));
				clicked = true;
			} else {
				iTween.RotateBy (gameObject, iTween.Hash ("z", -0.5f, "time", 0.7f, "easetype", iTween.EaseType.easeOutQuint, "oncomplete", "SetFinished"));
				clicked = false;
			}
		}
	}

	public void Rotate180(string dir) {

		if (finishedAnimation) {
			finishedAnimation = false;
			if (dir.Equals ("left"))
				iTween.RotateBy (gameObject, iTween.Hash ("z", 0.5f, "time", 0.7f, "easetype", iTween.EaseType.easeOutQuint, "oncomplete", "SetFinished"));
			else if (dir.Equals ("right"))
				iTween.RotateBy (gameObject, iTween.Hash ("z", -0.5f, "time", 0.7f, "easetype", iTween.EaseType.easeOutQuint, "oncomplete", "SetFinished"));
		}
	}

	public void PulseSize(Color color) {
		
		this.color = color;
		StartCoroutine(IncreaseSize ());
	}

	private IEnumerator IncreaseSize() {
		
		iTween.ScaleTo (gameObject, iTween.Hash ("scale", new Vector3 (1.35f, 1.35f), "time", Constants.CubeScaleBackTime, "easetype", iTween.EaseType.easeOutQuad));
		iTween.ValueTo(gameObject, iTween.Hash ("from", 0, "to", 255, "time", Constants.CubeScaleBackTime, "easetype", iTween.EaseType.linear, "onupdate", "ChangeColor"));
		yield return new WaitForSeconds (Constants.CubeScaleBackTime);
		DecreaseSize ();
	}

	private void DecreaseSize() {
		
		iTween.ScaleTo (gameObject, iTween.Hash ("scale", new Vector3 (1f, 1f), "time", Constants.CubeScaleBackTime, "easetype", iTween.EaseType.easeOutQuad));
		iTween.ValueTo(gameObject, iTween.Hash ("from", 255, "to", 0, "time", Constants.CubeScaleBackTime, "easetype", iTween.EaseType.linear, "onupdate", "ChangeColor"));
	}

	private void ChangeColor(int val) {
		GetComponent<TextMeshProUGUI> ().faceColor = new Color32((byte) (val * color.r), (byte) (val * color.g), (byte) (val * color.b), 255);
	}

	private void SetFinished() {
		finishedAnimation = true;
	}

	private IEnumerator FadeIn() {

		gameObject.FadeTo<TextMeshProUGUI> (255f, 1f);
		yield return new WaitForSeconds (1f);
		StartCoroutine (FadeOut ());
	}

	private IEnumerator FadeOut() {
		
		gameObject.FadeTo<TextMeshProUGUI> (127.5f, 1f);
		yield return new WaitForSeconds (1f);
		StartCoroutine (FadeIn ());
	}
}