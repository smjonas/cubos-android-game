using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSprites : MonoBehaviour {

	public Sprite normalSprite, altSprite;

	void Awake() {
		UpdateSprite ();
	}

	public void UpdateSprite() {
		GetComponent<Image> ().sprite = LevelManager.instance.soundOn ? normalSprite : altSprite;
	}	
}
