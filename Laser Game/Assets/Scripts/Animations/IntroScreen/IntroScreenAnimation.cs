using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreenAnimation : MonoBehaviour {

	public GameObject panel;
	public GameObject logo;

	void Start () {
		StartCoroutine (Animate ());
	}

	private IEnumerator Animate() {

		logo.FadeTo<RawImage> (255f, 0.75f);
		yield return new WaitForSeconds (0.75f);
		logo.FadeTo<RawImage> (0f, 1.25f);
		yield return new WaitForSeconds (1.25f);
		panel.FadeTo<Image> (0f, 1f);
		yield return new WaitForSeconds (1f);

		panel.SetActive (false);
		logo.SetActive (false);
	}
}
