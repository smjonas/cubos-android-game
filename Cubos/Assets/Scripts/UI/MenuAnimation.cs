using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MenuAnimation : MonoBehaviour {

	public Colors colors;
    public MenuCube cubePrefab;
    List<MenuCube> cubes;
    float time, maxTime;

    void Start() {
		
        LevelManager.instance.boardWidth = 5;
        LevelManager.instance.boardHeight = 9;
		LevelManager.instance.gameState = LevelManager.GameState.InMenu;

        cubes = new List<MenuCube>();
		var lvlIndex = Random.Range (0, colors.lvlSchemes.Count);

		var xStart = -2.15f;
		var yStart = -4.4f;

        for (int x = 0; x < LevelManager.instance.boardWidth; x++) {
            for (int y = 0; y < LevelManager.instance.boardHeight; y++) {

				if (y < 4 || y >= 7) {

					cubes.Add (Instantiate (cubePrefab, new Vector3 (xStart + x * 1.1f, yStart + y * 1.1f, 0), Quaternion.identity, transform));
					cubes [cubes.Count - 1].pos = y * LevelManager.instance.boardWidth + x;
					//cubes[cubes.Count - 1].SetColor(Helpers.WeightedRandomness.GetElement(50f, 50f) == 0 ? Colors.firenze3 :
					//	(Helpers.WeightedRandomness.GetElement(50f, 50f)) == 0 ? Colors.firenze2 : Colors.firenze4);
					//cubes[cubes.Count - 1].SetColor (new Color(cubes[cubes.Count - 1].color.r, cubes[cubes.Count - 1].color.g, cubes[cubes.Count - 1].color.b, 0.4f));
					cubes[cubes.Count - 1].SetColor (colors.lvlSchemes[lvlIndex].colors[Random.Range (0, 4)]);
				}
            }
        }
        foreach (var c in cubes) 
            c.SetNeighbors();

        maxTime = Random.Range(2f, 2.5f);

    }

    void Update() {

        time += Time.deltaTime;

        if (time > maxTime) {

            if (cubes.Count > 1) {
                var pos = Random.Range(0, LevelManager.instance.boardWidth * LevelManager.instance.boardHeight);
				var cube = cubes.Where (x => x.pos == pos).FirstOrDefault ();

				if (cube != null) {
					cube.RandomPowerup (cubes);

					foreach (var c in cubes)
						c.SetNeighbors ();

					time = 0f;
					maxTime = Random.Range (2f, 3f);
				}
            }
        }
    }
}
