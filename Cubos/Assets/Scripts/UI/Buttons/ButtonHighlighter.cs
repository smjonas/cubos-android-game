using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonHighlighter : MonoBehaviour {

	public List<Button> buttons;
	private string currentLanguage, newLanguage;

	void Start() {
		
		currentLanguage = LevelManager.instance.language;
		newLanguage = currentLanguage;

		for (int i = 0; i < buttons.Count; i++) {

			if (buttons [i].name.Equals (LevelManager.instance.language))
				Select (i);
		}
	}

	public void Select(int index) {
		
		DeselectAll ();
		buttons [index].GetComponent<Image> ().color = Color.white;

		newLanguage = buttons [index].name;
	}

	private void DeselectAll() {

		foreach (var b in buttons) {
			b.GetComponent<Image> ().color = Color.grey;
		}
	}

	public void ApplyNewLanguage() {
		LevelManager.instance.language = newLanguage;
	}

	public void RevertToCurrentLanguage() {
		LevelManager.instance.language = currentLanguage;
	}

}
