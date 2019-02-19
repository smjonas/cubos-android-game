using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsScreen : BaseScreen {

	public static OptionsScreen instance;

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
