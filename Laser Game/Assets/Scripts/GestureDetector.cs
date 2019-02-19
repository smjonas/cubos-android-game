using PDollarGestureRecognizer;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class GestureDetector : MonoBehaviour {

    private static List<Cube> cubeList = new List<Cube>();
    private static List<int> positions = new List<int>();
    public static bool ready = true;
    private static bool reset = true;

	private float startTime, maxTime = 2f;
	private Vector2 startPos;
	private float minDist = 1f;

	public Powerups powerups;

	private List<Gesture> trainingSet = new List<Gesture>();
	public List<Point> points = new List<Point>();
	private int strokeId = -1;

	private Vector3 virtualKeyPosition = Vector2.zero;

	private bool recognized;
	public Result gestureResult;

	void Start() {

		// Load custom gestures
		TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("Gestures");
		foreach (TextAsset gestureXml in gesturesXml)
			trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
		
		if (LevelManager.instance.gameState == LevelManager.GameState.InLevelEditor)
			powerups = (LevelEditorPowerups)powerups;		
	}

    public static void AddCube(Cube cube) {

		if (cube.isMovable || cubeList.Count > 0) {

			if (LevelManager.instance.gameState != LevelManager.GameState.InLevelEditor || (LevelManager.instance.gameState == LevelManager.GameState.InLevelEditor && LevelEditorSpawner.editToolIsOn)) {

				if (!cubeList.Contains (cube) && !positions.Contains (cube.pos)) {
					cubeList.Add (cube);
					positions.Add (cube.pos);

				} else if (cubeList.Count == 4 && cubeList [0].pos == cube.pos) {
					cubeList.Add (cube);
					positions.Add (cube.pos);
				}
			}

			if (LevelManager.instance.gameState == LevelManager.GameState.InLevelEditor) {

				var cubes = GameObject.FindGameObjectsWithTag ("Cube");
				var markers = GameObject.FindGameObjectsWithTag ("Marker");

				if (LevelEditorSpawner.fillToolIsOn) {

					foreach (var c in cubes)
						c.GetComponent<Cube> ().SetColor (LevelEditorSpawner.color);
					foreach (var m in markers)
						m.GetComponent<Marker> ().SetColor (LevelEditorSpawner.color);
				
				} else if (!LevelEditorSpawner.editToolIsOn) {

					cube.SetColor (new Color(UnityEngine.Random.Range (0f, 1f), UnityEngine.Random.Range (0f, 1f), UnityEngine.Random.Range (0f, 1f)));
				
					foreach (var m in markers) {

						if (m.GetComponent<Marker> ().pos == cube.pos)
							m.GetComponent<Marker> ().SetColor (cube.color);
					}
				}
			}
		}
    }

    void Update() {
		
		if (ready) {
	        if (Input.touchCount > 0) {

	            reset = false;
	            var touch = Input.GetTouch(0);

				if (touch.phase == TouchPhase.Began) {
					
					startTime = Time.time;
					startPos = touch.position;

					if (recognized) {

						recognized = false;
						strokeId = -1;
						points.Clear ();
					}
					++strokeId;
				}

				if (touch.phase == TouchPhase.Moved)
					points.Add (new Point (touch.position.x, -touch.position.y, strokeId));

				if (touch.phase == TouchPhase.Ended) {
					if (Time.time - startTime < maxTime && (touch.position - startPos).magnitude > minDist) {
						
						recognized = true;
						if (points.Count > 2) {
							
							Gesture candidate = new Gesture (points.ToArray ());
							gestureResult = PointCloudRecognizer.Classify (candidate, trainingSet.ToArray ());
							InterpretGesture (gestureResult.GestureClass, points);
						}
					}
				} 			 	
	        }
        }
		if (Input.touchCount == 0 && !reset) {
			
			reset = true;
			Reset();
		}
    }

    public void Reset() {

        cubeList.Clear();
        positions.Clear();
    }

	private void InterpretGesture(string gesture, List<Point> points) {

		var first = points [0];
		var last = points [points.Count - 1];
		var direction = Swipes.Direction.NONE;

		switch (gesture) {
			
		case "LineStraight":
			if (first.X > last.X) {
				//print ("Left");
				direction = Swipes.Direction.LEFT;
			} else {
				//print ("Right");
				direction = Swipes.Direction.RIGHT;
			}
			break;

		case "LineUp":
			if (first.Y > last.Y) {
//				print ("Up");
				direction = Swipes.Direction.UP;
			} else {
//				print ("Down");
				direction = Swipes.Direction.DOWN;
			}
			break;

		case "LineRightUp":
			if (first.Y > last.Y) {
//				print ("RightUp");
				direction = Swipes.Direction.UP_RIGHT;
			} else {
//				print ("LeftDown");
				direction = Swipes.Direction.DOWN_LEFT;
			}
			break;

		case "LineRightDown":
			if (first.Y > last.Y) {
//				print ("LeftUp");
				direction = Swipes.Direction.UP_LEFT;
			} else {
//				print ("RightDown");
				direction = Swipes.Direction.DOWN_RIGHT;
			}
			break;

		case "Circle":
//			print ("Circle!");
			direction = Swipes.Direction.LEFT;
			break;

		default:
			Debug.LogError ("GestureInterpreter: Gesture " + gesture + " not defined!");
			break;
		}

		if (direction != Swipes.Direction.NONE) {

			if ((int)direction <= 3) {
				if (cubeList.Count == 1 && cubeList [0].neighbors [(int)direction] == null) {
					StartCoroutine (powerups.ExecutePowerup (Constants.Powerup.SwitchRow, cubeList, positions, direction));
				}
			}
			if (cubeList.Count == 1 || cubeList.Count == 2) {
				StartCoroutine (powerups.ExecutePowerup (Constants.Powerup.Switch, cubeList, positions, direction));
			} else if (cubeList.Count == 5)
				StartCoroutine (powerups.ExecutePowerup (Constants.Powerup.Rotate, cubeList, positions, Swipes.Direction.NONE));
		}
		cubeList.Clear ();
	}
}