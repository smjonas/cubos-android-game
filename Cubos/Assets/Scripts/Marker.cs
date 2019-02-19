using UnityEngine;

public class Marker : MonoBehaviour {

	public int pos { get; set; }
	public Sprite[] markerSprites;

	void Start() {

		gameObject.GetComponent<SpriteRenderer> ().sprite = markerSprites [0];
        gameObject.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(0.7f, 0.7f, 1f);
    }

	public void SetColor(Color color) {
		gameObject.GetComponent<SpriteRenderer> ().color = color;
	}
}
