using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver : MonoBehaviour {

	static List<PowerupExe> powerups;

	public static Solver instance;

	void Awake() {

		if (instance == null) {

			DontDestroyOnLoad (transform.gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		powerups = new List<PowerupExe> ();
	}

	public void Add(PowerupExe powerup) {
		powerups.Add (PowerupFactory.GetPowerup (powerup));
	}

	public IEnumerator Execute() {

		Reverse ();

		for (int i = 0; i < powerups.Count; i++) {
			//powerups [i].Execute ();
			yield return new WaitForSeconds (Constants.CubeSwitchTime * 1.5f);
			Powerups.spawner.UpdateBoard ();
			yield return new WaitForSeconds (Constants.CubeSwitchTime);
		}

		Powerups.spawner.UpdateBoard ();
	}

	public void Reset() {
		powerups.Clear ();
	}

	public static void Reverse() {
		powerups.Reverse ();
	}
}