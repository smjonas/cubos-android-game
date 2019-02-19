using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Rotate : BasePowerup {

	private List<Cube> cubes = new List<Cube>();
	private List<int> positions = new List<int>();
	private List<Cube> cubesCopy;
	private List<int> positionsCopy;

	public void GetDirectionAndExecute(List<Cube> cubes, List<int> positions) {

		this.cubes = cubes;
		this.positions = positions;
		this.cubesCopy = new List<Cube> (cubes);
		this.positionsCopy = new List<int> (positions);

		cubes.RemoveAt(cubes.Count - 1);
		positions.RemoveAt(positions.Count - 1);

		var sorted = new List<int>(positions);
		sorted.Sort();

		if (sorted[1] != sorted[0] + 1 || sorted[3] != sorted[2] + 1 ||
			sorted[0] + LevelManager.instance.boardWidth != sorted[2]) {

			Clear();
			return;
		}

		var cubeOrder = new int[4];
		// Cube positions in order of touch as 0/1/2/3 positions
		for (int i = 0; i < cubeOrder.Length; i++)
			cubeOrder[i] = sorted.IndexOf(sorted.Find(x => x == positions[i]));

		// Right swipe
		if (cubeOrder.ContainsSequence(0, 1) || cubeOrder.ContainsSequence(3, 2))
			TryExecute(cubes.Where(c => c.pos == sorted[0]).First(), Swipes.Direction.RIGHT);
		// Left swipe
		else if (cubeOrder.ContainsSequence(1, 0) || cubeOrder.ContainsSequence(2, 3))
			TryExecute(cubes.Where(c => c.pos == sorted[1]).First(), Swipes.Direction.LEFT);

		Clear();
	}

	private void Clear() {

		cubes.Clear();
		positions.Clear();
	}

	protected override void Execute(Cube cube, Swipes.Direction direction) {

		if (cube == null) {
			powerups.finishedAnimation = true;
			return;
		}

		Cube left = cube.neighbors[Constants.Left];
		Cube leftUp = left != null ? cube.neighbors[Constants.Left].neighbors[Constants.Up] : null;
		Cube up = cube.neighbors[Constants.Up];
		Cube right = cube.neighbors[Constants.Right];
		Cube rightUp = right != null ? cube.neighbors[Constants.Right].neighbors[Constants.Up] : null;

		if (up == null) {
			powerups.finishedAnimation = true;
			return;
		}

		var tempCube = cube.neighbors;
		var tempUp = up.neighbors;
		powerup = Constants.Powerup.Rotate;

		var dir = direction == Swipes.Direction.LEFT ? Swipes.Direction.RIGHT : Swipes.Direction.LEFT;

		switch (direction) {               
		case Swipes.Direction.LEFT:

			if (cube && left && leftUp && up) {

				var tempLeft = left.neighbors;
				var tempLeftUp = leftUp.neighbors;

				powerups.MoveToCube(cube, left, Constants.CubeSwitchTime);
				cube.pos--;
				cube.neighbors = tempLeft;
				powerups.MoveToCube(left, leftUp, Constants.CubeSwitchTime);
				left.pos += LevelManager.instance.boardWidth;
				left.neighbors = tempLeftUp;
				powerups.MoveToCube(leftUp, up, Constants.CubeSwitchTime);
				leftUp.pos++;
				leftUp.neighbors = tempUp;
				powerups.MoveToCube(up, cube, Constants.CubeSwitchTime);
				up.pos -= LevelManager.instance.boardWidth;
				up.neighbors = tempCube;

				powerups.UpdateMoveInfo (Constants.Powerup.Rotate);
				if (lvlSpawner != null) {
					positionsCopy.Reverse ();
					lvlSpawner.RecordMove (new PowerupExe (Constants.Powerup.Rotate, cubesCopy, positionsCopy, Swipes.Direction.RIGHT));
				}
			}
			else powerups.finishedAnimation = true;
			break;

		case Swipes.Direction.RIGHT:

			if (cube && right && rightUp && up) {

				var tempRightUp = rightUp.neighbors;
				var tempRight = right.neighbors;

				powerups.MoveToCube(cube, right, Constants.CubeSwitchTime);
				cube.pos++;
				cube.neighbors = tempRight;
				powerups.MoveToCube(right, rightUp, Constants.CubeSwitchTime);
				right.pos += LevelManager.instance.boardWidth;
				right.neighbors = tempRightUp;
				powerups.MoveToCube(rightUp, up, Constants.CubeSwitchTime);
				rightUp.pos--;
				rightUp.neighbors = tempUp;
				powerups.MoveToCube(up, cube, Constants.CubeSwitchTime);
				up.pos -= LevelManager.instance.boardWidth;
				up.neighbors = tempCube;

				powerups.UpdateMoveInfo (Constants.Powerup.Rotate);
				if (lvlSpawner != null) {
					positionsCopy.Reverse ();
					lvlSpawner.RecordMove (new PowerupExe (Constants.Powerup.Rotate, cubesCopy, positionsCopy, Swipes.Direction.LEFT));
				}
			}
			else powerups.finishedAnimation = true;
			break;

		default:
			Debug.LogError("Wrong direction!");
			powerups.finishedAnimation = true;
			break;
		}
	}
}