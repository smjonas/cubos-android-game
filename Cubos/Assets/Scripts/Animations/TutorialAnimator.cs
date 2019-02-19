using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimator : MonoBehaviour {

	public GameObject hand;
	private Vector3 startPos, beginPos, relEndPos, endPos;
	private int angle;
	private Coroutine coroutine;

	void Start() {
		hand.GetComponent<SpriteRenderer> ().material.renderQueue = 2001;
	}

	public void SetPositions(Vector3 startPos, Swipes.Direction direction, float length, int angle) {

		this.startPos = startPos;
		this.angle = angle;

		switch (direction) {
			
		case Swipes.Direction.LEFT:
			relEndPos = new Vector3 (-length * Constants.CubeSizeX, 0);
			break;
		case Swipes.Direction.RIGHT:
			relEndPos = new Vector3 (length * Constants.CubeSizeX, 0);
			break;
		case Swipes.Direction.UP:
			relEndPos = new Vector3 (0, length * Constants.CubeSizeY);
			break;
		case Swipes.Direction.DOWN:
			relEndPos = new Vector3 (0, -length * Constants.CubeSizeY);
			break;
		case Swipes.Direction.DOWN_LEFT:
			relEndPos = new Vector3 (-length * Constants.CubeSizeX, -length * Constants.CubeSizeY);
			break;
		case Swipes.Direction.DOWN_RIGHT:
			relEndPos = new Vector3 (length * Constants.CubeSizeX, -length * Constants.CubeSizeY);
			break;
		case Swipes.Direction.UP_LEFT:
			relEndPos = new Vector3 (-length * Constants.CubeSizeX, length * Constants.CubeSizeY);
			break;
		case Swipes.Direction.UP_RIGHT:
			relEndPos = new Vector3 (length * Constants.CubeSizeX, length * Constants.CubeSizeY);
			break;			
		}
	}

	public void SetPositions(Vector3 startPos, int angle) {
	
		this.startPos = startPos;
		this.angle = angle;
	}

	public void SetCircularPositions(Vector3 beginPos) {
		this.beginPos = beginPos;

	}

	public void Animate(bool repeat) {
		Animate (Constants.TutorialHandMoveTime, repeat);
	}

	public void Animate(float time, bool repeat) {

		hand.SetActive (true);
		hand.transform.position = startPos;
		hand.transform.eulerAngles = new Vector3 (0, 0, angle);
		hand.GetComponent<SpriteRenderer> ().color = Color.white;

		var pos = new Vector3 (startPos.x + relEndPos.x, startPos.y + relEndPos.y);
		coroutine = FadeOut ();
		if (repeat)
			iTween.MoveTo (hand, iTween.Hash ("name", "handAnimation", "position", pos, "time", time, "easetype", "linear", "oncomplete", "Animate", "oncompleteparams", true, "oncompletetarget", gameObject));	
		else
			iTween.MoveTo (hand, iTween.Hash ("name", "handAnimation", "position", pos, "time", time, "easetype", "linear"));
	}

	public IEnumerator AnimateRotate() {

		SetPositions (beginPos, Swipes.Direction.RIGHT, 2.6f, -55);
		Animate (0.6f, false);
		yield return new WaitForSeconds (0.6f);

		SetPositions (hand.transform.position, Swipes.Direction.DOWN, 2.8f, -145);
		Animate (0.6f, false);
		yield return new WaitForSeconds (0.6f);

		SetPositions (hand.transform.position, Swipes.Direction.LEFT, 2.7f, -235);
		Animate (0.6f, false);
		yield return new WaitForSeconds (0.6f);

		SetPositions (hand.transform.position, Swipes.Direction.UP, 2.8f, 35);
		Animate (0.6f, false);
		yield return new WaitForSeconds (0.6f);
		StopAnimation ();

		yield return new WaitForSeconds (1f);
		StartCoroutine (AnimateRotate ());
	}

	public void AnimateStill() {
		
		hand.SetActive (true);
		hand.transform.position = startPos;
		hand.transform.eulerAngles = new Vector3 (0, 0, angle);
		hand.GetComponent<SpriteRenderer> ().color = Color.white;
		iTween.MoveTo (hand, iTween.Hash ("name", "handAnimation", "position", startPos, "time", Constants.TutorialHandMoveTime, "easetype", "linear", "oncomplete", "AnimateStill", "oncompletetarget", gameObject));
	}
		
	public void StopAnimation() {

		if (coroutine != null)
			StopCoroutine (coroutine);
		hand.GetComponent<SpriteRenderer> ().color = Color.white;
		if (GetComponent<iTween> () != null)
			iTween.StopByName ("handAnimation");
		hand.SetActive (false);
	}

	public void DestroyHand() {

		StopAllCoroutines ();
		FadeOut ();
		Destroy (hand, 1f);
	}

	protected Coroutine FadeOut() {
		return hand.FadeTo<SpriteRenderer>(50f, Constants.TutorialHandMoveTime);
	}
}
