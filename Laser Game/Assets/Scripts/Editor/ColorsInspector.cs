using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Colors))]

public class ColorsEditor : Editor {

	Colors c;
	private string index, level, color;

	void OnEnable() {
		c = (Colors) target;
	}

	public override void OnInspectorGUI() {

		serializedObject.Update ();

		GUILayout.Space (15);
		GUI.color = Color.white;
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Menu colors:", GUILayout.Width (75));

		GUILayout.Space (10);
		for (int j = 0; j < c.menuColors.colors.Count; j++) {
			c.menuColors.colors [j] = EditorGUILayout.ColorField (c.menuColors.colors [j], GUILayout.Width (40));
		}
	
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("+", GUILayout.Width (30))) {
			AddMenuColor ();
		}

		if (GUILayout.Button ("-", GUILayout.Width (30))) {
			RemoveMenuColor ();
		}

		GUILayout.EndHorizontal ();
		GUILayout.Space (15);

		#region tutorials
		GUI.color = Color.white;

		GUILayout.BeginHorizontal ();
		GUILayout.Space (10);

		if (GUILayout.Button ("Add tutorial color scheme (" + c.tutSchemes.Count + ")", GUILayout.Height (25))) {
			AddTutColorScheme ();
		}
		GUILayout.Space (10);
		GUILayout.EndHorizontal ();
		GUILayout.Space (15);


		for (int i = 0; i < c.tutSchemes.Count; i++) {

			GUI.color = Color.white;
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Tutorial " + (i + 1), GUILayout.Width (70));

			c.tutSchemes [i].bgColor = EditorGUILayout.ColorField (c.tutSchemes [i].bgColor, GUILayout.Width (40));
			GUILayout.Space (10);

			for (int j = 0; j < c.tutSchemes [i].colors.Count; j++) {
				c.tutSchemes [i].colors [j] = EditorGUILayout.ColorField (c.tutSchemes [i].colors [j], GUILayout.Width (40));
			}

			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("+", GUILayout.Width (30))) {
				AddTutColor (i);
			}

			if (GUILayout.Button ("-", GUILayout.Width (30))) {
				RemoveLastTutColor (i);
			}

			GUI.color = Colors.lightRed;
			if (GUILayout.Button ("X", GUILayout.Width (30))) {
				RemoveTutColorScheme (i);
				return;
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
		}

		GUI.color = Color.white;
		GUILayout.Space (15);
		GUILayout.BeginHorizontal ();
		GUILayout.Space (10);

		GUILayout.Label("Remove color at index:");

		level = EditorGUILayout.TextField (level, GUILayout.Height (18));
		color = EditorGUILayout.TextField (color, GUILayout.Height (18));

		GUILayout.Space (5);
		if (GUILayout.Button ("-", GUILayout.Width (40))) {

			int lvl, col;
			if (Int32.TryParse (level, out lvl)) {
				if (Int32.TryParse (color, out col)) {
					RemoveTutColor (Int32.Parse (level) - 1, Int32.Parse (color) - 1);
				}
				else Debug.LogError ("Wrong index entered!");
			}
			else Debug.LogError ("Wrong level entered!");

			level = string.Empty;
			color = string.Empty;
		}

		GUILayout.Space (10);
		GUILayout.EndHorizontal ();
		#endregion

		#region levels
		GUILayout.Space (15);
		GUILayout.BeginHorizontal ();
		GUILayout.Space (10);

		if (GUILayout.Button ("Add level color scheme (" + c.lvlSchemes.Count + ")", GUILayout.Height (25))) {
			AddLvlColorScheme ();
		}

		GUILayout.Space (10);
		GUILayout.EndHorizontal ();
		GUILayout.Space (15);


		for (int i = 0; i < c.lvlSchemes.Count; i++) {

			GUI.color = Color.white;
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Scheme " + (i + 1), GUILayout.Width (75));

			c.lvlSchemes [i].bgColor = EditorGUILayout.ColorField (c.lvlSchemes [i].bgColor, GUILayout.Width (40));
			GUILayout.Space (10);

			for (int j = 0; j < c.lvlSchemes [i].colors.Count; j++) {
				c.lvlSchemes [i].colors [j] = EditorGUILayout.ColorField (c.lvlSchemes [i].colors [j], GUILayout.Width (40));
			}

			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("+", GUILayout.Width (30))) {
				AddLvlColor (i);
			}

