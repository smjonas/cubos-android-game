using System.Collections.Generic;
using UnityEngine;

public class ScreenContainer : MonoBehaviour {

    public List<BaseScreen> activeScreens;
	public static ScreenContainer instance;

	void Awake () {

        instance = this;
        activeScreens = new List<BaseScreen>();
    }

	public void AddScreen(BaseScreen screen) {
		activeScreens.Add (screen);
	}

	public void RemoveScreen(BaseScreen screen) {
		activeScreens.Remove (screen);
	}

	public BaseScreen GetTopScreen() {
		return activeScreens [activeScreens.Count - 1];
	}

	public void CloseTopScreen() {

		var type = activeScreens [activeScreens.Count - 1].GetType ();
		var instance = type.GetField ("instance").GetValue (activeScreens [activeScreens.Count - 1]);
		type.GetMethod ("Animate").Invoke (instance, null);
	}
}
