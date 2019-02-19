using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial2Spawner : BaseTutorialSpawner {

	protected override void Start () {

		tutorialNumber = 2;
		base.Start ();
	}
		
	protected override IEnumerator UpdateTutorialHand(int index) {

		LevelManager.instance.lastPulseIndex = index;
		switch (index) {

		case 1:
			DisableCubes ();
			yield return new WaitForSeconds (0.2f);
			FadeIn ();
			yield return new WaitForSeconds (0.6f);
			AnimateHand (2, new Vector3 (-0.4f, 0.65f), Swipes.Direction.UP_RIGHT, 1f, -45);
			UpdateTutorialProgress (false);
			break;

		case 2:
			DisableCubes ();
			FadeOut ();
			yield return new WaitForSeconds (0.6f);
			AnimateHand (3, new Vector3 (0.4f, -0.65f), Swipes.Direction.DOWN_LEFT, 1.25f, 115);
			UpdateTutorialProgress (false);
			break;

		case 3:
			DisableCubes ();
			WinningScreen.instance.Show (Constants.OnLevelEndScrollDownTime);
			break;			
		}
	}
}