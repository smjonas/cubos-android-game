using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial3Spawner : BaseTutorialSpawner {
	
	protected override void Start () {

		tutorialNumber = 3;
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
			AnimateHand (2, new Vector3(-0.5f, 0.85f));
			UpdateTutorialProgress (false);
			break;

		case 2:
			DisableCubes ();
			WinningScreen.instance.Show (Constants.OnLevelEndScrollDownTime);
			break;			
		}
	}
}