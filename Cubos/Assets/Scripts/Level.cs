using System.Linq;
using UnityEngine;

public class Level {

    public int width, height;
	public int priorTutorial;
    public int[] moveInfo;
    public int movesLeft;
    public int[,] markerPositions, cubePositions, pulsePositions;
	public int gaps;
	public Color bgColor;
    public Color[] colors;
	public Swipes.Direction[] moveDirections;

	// Constructor for regular level
	public Level(Colors colors, int number, int[,] markerPositions, int[,] cubePositions, params int[] moveInfo) {

		this.bgColor = colors.lvlSchemes [number % 6].bgColor;
		this.colors = colors.lvlSchemes [number % 6].colors.ToArray ();

        this.markerPositions = markerPositions;
        this.cubePositions = cubePositions;

        this.width = markerPositions.GetLength(1);
        this.height = markerPositions.GetLength(0);
        this.moveInfo = moveInfo;
        this.movesLeft = moveInfo.Sum();
    }

	// Constructor for tutorial level
	public Level(Colors colors, int number, int[,] markerPositions, int[,] cubePositions, int[,] pulsePositions, Swipes.Direction[] moveDirections, params int[] moveInfo) {

		if (colors.tutSchemes.Count > number) {
			this.bgColor = colors.tutSchemes [number].bgColor;
			this.colors = colors.tutSchemes [number].colors.ToArray ();
		}
		else Debug.LogError ("No color data for tutorial #" + (number + 1) + " available!");

		this.markerPositions = markerPositions;
		this.cubePositions = cubePositions;

		this.pulsePositions = pulsePositions;
		this.moveDirections = moveDirections;

		this.width = markerPositions.GetLength(1);
		this.height = markerPositions.GetLength(0);
		this.moveInfo = moveInfo;
		this.movesLeft = moveInfo.Sum();

	}
}
