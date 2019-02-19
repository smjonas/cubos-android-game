using UnityEngine;
using System.Collections;

public class SceneFading : MonoBehaviour {

	public Texture2D fadeOutTexture;
	public bool fadeInOnStart;
	public float fadeInTime = 1f, maxFadeInAlpha = 1f;
	public float fadeOutTime = 0.8f, maxFadeOutAlpha = 1f;

	private bool fadeBlack;
	private float alpha;

	void Start() {
		
		if (fadeInOnStart)
			StartCoroutine (BeginFade ());
	}

	public void FadeBlack() {

		fadeBlack = true;
		StartCoroutine (BeginFade ());
	}

	public void FadeWhite() {

		fadeBlack = false;
		StartCoroutine (BeginFade ());
	}


	private IEnumerator BeginFade() {

		var time = 0f;
		if (fadeBlack) {

			while (time < fadeOutTime) {
				
				alpha = Mathf.Lerp (0f, maxFadeOutAlpha, time / fadeOutTime);
				time += Time.deltaTime;
				yield return null;
			}
			alpha = maxFadeOutAlpha;
		} 

		else {

			while (time < fadeInTime) {
				
				alpha = Mathf.Lerp (maxFadeInAlpha, 0f, time / fadeInTime);
				time += Time.deltaTime;
				yield return null;
			}
			alpha = 0f;
		}
	}

	void OnGUI () {

		alpha = Mathf.Clamp01(alpha);
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = -1000;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
	}
}
