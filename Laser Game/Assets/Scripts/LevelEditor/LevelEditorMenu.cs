using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class LevelEditorMenu : MonoBehaviour {

	public static LevelEditorMenu instance;

	void Awake () {

		if (instance == null) {

			DontDestroyOnLoad (transform.gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	public void SetData() {

		int number;
		if (Int32.TryParse (GameObject.Find ("Width").GetComponent<TMP_InputField> ().text, out number))
			LevelManager.instance.boardWidth = number;
		if (Int32.TryParse (GameObject.Find ("Height").GetComponent<TMP_InputField> ().text, out number))
			LevelManager.instance.boardHeight = number;
	}
}
