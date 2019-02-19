using UnityEngine;
using System;
using System.IO;

public class TextToFile : MonoBehaviour {

	private const string fileName = "LevelInfo.txt";
	static int currentLevel;

	public static void WriteLine(object data, bool newLevel, bool separate) {

		if (newLevel) {
			print ("READING");
			var text = File.ReadAllText (fileName);

			var lastLevel = text.LastIndexOf ("Level ");
			lastLevel = lastLevel != -1 ? (lastLevel + "Level ".Length) : -1;

			currentLevel = lastLevel == -1 ? 0 : int.Parse (text.Substring (lastLevel, 1));
			print ("LEVEL " + currentLevel);
		}
					
		using (StreamWriter writer = new StreamWriter (fileName, true)) {
			
			if (newLevel) {
				writer.WriteLine ("\n");
				writer.WriteLine ("\n");
				writer.WriteLine (DateTime.Today.ToLongDateString ());
				writer.WriteLine ("Level " + (currentLevel + 1));
				writer.WriteLine ("\n");
			}

			if (separate)
				writer.WriteLine ("\n");
			
			writer.WriteLine (data);
		}
	}

	public static void WriteLine(object data) {

		WriteLine (data, false, false);
	}

	public static void Write(object data, bool separate) {

		using (StreamWriter writer = new StreamWriter (fileName, true)) {

			if (separate)
				writer.WriteLine ("\n");

			writer.Write (data);
		}
	}
}