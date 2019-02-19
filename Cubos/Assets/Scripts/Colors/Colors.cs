using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Colors : MonoBehaviour {
	
	public static Color lightGrey = RGBToColor (227, 227, 227);
	public static Color powerupMissing = RGBToColor (176, 55, 83);
	public static Color powerupUsed = RGBToColor (105, 84, 128);

	public static Color lightRed = RGBToColor(243, 69, 69);


    private static Color RGBToColor(int r, int g, int b) {
        return new Color(r / 255f, g / 255f, b / 255f);
    }

	[SerializeField]
	public ColorScheme menuColors = new ColorScheme ();
	[SerializeField]
	public List<ColorScheme> lvlSchemes = new List<ColorScheme>();
	[SerializeField]
	public List<ColorScheme> tutSchemes = new List<ColorScheme>();

}