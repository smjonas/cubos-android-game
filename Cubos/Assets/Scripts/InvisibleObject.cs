using UnityEngine;

public class InvisibleObject : MonoBehaviour {

    public bool isTouched;
    bool isOver;

    void Update() {

        if (Input.touchCount > 0 && isOver)
            isTouched = true;
        
        else isTouched = false;
    }

   void OnMouseOver() {
        isOver = true;
    }
}
