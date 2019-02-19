using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LevelGenerator : MonoBehaviour {

	#region initialization

	int mainColor;
	int width, height, maxPos;
	List<int> colors;
	List<int> colorCount;
	List<int> colorLine;
	List<int> positions;
	bool randomize;

	public int movesLeft;
	public int[] moveInfo;
	public int[,] cubePositions, markerPositions;
	public int[] colorRange;
	public int[] powerups;
	List<int> powerupOrder;

	List<string> solverMoveInfo;
	string levelInfo;
	Swipes.Direction switchDirection;

	public static LevelGenerator instance;

	void Awake() {

		if (instance == null) {

			DontDestroyOnLoad (transform.gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
			
		UpdateInfo ();
	}
					
	public void Generate() {

		Solver.instance.Reset ();
		colorLine = new List<int> ();
		positions = new List<int> ();
		solverMoveInfo = new List<string> ();
		levelInfo = string.Empty;

		mainColor = Random.Range (0, 7 + 1);
		movesLeft = movesLeft == 1 ? Random.Range (2, 6 + 1) : movesLeft;
		moveInfo = new int[3];

		colorRange = new int[] { mainColor, colors[0], colors[1], colors[2], colors[3] };
		powerups = new int[3];

		for (int i = 0; i <= 2; i++)
			powerups [i] = Random.Range (0, 3);

		if (randomize) {

			var randWidth = Helpers.WeightedRandomness.GetElement ( 50f, 20f, 15f, 15f );
			switch (randWidth) {
			case 1:
				width = 3; // 20%
				break;
			case 2:
				width = 2; // 15%
				break;
			case 3:
				width = 5; // 15%
				break;
			default:
				width = 4; // 50%
				break;
			}

			var randHeight = Helpers.WeightedRandomness.GetElement ( 40f, 25f, 25f, 10f );
			switch (randHeight) {
			case 1:
				height = 4; // 25%
				break;
			case 2:
				height = 5; // 25%
				break;
			case 3:
				if (width != 2)
				height = 2; // 10%
				else height = 3;
				break;
			default:
				height = 3; // 40%
				break;
			}
			maxPos = height * width - 1;


			var count = 0;

			for (int i = 0; i <= 3; i++) {
				if (count <= movesLeft) {
					colors[i] = Random.Range (0, 7 + 1);
					if (colors [i] != mainColor) {
						var rand = Random.Range (1, 4 + 1);
						colorCount [i] = count + rand <= movesLeft ? rand : 0;
						count += colorCount [i];
					} else {
						colors [i] = Random.Range (0, 7 + 1);
						var rand = Random.Range (1, 4 + 1);
						colorCount [i] = count + rand <= movesLeft ? rand : 0;
						count += colorCount [i];
					}
				}
			}
		}

		for (int c1 = 0; c1 < colorCount [0]; c1++) {
			colorLine.Add (colors[0]);
		}
		for (int c2 = 0; c2 < colorCount [1]; c2++) {
			colorLine.Add (colors[1]);
		}
		for (int c3 = 0; c3 < colorCount [2]; c3++) {
			colorLine.Add (colors[2]);
		}
		for (int c4 = 0; c4 < colorCount [3]; c4++) {
			colorLine.Add (colors[3]);
		}

		cubePositions = new int[height, width];
		markerPositions = new int[height, width];

		for (int line = 0; line < markerPositions.GetLength (0); line++) {
			for (int col = 0; col < markerPositions.GetLength (1); col++) {

				markerPositions [line, col] = 0;

				var pos = line * markerPositions.GetLength (1) + col;
				if (pos < colorLine.Count) {

					// Add index of item from colorLine to objective (+1 --> no main color)
					markerPositions [line, col] = colorLine.Distinct().ToList().IndexOf(colorLine [pos]) + 1;
				}
			}
		}
		markerPositions = markerPositions.Shuffle ();
		cubePositions = markerPositions.Clone () as int[,];

		for (int l = 0; l < cubePositions.GetLength (0); l++) {
			for (int c = 0; c < cubePositions.GetLength (1); c++) {

				var pos = l * cubePositions.GetLength (1) + c;

				if (cubePositions [l, c] != 0)
					positions.Add (pos);
			}
		}

        if (positions.Distinct().Count() <= 1)
            Generate();

		UsePowerups();

		solverMoveInfo.Reverse ();
		foreach (var f in solverMoveInfo)
			Debug.LogError (f);

		LevelManager.instance.UpdateLevelGenInfoAndLoad (cubePositions);
		Menu.instance.Play ();	
	}

	#endregion initialization

	public void SaveData() {

		// Level info for saving to LevelInfo.txt
		var objectiveCopy = markerPositions.Clone () as int[,];
		var mirroredObjective = objectiveCopy.Mirror();

		var cubePositionsCopy = cubePositions.Clone () as int[,];
		var mirroredCubePosition = cubePositionsCopy.Mirror ();

		int[,] currentInfo;

		for (int i = 1; i <= 2; i++) {

			currentInfo = i == 1 ? mirroredObjective : mirroredCubePosition;

			for (var line = 0; line < currentInfo.GetLength (0); line++) {

				levelInfo += "{ ";
				for (var col = 0; col < currentInfo.GetLength (1); col++) {

					levelInfo += currentInfo[line, col];
					if (col < currentInfo.GetLength (1) - 1)
						levelInfo += ", ";
					else
						levelInfo += " ";
				}
				if (line < currentInfo.GetLength (0) - 1)
					levelInfo += "},\n";
				else
					levelInfo += "}\n";

				if (line == 0 && i == 1)
					TextToFile.WriteLine (levelInfo, true, false);
				else if (line == 0 && i == 2)
					TextToFile.WriteLine (levelInfo, false, true);
				else
					TextToFile.WriteLine (levelInfo);

				levelInfo = string.Empty;

			}
		}

		TextToFile.WriteLine ("Moves left: " + movesLeft, false, true);

		foreach (var i in solverMoveInfo)
			TextToFile.WriteLine (i);

		TextToFile.Write ("Colors: ", true);
		/*TextToFile.Write (Constants.ColorsString [colorRange[0]] + ", ", false);
		for (var i = 1; i < colorRange.Length; i++) {
			
			if (colorCount [i - 1] > 0) {
				TextToFile.Write (Constants.ColorsString [colorRange[i]], false);
				if (i < colorRange.Length - 1)
					TextToFile.Write (", ", false);
			}
		}*/
	}

	void UsePowerups() {

		//var moveDistribution = movesLeft.RandomDistribution (cubePositions.Length);
		var moveDistribution = new int[movesLeft];
		moveDistribution.SetAllValues (1);

		powerupOrder = movesLeft.RandomDistribution (powerups.Count ()).ToOrderList ();

		int currentPosition = 0;
		
		/*foreach (var p in powerupOrder)
			print ("PU ORDER " + p);*/

		//print ("SWitch count " + powerupOrder.GetItemCount (Constants.Powerup.Switch));

		/*if (powerupOrder.GetItemCount (Constants.Powerup.Switch) > 1) {
	
			if (Helpers.WeightedRandomness.GetElement (new float[] { 50f, 50f }) == 0) {
				// Remove switch powerup once
				print("REMOVE");
				powerupOrder.RemoveAt (0);
				moveDistribution [0] = 2;
			}
		}*/

		powerupOrder.ShuffleList ();

		for (int i = 0; i < movesLeft; i++) {

			var powerup = powerups[powerupOrder [i]];

			switch (powerup) {

			case 0: 
                UseSwitch(currentPosition, moveDistribution);
                break;
                
            case 1:
                UseRotate();
                break;

			case 2:
                UseSwitchRow();
                break;
			
			default:
				Debug.LogError ("Powerup type " + powerup + " not defined!");
				break;								

			}
            currentPosition++;
		}

		// Regenerate level
		if (solverMoveInfo.Count != movesLeft) {
			Debug.LogError ("Regenerating!");
			Generate ();
		}
	}

    #region Switch
    bool UseSwitch(int currentPosition, int[] moveDistribution) {

        var maxIterations = 12;
        // Choose next cube
        var pos = positions[Random.Range(0, positions.Count)];

        // Choose random neighbor of cube and call RandomSwitch method again if cubes at both positions are of the same color
        // or if objective at this position is of the same as the cube itself
        var endPos = RandomSwitch(pos, moveDistribution[currentPosition]);

        while ((endPos == pos || cubePositions[GetRow(endPos), GetCol(endPos)] == cubePositions[GetRow(pos), GetCol(pos)]
               || markerPositions[GetRow(endPos), GetCol(endPos)] == cubePositions[GetRow(pos), GetCol(pos)]
               || markerPositions[GetRow(pos), GetCol(pos)] == cubePositions[GetRow(endPos), GetCol(endPos)])
               && maxIterations > 0) {

            endPos = RandomSwitch(pos, moveDistribution[currentPosition]);
            //pos = positions[Random.Range(0, positions.Count)]; !!!!!!!!!!!!!!!
            //print("Temp startpos " + pos);
            //print("Temp endpos " + endPos);
            maxIterations--;
        }

        if (maxIterations == 0) {
            // Change powerup
            Debug.LogError("Max iterations reached!");
            //powerups[powerupOrder[i]] = Random.Range(0, 1 + 1) == 0 ? 1 : 2;
            return false;
        }

        // Adapt movesLeft
        movesLeft -= moveDistribution[currentPosition] - 1;

        // Switch positions
        var temp = cubePositions[GetRow(pos), GetCol(pos)];
        cubePositions[GetRow(pos), GetCol(pos)] = cubePositions[GetRow(endPos), GetCol(endPos)];
        cubePositions[GetRow(endPos), GetCol(endPos)] = temp;

		// Increment move info
		moveInfo[0]++;

        // Add solver move info (reversed)
        //Solver.instance.Add(new SwitchExe(pos, switchDirection));
        solverMoveInfo.Add("Switch from " + GetRow(pos) + "|" + GetCol(pos) + " to " + GetRow(endPos) + "|" + GetCol(endPos));

        return true;
    }

    int RandomSwitch(int pos, int moves) {

        var startPos = pos;
        int endPos = startPos;
        var count = moves;

        print("Moves " + moves);

        while (count > 0) {
            endPos = ChooseRandomNeighbor(pos);
            pos = endPos;
            count--;
        }

        // Check for valid switch (max. 2 rows/cols distance)
        if (Mathf.Abs(GetRow(startPos) - GetRow(endPos)) >= moves ||
            Mathf.Abs(GetCol(startPos) - GetCol(endPos)) >= moves)
            return endPos;

        else {
            switchDirection = Swipes.Direction.NONE;
            return RandomSwitch(pos, moves);
        }
    }

    int ChooseRandomNeighbor(int pos) {

        var direction = Random.Range(0, 8);
        var endPos = -1;

        switch (direction) {

            case 0: // Left
                if (pos - 1 >= 0 && GetRow(pos - 1) == GetRow(pos))
                    endPos = pos - 1;
                break;
            case 1: // Right
                if (pos + 1 <= maxPos && GetRow(pos + 1) == GetRow(pos))
                    endPos = pos + 1;
                break;
            case 2: // Up
                if (pos + width <= maxPos)
                    endPos = pos + width;
                break;
            case 3: // Down
                if (pos - width >= 0)
                    endPos = pos - width;
                break;
            case 4: // Up left
                if (pos + width - 1 <= maxPos && GetRow(pos + width - 1) == GetRow(pos) + 1)
                    endPos = pos + width - 1;
                break;
            case 5: // Up right
                if (pos + width + 1 <= maxPos && GetRow(pos + width + 1) == GetRow(pos) + 1)
                    endPos = pos + width + 1;
                break;
            case 6: // Down left
                if (pos - width - 1 >= 0 && GetRow(pos - width - 1) == GetRow(pos) - 1)
                    endPos = pos - width - 1;
                break;
            case 7: // Down right
                if (pos - width + 1 >= 1 && GetRow(pos - width + 1) == GetRow(pos) - 1)
                    endPos = pos - width + 1;
                break;
        }

        if (endPos == -1)
            return ChooseRandomNeighbor(pos);
        else {
			switchDirection = (Swipes.Direction) direction;
            return endPos;
        }
    }
    #endregion Switch

    #region Rotate
    bool UseRotate() {

        var maxIterations = 14;
        // Choose random cube
        var pos = Random.Range(0, maxPos);
        var direction = Random.Range(0, 1 + 1) == 0 ? Swipes.Direction.LEFT : Swipes.Direction.RIGHT;

        while (!IsValidRotatePosition(pos, direction) && maxIterations > 0) {
            pos = Random.Range(0, maxPos);
            //direction = Random.Range(0, 1 + 1) == 0 ? Swipes.Direction.LEFT : Swipes.Direction.RIGHT;
            maxIterations--;
        }

        if (maxIterations == 0) {
            // Change powerup
            Debug.LogError("Max iterations reached!");
            //powerups[powerupOrder[i]] = Random.Range(0, 1 + 1) == 0 ? 0 : 2;
            return false;
        }

        var pos0 = direction == Swipes.Direction.LEFT ? pos : pos + 1;
        var pos1 = direction == Swipes.Direction.LEFT ? pos - 1 : pos;
        var pos2 = direction == Swipes.Direction.LEFT ? pos + width : pos + 1 + width;
        var pos3 = direction == Swipes.Direction.LEFT ? pos - 1 + width : pos + width;

        var temp0 = cubePositions[GetRow(pos0), GetCol(pos0)];
        var temp1 = cubePositions[GetRow(pos1), GetCol(pos1)];
        var temp2 = cubePositions[GetRow(pos2), GetCol(pos2)];
        var temp3 = cubePositions[GetRow(pos3), GetCol(pos3)];

        // Switch positions
        if (direction == Swipes.Direction.LEFT) {

            cubePositions[GetRow(pos), GetCol(pos)] = temp2;
            cubePositions[GetRow(pos - 1), GetCol(pos - 1)] = temp0;
            cubePositions[GetRow(pos - 1 + width), GetCol(pos - 1 + width)] = temp1;
            cubePositions[GetRow(pos + width), GetCol(pos + width)] = temp3;

        }
        else if (direction == Swipes.Direction.RIGHT) {

            cubePositions[GetRow(pos + 1), GetCol(pos + 1)] = temp1;
            cubePositions[GetRow(pos), GetCol(pos)] = temp3;
            cubePositions[GetRow(pos + width), GetCol(pos + width)] = temp2;
            cubePositions[GetRow(pos + 1 + width), GetCol(pos + 1 + width)] = temp0;
        }
		// Increment move info
		moveInfo[1]++;

		// Add solver move info (reversed)
        var undoPos = direction == Swipes.Direction.LEFT ? pos - 1 : pos + 1;
        var undoDir = direction == Swipes.Direction.LEFT ? Swipes.Direction.RIGHT : Swipes.Direction.LEFT;

        //Solver.instance.Add(new RotateExe(undoPos, undoDir));
        solverMoveInfo.Add("Rotate 2x2 from " + GetRow(undoPos) + "|" + GetCol(undoPos) + " " + undoDir);

        return true;
    }

    bool IsValidRotatePosition(int pos, Swipes.Direction direction) {

        bool isValidPosition = false;

        switch (direction) {

            case Swipes.Direction.LEFT:

                /*
                X X X X X X
                X O O O O O
                X O O O O O
                X O O O O O
                         */

                isValidPosition = GetRow(pos) < height - 1 && GetCol(pos) > 0;
                break;
            case Swipes.Direction.RIGHT:

                /*
                X X X X X X
                O O O O O X
                O O O O O X
                O O O O O X
                         */

                isValidPosition = GetRow(pos) < height - 1 && GetCol(pos) < width - 1;
                break;
        }

        if (!isValidPosition || positions.Count < 2)
            return false;

        // 3 2
        // 1 0 <-- left: @arg pos = 0, right: @arg pos = 1

        var pos0 = direction == Swipes.Direction.LEFT ? pos : pos + 1;
        var pos1 = direction == Swipes.Direction.LEFT ? pos - 1 : pos;
        var pos2 = direction == Swipes.Direction.LEFT ? pos + width : pos + 1 + width;
        var pos3 = direction == Swipes.Direction.LEFT ? pos - 1 + width : pos + width;

        var valid0 = cubePositions[GetRow(pos0), GetCol(pos0)] != 0;
        var valid1 = cubePositions[GetRow(pos1), GetCol(pos1)] != 0;
        var valid2 = cubePositions[GetRow(pos2), GetCol(pos2)] != 0;
        var valid3 = cubePositions[GetRow(pos3), GetCol(pos3)] != 0;

        return ((valid0 && (valid1 || valid2 || valid3)) ||
                 (valid1 && (valid2 || valid3)) ||
                 (valid2 && valid3)
        );
    }
    #endregion Rotate

    #region SwitchRow
    bool UseSwitchRow() {

        var maxIterations = (width * height) / 2;
        var pos = Random.Range(0, maxPos);
        var direction = GetValidSwitchRowPosition(pos);

        int undoPos = 0;
        Swipes.Direction undoDir = Swipes.Direction.NONE;

        switch (direction) {
            case Swipes.Direction.LEFT:
                undoPos = GetRow(pos) * width + (width - 1);
                undoDir = Swipes.Direction.RIGHT;
                break;
            case Swipes.Direction.RIGHT:
                undoPos = GetRow(pos) * width;
                undoDir = Swipes.Direction.LEFT;
                break;
            case Swipes.Direction.UP:
                undoPos = GetCol(pos);
                undoDir = Swipes.Direction.DOWN;
                break;
            case Swipes.Direction.DOWN:
                undoPos = GetRow(maxPos) * width + GetCol(pos);
                undoDir = Swipes.Direction.UP;
                break;
        }

        while (direction == Swipes.Direction.NONE && maxIterations > 0) {

            pos = Random.Range(0, maxPos);
            direction = GetValidSwitchRowPosition(pos);
            print("Valid direction: " + direction);

            switch (direction) {
                case Swipes.Direction.LEFT:
                    undoPos = GetRow(pos) * width + (width - 1);
                    undoDir = Swipes.Direction.RIGHT;
                    break;
                case Swipes.Direction.RIGHT:
                    undoPos = GetRow(pos) * width;
                    undoDir = Swipes.Direction.LEFT;
                    break;
                case Swipes.Direction.UP:
                    undoPos = GetCol(pos);
                    undoDir = Swipes.Direction.DOWN;
                    break;
                case Swipes.Direction.DOWN:
                    undoPos = GetRow(maxPos) * width + GetCol(pos);
                    undoDir = Swipes.Direction.UP;
                    break;
            }

            maxIterations--;
        }

        if (maxIterations == 0) {
            // Change powerup
            Debug.LogError("Max iterations reached!");
            //powerups[powerupOrder[i]] = Random.Range(0, 1 + 1) == 0 ? 0 : 1;
            return false;
        }

        cubePositions.ShiftByOne(pos, direction);

		// Increment move info
		moveInfo[2]++;

		// Add solver move info (reversed)
        //Solver.instance.Add(new SwitchRowExe(undoPos, undoDir));
        solverMoveInfo.Add("Switch row from " + GetRow(undoPos) + "|" + GetCol(undoPos) + " " + undoDir);

        return true;

    }

    Swipes.Direction GetValidSwitchRowPosition(int pos) {

        for (int i = 0; i <= 3; i++) {

            switch (i) {
                case 0:
                    if (IsValidSwitchRowPosition(pos, Swipes.Direction.LEFT))
                        return Swipes.Direction.LEFT;
                    break;
                case 1:
                    if (IsValidSwitchRowPosition(pos, Swipes.Direction.RIGHT))
                        return Swipes.Direction.RIGHT;
                    break;
                case 2:
                    if (IsValidSwitchRowPosition(pos, Swipes.Direction.UP))
                        return Swipes.Direction.UP;
                    break;
                case 3:
                    if (IsValidSwitchRowPosition(pos, Swipes.Direction.DOWN))
                        return Swipes.Direction.DOWN;
                    break;
            }
        }
        return Swipes.Direction.NONE;
    }

    bool IsValidSwitchRowPosition(int pos, Swipes.Direction direction) {

        bool isValidPosition = false;

        if (direction == Swipes.Direction.LEFT || direction == Swipes.Direction.RIGHT)
            isValidPosition = DifferentCubesInRow(pos);
        else if (direction == Swipes.Direction.UP || direction == Swipes.Direction.DOWN)
            isValidPosition = DifferentCubesInCol(pos);

        if (!isValidPosition)
            return false;

        return TotalMoves(pos, direction) > 1;
    }

    bool DifferentCubesInRow(int pos) {

        return cubePositions.GetRow(GetRow(pos)).Distinct().Count() > 1;
    }

    bool DifferentCubesInCol(int pos) {

        return cubePositions.GetCol(GetCol(pos)).Distinct().Count() > 1;
    }

    int TotalMoves(int pos, Swipes.Direction direction) {

        var totalMoves = 0;
        int startPos = -1;

        switch (direction) {

            case Swipes.Direction.LEFT:
                startPos = cubePositions[GetRow(pos), 0];
                break;
            case Swipes.Direction.RIGHT:
                startPos = cubePositions[GetRow(pos), width - 1];
                break;
            case Swipes.Direction.UP:
                startPos = cubePositions[height - 1, GetCol(pos)];
                break;
            case Swipes.Direction.DOWN:
                startPos = cubePositions[0, GetCol(pos)];
                break;
        }

        // Cube at the end
        if (cubePositions[GetRow(startPos), GetCol(startPos)] != 0) {
            if (direction == Swipes.Direction.LEFT || direction == Swipes.Direction.RIGHT)
                totalMoves += width - 1;
            else if (direction == Swipes.Direction.UP || direction == Swipes.Direction.DOWN)
                totalMoves += height - 1;
        }

        if (totalMoves > 0) {
            if (direction == Swipes.Direction.LEFT || direction == Swipes.Direction.RIGHT)
                totalMoves += NonMainCubesInRow(pos) - 1;
            else if (direction == Swipes.Direction.UP || direction == Swipes.Direction.DOWN)
                totalMoves += NonMainCubesInCol(pos) - 1;
        }
        else {
            if (direction == Swipes.Direction.LEFT || direction == Swipes.Direction.RIGHT)
                totalMoves += NonMainCubesInRow(pos);
            else if (direction == Swipes.Direction.UP || direction == Swipes.Direction.DOWN)
                totalMoves += NonMainCubesInCol(pos);
        }

        return totalMoves;
    }

    int NonMainCubesInRow(int pos) {

        var count = 0;
        for (int i = 0; i < width; i++) {

            if (cubePositions[GetRow(pos), i] != 0) {
                count++;
            }
        }
        return count;
    }

    int NonMainCubesInCol(int pos) {

        var count = 0;
        for (int i = 0; i < height; i++) {

            if (cubePositions[i, GetCol(pos)] != 0) {
                count++;
            }
        }
        return count;
    }
    #endregion SwitchRow

    int GetRow(int pos) {
		return pos / width;
	}

	int GetCol(int pos) {
		return pos % width;
	}

	#region UpdateInfo
	public void UpdateInfo() {

		colors = new List<int> ();
		colorCount = new List<int> ();

		// Main color
		mainColor = (int) GameObject.Find ("MainColorSlider").GetComponent<Slider> ().value;
		//GameObject.Find ("MainColorInfo").GetComponent<Text>().text = Constants.ColorsString[mainColor];

		// Moves Left
		movesLeft = (int) GameObject.Find ("TurnsSlider").GetComponent<Slider>().value;
		GameObject.Find ("TurnsInfo").GetComponent<Text> ().text = movesLeft.ToString ();

		// Level width + height
		width = (int) GameObject.Find ("WidthSlider").GetComponent<Slider> ().value;
		height = (int) GameObject.Find ("HeightSlider").GetComponent<Slider> ().value;
		GameObject.Find ("WidthInfo").GetComponent<Text>().text =  width.ToString ();
		GameObject.Find ("HeightInfo").GetComponent<Text> ().text = height.ToString ();

		// Powerups
		/*powerups[0] = (int) GameObject.Find ("Powerup1Slider").GetComponent<Slider> ().value;
		powerups[1] = (int) GameObject.Find ("Powerup2Slider").GetComponent<Slider> ().value;
		powerups[2] = (int) GameObject.Find ("Powerup3Slider").GetComponent<Slider> ().value;
		GameObject.Find ("Powerup1Info").GetComponent<Text> ().text = Constants.Powerup.Text [powerups[0]];
		GameObject.Find ("Powerup2Info").GetComponent<Text> ().text = Constants.Powerup.Text [powerups[1]];
		GameObject.Find ("Powerup3Info").GetComponent<Text> ().text = Constants.Powerup.Text [powerups[2]];*/

		// Colors
		colors.Add ((int)  GameObject.Find ("Color1Slider").GetComponent<Slider> ().value);
		colors.Add ((int) GameObject.Find ("Color2Slider").GetComponent<Slider> ().value);
		colors.Add ((int) GameObject.Find ("Color3Slider").GetComponent<Slider> ().value);
		colors.Add ((int) GameObject.Find ("Color4Slider").GetComponent<Slider> ().value);
		/*GameObject.Find ("Color1Info").GetComponent<Text> ().text = Constants.ColorsString [colors[0]];
		GameObject.Find ("Color2Info").GetComponent<Text> ().text = Constants.ColorsString [colors[1]];
		GameObject.Find ("Color3Info").GetComponent<Text> ().text = Constants.ColorsString [colors[2]];
		GameObject.Find ("Color4Info").GetComponent<Text> ().text = Constants.ColorsString [colors[3]];*/

		// Color count
		colorCount.Add ((int) GameObject.Find ("Color1CountSlider").GetComponent<Slider> ().value);
		colorCount.Add ((int) GameObject.Find ("Color2CountSlider").GetComponent<Slider> ().value);
		colorCount.Add ((int) GameObject.Find ("Color3CountSlider").GetComponent<Slider> ().value);
		colorCount.Add ((int) GameObject.Find ("Color4CountSlider").GetComponent<Slider> ().value);
		GameObject.Find ("Color1CountInfo").GetComponent<Text> ().text = "x " + colorCount[0].ToString ();
		GameObject.Find ("Color2CountInfo").GetComponent<Text> ().text = "x " + colorCount[1].ToString ();
		GameObject.Find ("Color3CountInfo").GetComponent<Text> ().text = "x " + colorCount[2].ToString ();
		GameObject.Find ("Color4CountInfo").GetComponent<Text> ().text = "x " + colorCount[3].ToString ();

		// Randomize
		randomize = GameObject.Find ("Randomize").GetComponent<Toggle>().isOn;
	}
	#endregion UpdateInfo
}
