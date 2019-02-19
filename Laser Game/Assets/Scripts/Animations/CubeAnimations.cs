using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeAnimations : MonoBehaviour {

	public void Play() {
		ScaleOut ();
	}

	public void Stop() {
		StartCoroutine ("ScaleBackAndStop");
	}

	private IEnumerator ScaleBackAndStop() {
		
		iTween.ScaleTo (gameObject, iTween.Hash ("scale", new Vector3 (0.75f, 0.75f), "time", Constants.CubeScaleBackTime, "easetype", iTween.EaseType.easeOutQuad));
		yield return new WaitForSeconds (0.8f);
		iTween.Stop (gameObject, "Scale");
	}

	public void ScaleOut() {
		iTween.ScaleTo (gameObject, iTween.Hash ("scale", new Vector3 (0.85f, 0.85f), "time", Constants.CubeScaleInOutTime, "easetype", iTween.EaseType.easeOutQuad, "oncomplete", "ScaleIn"));
	}

	public void ScaleIn() {
		iTween.ScaleTo (gameObject, iTween.Hash ("scale", new Vector3 (0.75f, 0.75f), "time", Constants.CubeScaleInOutTime, "easetype", iTween.EaseType.easeOutQuad, "oncomplete", "ScaleOut"));
	}
}
