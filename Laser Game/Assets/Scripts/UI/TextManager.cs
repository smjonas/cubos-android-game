using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour {

	public bool levelEndText;
	
	void OnEnable() {

		var textPanel = GetComponent<TextMeshProUGUI> ();
		var langManager = new LanguageManager (LevelManager.instance.language);

		if (!levelEndText) {
			// Set the text of the text panel
			textPanel.text = langManager.strings [name].ToString ();
		} else
			textPanel.text = "LEVEL " + LevelManager.instance.currentLevel + " " + langManager.strings [name].ToString ();
	}
}
