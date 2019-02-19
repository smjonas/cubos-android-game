using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonSpawner : MonoBehaviour {

	public GameObject buttonPrefab;
	public float xStart, yStart;
	public int width, height;

	void Start() {

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {

				var pos = y * width + x + 1;
				var button = Instantiate (buttonPrefab, transform);

				button.transform.localPosition = new Vector3 (x * 215 + xStart, y * (-206) + yStart);
				button.name = "Level" + pos;
				button.GetComponent<ButtonData> ().levelNumber = pos;
				button.GetComponentInChildren<TextMeshProUGUI> ().text = pos.ToString ();

				button.GetComponent<Button> ().onClick.AddListener (() => LevelSelection.instance.SelectLevel ());

			}
		}
	}
}
