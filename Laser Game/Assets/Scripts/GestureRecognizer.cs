using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.UI;
using PDollarGestureRecognizer;

public class GestureRecognizer : MonoBehaviour {

	private List<Gesture> trainingSet = new List<Gesture>();
	public List<Point> points = new List<Point>();
	private int strokeId = -1;

	private Vector3 virtualKeyPosition = Vector2.zero;

	private bool recognized;
	public Result gestureResult;

	void Awake() {

		//Load pre-made gestures
		/*TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
        foreach (TextAsset gestureXml in gesturesXml)
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));*/

		//Load user custom gestures
		string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
		foreach (string filePath in filePaths)
			trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
	}

	public void Recognize() {

		if (Input.touchCount > 0) {
			var touch = Input.GetTouch (0);

			if (touch.phase == TouchPhase.Began) {

				if (recognized) {

					recognized = false;
					strokeId = -1;
					points.Clear ();
				}
				++strokeId;
			}

			if (touch.phase == TouchPhase.Moved) {

				virtualKeyPosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y);
				points.Add (new Point (virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));
			}

			if (touch.phase == TouchPhase.Ended) {

				recognized = true;
				Gesture candidate = new Gesture (points.ToArray ());
				gestureResult = PointCloudRecognizer.Classify (candidate, trainingSet.ToArray ());
			}
		}
	}

	/*public void Add() {

		if (points.Count > 0 && name != string.Empty) {

			string fileName = String.Format("{0}/{1}-{2}.xml", Application.persistentDataPath, name, DateTime.Now.ToFileTime());

			#if !UNITY_WEBPLAYER
			GestureIO.WriteGesture(points.ToArray(), name, fileName);
			#endif

			trainingSet.Add(new Gesture(points.ToArray(), name));
			name = string.Empty;
		}
	}*/
}
