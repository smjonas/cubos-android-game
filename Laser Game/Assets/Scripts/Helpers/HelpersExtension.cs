using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

/*
  Needed to run a coroutine from inside Helpers class
*/ 

public class HelpersExtension : MonoBehaviour {

	public static HelpersExtension instance;

	void Awake() {

		if (instance == null) {
			DontDestroyOnLoad (transform.gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	public IEnumerator FadeToCoroutine<T>(T component, PropertyInfo property, Color color, float alpha, float time) {

		var a = color.a;
		var t = 0f;

		while (t < time) {

			var newAlpha = Mathf.Lerp (a, alpha / 255, t / time);
			try {
				property.SetValue (component, new Color (color.r, color.g, color.b, newAlpha), null);
			} catch (Exception e) {
				///
			}
			t += Time.deltaTime;
			yield return null;
		}

		try {
			property.SetValue (component, new Color (color.r, color.g, color.b, alpha / 255), null);
		} catch (Exception e) {
			///
		}
	}
}
