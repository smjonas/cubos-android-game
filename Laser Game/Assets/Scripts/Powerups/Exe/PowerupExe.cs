using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PowerupExe {

	public int type { get; private set; }
	public Cube cube { get; private set; }
	public List<Cube> cubes { get; private set; }
	public List<int> positions { get; private set; }
	public Swipes.Direction direction { get;set; }

	public PowerupExe(int type, List<Cube> cubes, List<int> positions, Swipes.Direction direction) : this(type, direction) {
		
		this.cubes = new List<Cube>(cubes);
		this.positions = new List<int>(positions);
	}

	public PowerupExe(int type, Cube cube, Swipes.Direction direction) : this(type, direction) {
		this.cube = cube;
	}

	private PowerupExe(int type, Swipes.Direction direction) {

		this.type = type;
		this.direction = direction;
	}
		
	/*protected void Exe () {

		var cubes = GameObject.FindObjectsOfType<Cube> ();
		cube = cubes.Where (c => c.pos == startPos).FirstOrDefault ();
	}*/

	//public abstract void Execute ();
}