			if (GUILayout.Button ("-", GUILayout.Width (30))) {
				RemoveLastLvlColor (i);
			}

			GUI.color = Colors.lightRed;
			if (GUILayout.Button ("X", GUILayout.Width (30))) {
				RemoveLvlColorScheme (i);
				return;
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
		}

		GUI.color = Color.white;
		GUILayout.Space (15);
		GUILayout.BeginHorizontal ();
		GUILayout.Space (10);

		GUILayout.Label("Remove color at index:");

		level = EditorGUILayout.TextField (level, GUILayout.Height (18));
		color = EditorGUILayout.TextField (color, GUILayout.Height (18));

		GUILayout.Space (5);
		if (GUILayout.Button ("-", GUILayout.Width (40))) {

			int lvl, col;
			if (Int32.TryParse (level, out lvl)) {
				if (Int32.TryParse (color, out col)) {
					RemoveLvlColor (Int32.Parse (level), Int32.Parse (color) - 1);
				}
				else Debug.LogError ("Wrong index entered!");
			}
			else Debug.LogError ("Wrong level entered!");

			level = string.Empty;
			color = string.Empty;
		}

		GUILayout.Space (10);
		GUILayout.EndHorizontal ();

		GUILayout.Space (5);
		GUILayout.BeginHorizontal ();
		GUILayout.Space (10);

		GUILayout.Label("Insert color scheme at level:");
		GUILayout.Space (5);

		index = EditorGUILayout.TextField (index, GUILayout.Height (18));
		GUILayout.Space (5);

		if (GUILayout.Button ("+", GUILayout.Width (40))) {

			int lvl;
			if (Int32.TryParse (index, out lvl)) {
				InsertLvlColorScheme (Int32.Parse (index));
			}
			else Debug.Log ("DDD");
			index = string.Empty;
		}

		GUILayout.EndHorizontal ();
		GUILayout.Space (15);

		#endregion

		Undo.RecordObject (c, "Name");
		serializedObject.ApplyModifiedProperties ();

	}

	private void AddLvlColorScheme() {
		c.lvlSchemes.Add (new ColorScheme());
	}

	private void RemoveLvlColorScheme(int index) {
		c.lvlSchemes.RemoveAt (index);
	}

	private void AddLvlColor(int index) {
		c.lvlSchemes[index].colors.Add (Color.white);
	}

	private void RemoveLvlColor(int level, int index) {
		
		if (level >= 0 && c.lvlSchemes [level].colors.Count > 1)
			c.lvlSchemes [level].colors.RemoveAt (index);
	}

	private void RemoveLastLvlColor(int index) {

		if (c.lvlSchemes[index].colors.Count - 1 >= 0)
			c.lvlSchemes [index].colors.RemoveAt (c.lvlSchemes[index].colors.Count - 1);
	}

	private void InsertLvlColorScheme(int index) {

		if (index < c.lvlSchemes.Count) {
			c.lvlSchemes.Insert (index, new ColorScheme ());
			this.index = string.Empty;
		}
	}




	private void AddTutColorScheme() {
		c.tutSchemes.Add (new ColorScheme());
	}

	private void RemoveTutColorScheme(int index) {
		c.tutSchemes.RemoveAt (index);
	}

	private void AddTutColor(int index) {
		c.tutSchemes[index].colors.Add (Color.white);
	}

	private void RemoveTutColor(int level, int index) {

		if (c.tutSchemes [level].colors.Count > 1)
			c.tutSchemes [level].colors.RemoveAt (index);
	}

	private void RemoveLastTutColor(int index) {

		if (c.tutSchemes[index].colors.Count - 1 >= 0)
			c.tutSchemes [index].colors.RemoveAt (c.tutSchemes[index].colors.Count - 1);
	}




	private void AddMenuColor() {
		c.menuColors.colors.Add (Color.white);
	}

	private void RemoveMenuColor() {

		if (c.menuColors.colors.Count > 1)
			c.menuColors.colors.RemoveAt (c.menuColors.colors.Count - 1);
	}
}
