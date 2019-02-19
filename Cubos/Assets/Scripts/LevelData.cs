using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour {

	public Colors colors;
    public List<Level> levels, tutorials;
	private int[] lvlTutorials;
    private int[,] markerPositions, cubePositions, pulsePositions;
	private Swipes.Direction[] moveDirections;
	private int gaps;

	public static LevelData instance;

    void Start() {

		if (instance == null) {

			DontDestroyOnLoad (transform.gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

        levels = new List<Level>();

        #region Levels 1 - 5
        // LEVEL 1
        markerPositions = new int[,] {
			{ 0, -1, 2 },
			{ 2,  0, 0 },
			{ 1,  0, 3 }
		};
		cubePositions = new int[,] {
			{ 2, -1, 0 },
			{ 0,  0, 2 },
			{ 3,  1, 0 }
		};
		SetMoveInfo (4, 0, 0);
		SetPriorTutorial (1);
		SetGaps (1);

		// LEVEL 2
		markerPositions = new int[,] {
			{ 0, 1 },
			{ 2, -1 },
			{ 3, 4 }
		};
		cubePositions = new int[,] {
			{ 2, 0 },
			{ 4, -1 },
			{ 1, 3 }
		};
		SetMoveInfo (4, 0, 0);
		SetGaps (1);

		// LEVEL 3
		markerPositions = new int[,] {
			{  0, 1, -1 },
			{  2, 3,  2 },
			{ -1, 1, -1}
		};
		cubePositions = new int[,] {
			{  3, 2, -1 },
			{  1, 0,  1 },
			{ -1, 2, -1 }
		};
		SetMoveInfo (3, 0, 0);
		SetPriorTutorial (2);
		SetGaps (3);

		// LEVEL 4 (A)
		markerPositions = new int[,] {
			{ -1, 0 },
			{ 1, 2 },
			{ 3, 4 },
			{ 0, -1 }
		};
		cubePositions = new int[,] {
			{ -1, 3 },
			{ 0, 0 },
			{ 2, 1 },
			{ 4, -1 }
		};
		SetMoveInfo (4, 0, 0);
		SetGaps (2);

		// LEVEL 5 (C)
		markerPositions = new int[,] {
			{ 2,  0,  1 },
			{ 5,  3, -1 },
			{ 4, -1, -1 }
		};
		cubePositions = new int[,] {
			{ 5,  2,  4 },
			{ 1,  0, -1 },
			{ 3, -1, -1 }
		};
		SetMoveInfo(2, 1, 0);
		SetGaps (3);
		SetPriorTutorial (3);

		// LEVEL 6 (B)
		markerPositions = new int[,] {
			{ 4, 1 },
			{ 2, 3 },
			{ 0, 4 }
		};
		cubePositions = new int[,] {
			{ 3, 4 },
			{ 4, 1 },
			{ 2, 0 }
		};
		SetMoveInfo (0, 2, 0);
		#endregion

		#region Level 6- 10
		// LEVEL 7 (D)
		markerPositions = new int[,] {
			{  3,  0,  1,  2 },
			{ -1,  4,  5, -1 },
			{  5, -1, -1,  6 }
		};
		cubePositions = new int[,] {
			{  0,  5,  3,  1 },
			{ -1,  6,  2, -1 },
			{  4, -1, -1,  5 }
		};
		SetMoveInfo (4, 1, 0);
		SetGaps (4);

		// LEVEL 8 (E)
		markerPositions = new int[,] {
			{  0, 1, -1 },
			{  2, 3, -1 },
			{ -1, 4,  6 },
			{ -1, 5,  2 }
		};
		cubePositions = new int[,] {
			{  2, 4, -1 },
			{  0, 1, -1 },
			{ -1, 5,  3 },
			{ -1, 2,  6 }
		};
		SetMoveInfo (2, 2, 0);
		SetGaps (4);

		//  LEVEL 9 [TEST_03 (APRIL 30)] 
		markerPositions = new int[,] {
			{  2,  3, 0, 0 },
			{ -1, -1, 1, 0 }
		};
		cubePositions = new int[,] {
			{  3,  0, 0, 1 },
			{ -1, -1, 2, 0 }
		};
		SetMoveInfo (1, 0, 1);
		SetPriorTutorial (4);
		SetGaps (2);

		// LEVEL 10
		markerPositions = new int[,] {
			{ 0, 1 },
			{ 1, 0 },
			{ 2, 3 }
		};
		cubePositions = new int[,] {
			{ 2, 0 },
			{ 1, 1 },
			{ 3, 0 }
		};
		SetMoveInfo(1, 1, 1);

		// LEVEL 11
		markerPositions = new int[,] {
			{ -1, 0,  1 },
			{ -1, 1,  0 },
			{  1, 0, -1 },
			{  0, 0, -1 },
			{  0, 0, -1 }
		};
		cubePositions = new int[,] {
			{ -1, 0,  0 },
			{ -1, 0,  1 },
			{  0, 1, -1 },
			{  0, 0, -1 },
			{  0, 1, -1 }
		};
		SetMoveInfo(1, 1, 1);
		SetGaps (5);
		#endregion

		#region Level 11-15
		// LEVEL 12 [TEST_04 (MAY 02)] 
		markerPositions = new int[,] {
			{ -1, 2, 0, -1 },
			{ -1, 0, 4, -1 },
			{  1, 3, 0,  0 },
		};
		cubePositions = new int[,] {
			{ -1, 0, 4, -1 },
			{ -1, 3, 0, -1 },
			{  2, 0, 0,  1 },
		};
		SetMoveInfo (1, 1, 1);
		SetGaps (4);

		// LEVEL 13
		markerPositions = new int[,] {
			{  2, 0, 0, 0 },
			{ -1, 1, 0, 0 },
			{  0, 0, 1, 2 }
		};
		cubePositions = new int[,] {
			{  0, 0, 0, 0 },
			{ -1, 1, 2, 0 },
			{  0, 1, 0, 2 }
		};
		SetMoveInfo (0, 1, 3);
		SetGaps (1);

		// LEVEL 14
		markerPositions = new int[,] {
			{ 0, 0 },
			{ 0, 3 },
			{ 0, 2 },
			{ 1, 0 },
			{ 0, 1 }
		};
		cubePositions = new int[,] {
			{ 0, 2 },
			{ 3, 0 },
			{ 0, 1 },
			{ 0, 0 },
			{ 1, 0 }
		};
		SetMoveInfo (0, 2, 1);

		// LEVEL 15
		markerPositions = new int[,] {
			{  2, 2, 2 },
			{  0, 0, 0 },
			{  1, 0, 0 },
			{ -1, 0, 0 },
			{ -1, 2, 0 }
		};
		cubePositions = new int[,] {
			{  2, 0, 0 },
			{  0, 0, 0 },
			{  0, 2, 1 },
			{ -1, 2, 0 },
			{ -1, 2, 0 }
		};
		SetMoveInfo (1, 0, 3);
		SetGaps (2);

		// LEVEL 16 
		markerPositions = new int[,] {
			{ -1, 0, 2, 0 },
			{  0, 0, 3, 0 },
			{  1, 1, 0, 0 },
		};
		cubePositions = new int[,] {
			{ -1, 0, 0, 3 },
			{  0, 2, 0, 1 },
			{  1, 0, 0, 0 },
		};
		SetMoveInfo (1, 2, 1);
		SetGaps (1);
		#endregion

		#region Level 16-20
		//LEVEL 17
		markerPositions = new int[,] {
			{ -1, 3, 5, -1, -1 },
			{  2, 0, 4,  2,  1 },
			{ -1, 5, 5, -1, -1 }
		};
		cubePositions = new int[,] {
			{ -1, 5, 5, -1, -1 },
			{  2, 0, 3,  4,  2 },
			{ -1, 1, 5, -1, -1 }
		};
		SetMoveInfo (2, 0, 2);
		SetGaps (6);

		// LEVEL 18 (lino)
		markerPositions = new int[,] {
			{  1, -1, -1, 2 },
			{  0,  0, -1, 1 },
			{  0,  2,  0, 0 },
			{ -1,  0,  3, 0 },
			{  1,  0,  0, 2 }
		};
		cubePositions = new int[,] {
			{  0, -1, -1, 0 },
			{  0,  2, -1, 2 },
			{  1,  0,  0, 1 },
			{ -1,  0,  0, 0 },
			{  0,  3,  2, 1 }
		};
		SetMoveInfo (1, 2, 2);
		SetGaps (4);

		// LEVEL 19 (E)
		markerPositions = new int[,] {
			{ 0, 1, 0, 2 },
			{ 2, 0, 1, 0 },
			{ 0, 0, 2, 0 }
		};
		cubePositions = new int[,] {
			{ 0, 2, 2, 1 },
			{ 0, 2, 0, 0 },
			{ 0, 0, 1, 0 }
		};
		SetMoveInfo (0, 1, 2);

		// LEVEL 20
		markerPositions = new int[,] {
			{  1,  2,  0, 0 },
			{  0,  3, -1, 0 },
			{ -1, -1,  4, 0 },
			{ -1, -1,  0, 5 }
		};
		cubePositions = new int[,] {
			{  1,  0,  0, 0 },
			{  3,  0, -1, 0 },
			{ -1, -1,  2, 5 },
			{ -1, -1,  0, 4 }
		};
		SetMoveInfo (2, 1, 2);
		SetGaps (5);
		#endregion

		#region Level 21-24
		// LEVEL 21
		markerPositions = new int[,] {
			{ 0,  1,  2, 3 },
			{ 2, -1,  4, 5 },
			{ 1,  3, -1, 6 },
			{ 5,  4,  2, 1 }
		};
		cubePositions = new int[,] {
			{ 3,  2,  1, 1 },
			{ 1, -1,  2, 4 },
			{ 5,  4, -1, 5 },
			{ 3,  2,  6, 0 }
		};
		SetMoveInfo (2, 0, 4);
		SetGaps (2);

		// LEVEL 22
		markerPositions = new int[,] {
			{  0,  1, -1, -1 },
			{  2,  5, -1,  3 },
			{  4, -1,  5,  4 },
			{  5,  6,  2,  1 },
			{  0,  0, -1, -1 }
		};
		cubePositions = new int[,] {
			{  0,  5, -1, -1 },
			{  1,  2, -1,  1 },
			{  0, -1,  3,  5 },
			{  6,  2,  0,  4 },
			{  5,  4, -1, -1 }
		};
		SetMoveInfo (2, 2, 2);
		SetGaps (6);

		// LEVEL 23
		markerPositions = new int[,] {
			{ -1, 1, 0, -1 },
			{  2, 3, 4,  5 },
			{  5, 4, 3,  2 },
			{ -1, 1, 0, -1 }
		};
		cubePositions = new int[,] {
			{ -1, 4, 5, -1 },
			{  3, 1, 5,  2 },
			{  2, 0, 4,  3 },
			{ -1, 1, 0, -1 }
		};
		SetMoveInfo (0, 1, 4);
		SetGaps (4);

		// LEVEL 24
		markerPositions = new int[,] {
			{ 0,  1, 2,  3, 4},
			{ 3, -1, 5,  6, 0 },
			{ 4, -1, 2, -1, 1 }
		};
		cubePositions = new int[,] {
			{ 4,   6, 0,  1, 0 },
			{ 1,  -1, 5,  2, 3 },
			{ 3,  -1, 2, -1, 4 }
		};
		SetMoveInfo (0, 2, 4);
		SetGaps (3);
		#endregion

		//////
		 
		LevelManager.instance.levelCount = levels.Count;
    
		#region Tutorial levels
		tutorials = new List<Level>();

		// TUTORIAL 1 (Switch)
		markerPositions = new int[,] {
			{ 0, 1 },
			{ 2, 0 }           
		};
		cubePositions = new int[,] {
			{ 1, 0 },
			{ 0, 2 }
		};
		pulsePositions = new int[,] {
			{ 1, 0 },
			{ 0, 2 }
		};
		moveDirections = new Swipes.Direction[] {
			Swipes.Direction.RIGHT, Swipes.Direction.LEFT
		};
		SetTutorialInfo(2, 0, 0);

		// TUTORIAL 2 (Switch diagonal)
		markerPositions = new int[,] {
			{ 0, 1 },
			{ 0, 0 },
			{ 2, 0 }
		};
		cubePositions = new int[,] {
			{ 0, 0 },
			{ 1, 2 },
			{ 0, 0 }
		};
		pulsePositions = new int[,] {
			{ 0, 0 },
			{ 1, 2 },
			{ 0, 0 }
		};
		moveDirections = new Swipes.Direction[] {
			Swipes.Direction.UP_RIGHT, Swipes.Direction.DOWN_LEFT	
		};
		SetTutorialInfo (2, 0, 0);


		// TUTORIAL 3 (Rotate)
		markerPositions = new int[,] {
			{ 2, 0 },
			{ 3, 1 }
		};
		cubePositions = new int[,] {
			{ 0, 1 },
			{ 2, 3 }
		};
		pulsePositions = new int[,] {
			{ 1, 0 },
			{ 0, 0 }
		};
		moveDirections = new Swipes.Direction[] {
			Swipes.Direction.LEFT
		};
		SetTutorialInfo (0, 1, 0);

		// TUTORIAL 4 (Switch Row)
		markerPositions = new int[,] {
			{ 2, 3, 5 },
			{ 0, 0, 1 },
			{ 0, 0, 4 }
		};
		cubePositions = new int[,] {
			{ 1, 2, 3 },
			{ 0, 0, 4 },
			{ 0, 0, 5 }
		};
		pulsePositions = new int[,] {
			{ 1, 0, 0 },
			{ 0, 0, 0 },
			{ 0, 0, 2 }
		};
		moveDirections = new Swipes.Direction[] {
			Swipes.Direction.LEFT, Swipes.Direction.DOWN
		};
		SetTutorialInfo (0, 0, 2);

		#endregion
    }

    private void SetMoveInfo(params int[] moveInfo) {
		levels.Add(new Level(colors, levels.Count, markerPositions, cubePositions, moveInfo));
    }

	private void SetGaps(int amount) {
		levels [levels.Count - 1].gaps = amount;
	}

	private void SetPriorTutorial(int tutorial) { // (1 - 4)
		levels [levels.Count - 1].priorTutorial = tutorial;
	}
		
	private void SetTutorialInfo(params int[] moveInfo) {
		tutorials.Add(new Level(colors, tutorials.Count, markerPositions, cubePositions, pulsePositions, moveDirections, moveInfo));
	}
}
