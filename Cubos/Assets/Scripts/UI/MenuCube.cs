using System.Collections.Generic;
using UnityEngine;

public class MenuCube : Cube {

    int maxPos, count;

    void Start() {
        maxPos = LevelManager.instance.boardWidth * LevelManager.instance.boardHeight - 1;
    }

    public void RandomPowerup(List<MenuCube> cubes) {

        var powerup = Random.Range(0, 2);
        switch (powerup) {

            case 0: // Switch
                Powerups.switch_.TryExecute(this, GetSwitchDirection());
                break;
            case 1: // Rotate
				Powerups.rotate.TryExecute(this, GetRotateDirection());
                break;
            default:
                Debug.LogError("RandomPowerup: Powerup " + powerup + " not defined!");
                break;
        }

        SetNeighbors();
    }

    Swipes.Direction GetSwitchDirection() {

        var direction = Random.Range(0, 8);
        var width = LevelManager.instance.boardWidth;
       
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
            return GetSwitchDirection();

        else return (Swipes.Direction) direction;
    }

    Swipes.Direction GetRotateDirection() {

        var width = LevelManager.instance.boardWidth;
        var height = LevelManager.instance.boardHeight;
        var direction = Random.Range(0, 2);

        var isValidPosition = false;

        switch (direction) {

            case 0: // Left
                if (GetRow(pos) < height - 1 && GetCol(pos) > 0)
                    isValidPosition = true;
                    break;
            case 1: // Right
                if (GetRow(pos) < height - 1 && GetCol(pos) < width - 1)
                    isValidPosition = true;
                    break;
        }

        if (!isValidPosition) {
            count++;
            if (count > 10)
                return Swipes.Direction.NONE;
            return GetRotateDirection();
        }
        else return (Swipes.Direction) direction;
    }
}