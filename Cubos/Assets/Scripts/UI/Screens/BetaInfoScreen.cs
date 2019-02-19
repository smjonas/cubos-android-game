using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetaInfoScreen : BaseScreen {

	public static BetaInfoScreen instance;    

	protected override void Start() {

		base.Start ();
		instance = this;
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
