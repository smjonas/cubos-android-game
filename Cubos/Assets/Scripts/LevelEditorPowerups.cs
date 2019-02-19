using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorPowerups : Powerups {

	protected override void Start () {
		base.Start ();
	}

	public override IEnumerator ExecutePowerup (int powerup, List<Cube> cubes, List<int> positions, Swipes.Direction direction) {

		var sp = GameObject.Find ("EditorSpawner").GetComponent<LevelEditorSpawner> ();
		sp.UpdateBoard ();
		GestureDetector.ready = false;

		switch (powerup) {
		case 0:
			switch_.TryExecute(cubes[0], direction);
			break;
		case 1:
			rotate.GetDirectionAndExecute(cubes, positions);
			break;
		case 2:
			switchRow.TryExecute(cubes[0], direction);
			break;
		default:
			Debug.LogError("Powerup not defined!");
			break;
		}
		yield return new WaitForSeconds(Constants.CubeSwitchTime * 1.2f);
		sp.UpdateBoard ();
		GestureDetector.ready = true;
	}
}
