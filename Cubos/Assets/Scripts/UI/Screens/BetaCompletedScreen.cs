using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetaCompletedScreen : BaseScreen {

	public static new BetaCompletedScreen instance;

	protected override void Start() {

		base.Start ();
		instance = this;

		if (spawner != null) {
			for (int i = 0; i < spawner.invObjectPrefabs.Count; i++)
				spawner.invObjectPrefabs [i].SetActive (false);
		}
	}

	protected override void AddScreen () {

		if (!panel.activeSelf) {
			ScreenContainer.instance.AddScreen (instance);
		}
		base.Animate ();
	}

	protected override void RemoveScreen() {
		ScreenContainer.instance.RemoveScreen (instance);
	}
}
