
public class Constants {

	public static class Powerup {

		public static readonly int Switch = 0;
		public static readonly int Rotate = 1;
		public static readonly int SwitchRow = 2;
	}

    public static readonly float CubeSizeX = 0.755f;
    public static readonly float CubeSizeY = 0.755f;
    public static readonly float CubeGapX = 0.15f;
    public static readonly float CubeGapY = 0.20f;
    public static readonly float CubeSpawnDistX = CubeSizeX + CubeGapX;
    public static readonly float CubeSpawnDistY = CubeSizeY + CubeGapY;

    public static readonly float MarkerOffset = 0.375f;

	public static readonly float PowerupIconCenterXPos = 65f;
	public static readonly float PowerupIconGap = 130f;

	public static readonly float MusicFadeInTime = 3.6f;

	public static readonly float CubeScaleInOutTime = 0.7f;
	public static readonly float CubeScaleBackTime = 0.8f;
    public static readonly float CubeSwitchTime = 0.34f;

    public static readonly float ScreenScrollTime = 0.55f;
	public static readonly float ScreenTintFadeTime = 0.34f;

	public static readonly float OnLevelEndScrollDownTime = 0.25f;
	public static readonly float TutorialHandMoveTime = 2f;

	public static readonly int Left = 0;
	public static readonly int Right = 1;
	public static readonly int Up = 2;
	public static readonly int Down = 3;
	public static readonly int UpRight = 4;
	public static readonly int UpLeft = 5;
	public static readonly int DownRight = 6;
	public static readonly int DownLeft = 7;

	public static readonly int MenuScene = 0;
	public static readonly int GameScene = 1;
	public static readonly int LevelSelectionScene = 2;
	public static readonly int LevelEditorMenuScene = 3;
	public static readonly int LevelEditorScene = 4;
	public static readonly int[] TutorialScenes = new int[] { 5, 6, 7, 8 };

}
