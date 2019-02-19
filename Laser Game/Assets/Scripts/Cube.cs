using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;

public class Cube : MonoBehaviour {

	#region variables
	public Color color { get; private set; }
	public int pos;
	public bool isPulsing { get; private set; }
	public bool isMovable { get; private set; }

    public List<Cube> neighbors;
	private int width, height;

    void Awake() {

        iTween.Init(gameObject);
        neighbors = new List<Cube>();

		isMovable = true;
		//print(GetComponent<SpriteRenderer> ().material.renderQueue);
		//GetComponent<SpriteRenderer> ().material.renderQueue = 2001;

		if (SceneManager.GetActiveScene().buildIndex != Constants.MenuScene)
        	transform.localScale = new Vector3(0.75f, 0.75f, 1f);

		width = LevelManager.instance.boardWidth;
		height = LevelManager.instance.boardHeight;
    }

    #endregion variables

    private void OnMouseOver() {

        if (Input.touchCount > 0)
            GestureDetector.AddCube(this);
    }

    public void SetNeighbors(List<Cube> cubes) {

        neighbors.Clear();
		// Right
        neighbors.Add(cubes.Where(x => x.pos == pos - 1 && GetRow(x.pos) == GetRow(pos)).FirstOrDefault());
        // Left
		neighbors.Add(cubes.Where(x => x.pos == pos + 1 && GetRow(x.pos) == GetRow(pos)).FirstOrDefault());
		// Up
		neighbors.Add(cubes.Where(x => x.pos == pos + width).FirstOrDefault());
		// Down
		neighbors.Add(cubes.Where(x => x.pos == pos - width).FirstOrDefault());
		// Up left
		neighbors.Add (cubes.Where (x => x.pos == pos + width - 1 && GetRow (x.pos) == GetRow (pos) + 1).FirstOrDefault ());
		// Up right
		neighbors.Add (cubes.Where (x => x.pos == pos + width + 1 && GetRow (x.pos) == GetRow (pos) + 1).FirstOrDefault ());
		// Down left
		neighbors.Add (cubes.Where (x => x.pos == pos - width - 1 && GetRow (x.pos) == GetRow (pos) - 1).FirstOrDefault ());
		// Down right
		neighbors.Add (cubes.Where (x => x.pos == pos - width + 1 && GetRow (x.pos) == GetRow (pos) - 1).FirstOrDefault ());
	}


    public void SetNeighbors() {

        var cubeObjects = new List<GameObject>();
        var cubes = new List<Cube>();

        cubeObjects.AddRange(GameObject.FindGameObjectsWithTag("Cube"));
        foreach (GameObject go in cubeObjects)
            cubes.Add(go.GetComponent<Cube>());
        
        SetNeighbors(cubes);
    }


	public void SetColor(Color color) {

		this.color = color;
		gameObject.GetComponent<SpriteRenderer> ().color = color;		 
	}

	public void SetMovable(bool setMovable) {
		isMovable = setMovable;
	}

	public void SetPulse(bool setPulse) {
		
		isPulsing = setPulse;
		if (isPulsing)
			GetComponent<CubeAnimations> ().Play ();
		else
			GetComponent<CubeAnimations> ().Stop ();
	}

    protected int GetRow(int pos) {
		return pos / width;
    }

    protected int GetCol(int pos) {
		return pos % width;
    }
}