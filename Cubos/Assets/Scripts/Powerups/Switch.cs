using UnityEngine;

public class Switch : BasePowerup {
	
	protected override void Execute(Cube cube, Swipes.Direction direction) {
		powerups.SwitchCubes (cube, cube.neighbors [(int)direction], direction);
	}
}
