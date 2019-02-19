using UnityEngine;

public class PowerupFactory {

	public static PowerupExe GetPowerup(PowerupExe powerup) {

		var type = powerup.GetType ().ToString ();

		/*switch (type) {
			
		case "RotateExe":
			return new RotateExe (powerup.startPos, powerup.direction);
		case "SwitchExe":
			return new SwitchExe (powerup.startPos, powerup.direction);
		case "SwitchRowExe":
			return new SwitchRowExe (powerup.startPos, powerup.direction);
		default:
			Debug.LogError ("PowerupFactory: Powerup type " + type + " not defined!");
			return null;
		}*/
		return null;
	}
}