using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial1Spawner : BaseTutorialSpawner {

	protected override void Start () {

		tutorialNumber = 1;
		base.Start ();
	}

	protected override IEnumerator UpdateTutorialHand(int index) {
		
		LevelManager.instance.lastPulseIndex = index;
		switch (index) {

		case 1:
			DisableCubes ();
			changeMoveInfo = false;
			yield return new WaitForSeconds (0.2f);
			FadeIn ();
			yield return new WaitForSeconds (0.6f);
			AnimateHand (2, new Vector3 (-0.3f, 0.75f), Swipes.Direction.RIGHT, 1.7f, -55);
			UpdateTutorialProgress (false);
			break;

		case 2:
			DisableCubes ();
			FadeOut ();
			yield return new WaitForSeconds (0.6f);

			AnimateHand (new Vector3 (0f, 3.8f), 40);
			yield return new WaitForSeconds (1.25f);
			powerupMovesLeft [Constants.Powerup.Switch].GetComponent<UIAnimations> ().PulseSize (Colors.powerupUsed);
			LevelManager.instance.moveInfo [Constants.Powerup.Switch]--;
			LevelManager.instance.movesLeft--;
			UpdatePowerupIcons ();
			yield return new WaitForSeconds (1.25f);

			changeMoveInfo = true;
			AnimateHand (1, new Vector3 (0.3f, -0.75f), Swipes.Direction.LEFT, 1.7f, 35);
			UpdateTutorialProgress (false);
			break;

		case 3:
			DisableCubes ();
			DestroyHand ();
			WinningScreen.instance.Show (Constants.OnLevelEndScrollDownTime);
			break;			
		}
	}
}