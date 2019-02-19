using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial4Spawner : BaseTutorialSpawner {

	protected override void Start () {

		tutorialNumber = 4;
		base.Start ();

		for (int i = 0; i < invObjects.Count; i++) {
			if (i == 0) {
				invObjects [0].transform.localPosition = new Vector3 (-1.95f, -2.75f, 1f);
				invObjects [0].transform.localScale = new Vector3 (0.8f, 2.5f, 1f);
			}
			else if (i == 1) {
				invObjects [1].transform.localPosition = new Vector3 (1.45f, -1.95f, 1f);
				invObjects [1].transform.localScale = new Vector3 (0.8f, 1f, 1f);
			}
			else if (i == 2) {
				invObjects [2].transform.localPosition = new Vector3 (0.72f, -0.95f, 1f);
				invObjects [2].transform.localScale = new Vector3 (0.8f, 1f, 1f);
			}
			else if (i == 3)
				invObjects [3].transform.localScale = new Vector3 (2.6f, 0.7f, 1f);
		}
	}

	protected override IEnumerator UpdateTutorialHand(int index) {

		LevelManager.instance.lastPulseIndex = index;
		switch (index) {

		case 1:
			DisableCubes ();
			yield return new WaitForSeconds (0.2f);
			FadeIn ();
			yield return new WaitForSeconds (0.6f);
			AnimateHand (6, new Vector3 (-0.24f, 0.6f), Swipes.Direction.LEFT, 1.7f, -55);
			UpdateTutorialProgress (false);
			break;

		case 2:
			DisableCubes ();
			FadeOut ();
			yield return new WaitForSeconds (0.6f);
			AnimateHand (2, new Vector3 (0.55f, 0.15f), Swipes.Direction.DOWN, 1.7f, -150);
			UpdateTutorialProgress (false);
			break;

		case 3:
			DisableCubes ();
			WinningScreen.instance.Show (Constants.OnLevelEndScrollDownTime);
			break;			
		}
	}
}
