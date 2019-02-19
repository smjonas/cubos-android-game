using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CloseOnClickOutside : MonoBehaviour {

	[HideInInspector] public bool done;

	void Update() {

		if (gameObject.activeSelf) {
			if (Input.touchCount > 0) {

				var rect = gameObject.GetComponent<RectTransform> ();
				var touch = Input.GetTouch (0).position;
				var camera = LevelManager.instance.gameState == LevelManager.GameState.InMenu ? null : Camera.main;

				if (!(RectTransformUtility.RectangleContainsScreenPoint (rect, touch, camera)) && !done) {

					if (ScreenContainer.instance.GetTopScreen ().Equals (GetComponentInParent<BaseScreen> ())) {
						ScreenContainer.instance.CloseTopScreen ();
						done = true;
					}
				}
			}
		}
	}
}
