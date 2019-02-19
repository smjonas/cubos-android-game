using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public AudioSource source;
	public bool fadeInOnStart;

	void Awake() {

		UpdateVolume ();
		if (fadeInOnStart)
			Fade (1f, Constants.MusicFadeInTime);
	}

	public void Fade(float endVol, float time) {

		StopCoroutine ("FadeRoutine");
		StartCoroutine ("FadeRoutine", new float[] { endVol, time });
	}

	private IEnumerator FadeRoutine(float[] args) {

		var endVol = args [0];
		var time = args [1];
		var startVol = source.volume;
		var t = 0f;

		while (t < time) {
			
			source.volume = Mathf.Lerp (startVol, endVol, t / time);
			t += Time.deltaTime;
			yield return null;
		}
		source.volume = endVol;
	}

	public void ToggleSound() {

		LevelManager.instance.soundOn = source.mute;
		UpdateVolume ();
		LevelManager.instance.Save ();
	}

	private void UpdateVolume() {
		source.mute = !LevelManager.instance.soundOn;
	}

	public void ResetVolume() {
		source.volume = 0f;
	}
}


