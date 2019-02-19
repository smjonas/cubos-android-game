using UnityEngine;

public class RenderQueue : MonoBehaviour {

	public int renderQueue;

	void Start() {
		GetComponent<SpriteRenderer> ().material.renderQueue = renderQueue;
	}
}
