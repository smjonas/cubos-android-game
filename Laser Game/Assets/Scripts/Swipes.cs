using UnityEngine;

public class Swipes : MonoBehaviour {

	public static Vector2 currentSwipe;
	public static Vector2 startPos, endPos;

	static float maxTime = 2f;
	static float minSwipeDist = 110f;

	public float startTime, endTime;
	public float swipeDistance, swipeTime;

	public static bool startSwipe, done;

	void Update () {
	
		if (startSwipe) {

			if (Input.touchCount > 0) {

				var touch = Input.GetTouch(0);

				if (!done) {
					
					startTime = Time.time;
					startPos = touch.position;
					done = true;
				}

				else if (touch.phase == TouchPhase.Ended) {

					endTime = Time.time;
					endPos = touch.position;

					currentSwipe = endPos - startPos;
					swipeDistance = currentSwipe.magnitude;
					swipeTime = endTime - startTime;

					if (swipeDistance > minSwipeDist && swipeTime < maxTime) {
						currentSwipe.Normalize ();
						startSwipe = false;
					} else {
						currentSwipe = Vector2.zero;
						startSwipe = false;
					}
				}
			}
		} 
	}

	public enum Direction {LEFT, RIGHT, UP, DOWN, UP_LEFT, UP_RIGHT, DOWN_LEFT, DOWN_RIGHT, NONE};

	class CardinalDirections {

		public static readonly Vector2 Up = new Vector2 (0, 1);
		public static readonly Vector2 Down = new Vector2 (0, -1);
		public static readonly Vector2 Right = new Vector2 (1, 0);
		public static readonly Vector2 Left = new Vector2 (-1, 0);

		public static readonly Vector2 UpRight = new Vector2 (1, 1);
		public static readonly Vector2 UpLeft = new Vector2 (-1, 1);
		public static readonly Vector2 DownRight = new Vector2 (1, -1);
		public static readonly Vector2 DownLeft = new Vector2 (-1,-1);
	}

	public static Direction GetSwipeDirection(Vector2 swipe) {

		Direction direction = Direction.NONE;
		if (Vector2.Dot (swipe, CardinalDirections.Up) > 0.906f)
			direction = Direction.UP;
		else if (Vector2.Dot (swipe, CardinalDirections.Down) > 0.906f)
			direction = Direction.DOWN;
		else if (Vector2.Dot (swipe, CardinalDirections.Left) > 0.906f)
			direction = Direction.LEFT;
		else if (Vector2.Dot (swipe, CardinalDirections.Right) > 0.906f)
			direction = Direction.RIGHT;
		else if (Vector2.Dot (swipe, CardinalDirections.UpRight) > 0.906f)
			direction = Direction.UP_RIGHT;
		else if (Vector2.Dot (swipe, CardinalDirections.UpLeft) > 0.906f)
			direction = Direction.UP_LEFT;
		else if (Vector2.Dot (swipe, CardinalDirections.DownLeft) > 0.906f)
			direction = Direction.DOWN_LEFT;
		else if (Vector2.Dot (swipe, CardinalDirections.DownRight) > 0.906f)
			direction = Direction.DOWN_RIGHT;

		return direction;
	}

	public static Direction GetSwipeAngleDirection(Vector2 startPos, Vector2 endPos) {

		var angle = Mathf.Atan2 (endPos.x - startPos.x, endPos.y - startPos.y) * 57.2957795131f; // * 180 / PI
		print (angle);

		var direction = Swipes.Direction.NONE;

		if (angle >= -22.5f && angle < 22.5f)
			direction = Direction.UP;
		else if (angle >= 22.5f && angle < 67.5f)
			direction = Direction.UP_RIGHT;
		else if (angle >= 67.5f && angle < 112.5f)
			direction = Direction.RIGHT;
		else if (angle >= 112.5f && angle < 157.5f)
			direction = Direction.DOWN_RIGHT;
		else if (angle >= 157.5f && angle <= 180f || angle >= -180f && angle < -157.5f)
			direction = Direction.DOWN;
		else if (angle >= -157.5f && angle < -112.5f)
			direction = Direction.DOWN_LEFT;
		else if (angle >= -112.5f && angle < -67.5f)
			direction = Direction.LEFT;
		else if (angle >= -67.5f && angle < -22.5f)
			direction = Direction.UP_LEFT;

		print (direction);
		return direction;
	}
}