using System.Collections.Generic;
using UnityEngine;

public class SwitchRow : BasePowerup {

	protected override void Execute(Cube cube, Swipes.Direction direction) {

		if (cube == null ||
		   (cube.pos.GetCol() != 0 && cube.pos.GetCol() != LevelManager.instance.boardWidth - 1 &&
			cube.pos.GetRow() != 0 && cube.pos.GetRow() != LevelManager.instance.boardHeight - 1)) {

			powerups.finishedAnimation = true;
			return;
		}

		var cubesInRow = new List<Cube>();
		cubesInRow.Add(cube);

		Swipes.Direction dir;

		switch (direction) {
		case Swipes.Direction.UP:
			dir = Swipes.Direction.DOWN;
			if (cube.pos.GetRow () != LevelManager.instance.boardHeight - 1) {
				powerups.finishedAnimation = true;
				return;
			}
			for (int i = 1; i < LevelManager.instance.boardHeight; i++)
				cubesInRow.Add(cubesInRow[i - 1].neighbors[Constants.Down]);
			break;

		case Swipes.Direction.DOWN:
			dir = Swipes.Direction.UP;
			if (cube.pos.GetRow () != 0) {
				powerups.finishedAnimation = true;
				return;
			}
			for (int i = 1; i < LevelManager.instance.boardHeight; i++)
				cubesInRow.Add(cubesInRow[i - 1].neighbors[Constants.Up]);
			break;

		case Swipes.Direction.LEFT:
			dir = Swipes.Direction.RIGHT;
			if (cube.pos.GetCol () != 0) {
				powerups.finishedAnimation = true;
				return;
			}
			for (int i = 1; i < LevelManager.instance.boardWidth; i++)
				cubesInRow.Add(cubesInRow[i - 1].neighbors[Constants.Right]);
			break;

		case Swipes.Direction.RIGHT:
			dir = Swipes.Direction.LEFT;
			if (cube.pos.GetCol () != LevelManager.instance.boardWidth - 1) {
				powerups.finishedAnimation = true;
				return;
			}
			for (int i = 1; i < LevelManager.instance.boardWidth; i++)
				cubesInRow.Add(cubesInRow[i - 1].neighbors[Constants.Left]);
			break;

		default:
			powerups.finishedAnimation = true;
			Debug.LogError("Wrong direction!");
			return;
		}

		var colorArray = new Color[cubesInRow.Count];

		for (int i = 0; i < cubesInRow.Count; i++) {
			colorArray[i] = cubesInRow[i].color;
		}

		// Spawn new cube at the end of the row and move towards the swipe direction
		Cube newCube = Powerups.spawner.SpawnCubeRelative(cubesInRow[cubesInRow.Count - 1], direction);
		newCube.SetColor(cubesInRow[0].color);
		newCube.pos = cubesInRow[cubesInRow.Count - 1].pos;
		newCube.SetNeighbors();
		powerups.MoveToCubeRelative(newCube, direction, Constants.CubeSwitchTime);

		// Move cube at the start of the row into invisible object and destroy after certain time
		powerups.MoveToCubeRelative(cubesInRow[0], direction, Constants.CubeSwitchTime);
		Destroy(cubesInRow[0].gameObject, Constants.CubeSwitchTime);

		var amount = direction == Swipes.Direction.LEFT || direction == Swipes.Direction.RIGHT ? 1 : LevelManager.instance.boardWidth;

		for (int j = 1; j < cubesInRow.Count; j++) {
			powerups.MoveToCube(cubesInRow[j], cubesInRow[j - 1], Constants.CubeSwitchTime);
			// Change pos according to swipe direction
			cubesInRow[j].pos = direction == Swipes.Direction.LEFT || direction == Swipes.Direction.DOWN ? cubesInRow[j].pos - amount : cubesInRow[j].pos + amount;
		}
		powerup = Constants.Powerup.SwitchRow;
		powerups.UpdateMoveInfo (Constants.Powerup.SwitchRow);

		if (lvlSpawner != null)
			lvlSpawner.RecordMove (new PowerupExe(Constants.Powerup.SwitchRow, newCube, dir));
	}
}
