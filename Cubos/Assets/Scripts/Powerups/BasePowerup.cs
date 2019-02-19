using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePowerup : MonoBehaviour {

	protected LevelSpawner lvlSpawner;
	protected BaseTutorialSpawner tutSpawner;
	protected Powerups powerups;
	protected int powerup;

	protected void Start() {
		
		powerups = GameObject.FindObjectOfType<Powerups> ();
		if (LevelManager.instance.gameState == LevelManager.GameState.InLevel)
			lvlSpawner = (LevelSpawner)Powerups.spawner;
		else if (LevelManager.instance.gameState == LevelManager.GameState.InTutorial)
			tutSpawner = (BaseTutorialSpawner)Powerups.spawner;
	}

	public void TryExecute(Cube cube, Swipes.Direction direction) {

		try {
			if (LevelManager.instance.gameState == LevelManager.GameState.InTutorial) {
				if (LevelData.instance.tutorials [LevelManager.instance.currentTutorial - 1].moveDirections [LevelManager.instance.pulseIndex - 1] == direction) {
			
					Execute (cube, direction);
					LevelManager.instance.pulseIndex++;
				}
				else {
					//LevelManager.instance.lastPulseIndex = LevelManager.instance.pulseIndex - 1;
					//LevelManager.instance.pulseIndex = LevelManager.instance.lastPulseIndex + 1;
				}
			} else
				Execute (cube, direction);
		} catch (System.Exception ex) {
			Debug.LogError ("BasePowerup: " + ex.GetBaseException ());
		}		
	}

	protected abstract void Execute (Cube cube, Swipes.Direction direction);
}
