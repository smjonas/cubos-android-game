using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour {

	/*public int money = 200;
	int currentItem;
	string[] text;

	GameObject clickedButton;

	public int [] itemCount;

	public static Shop instance;

	void SetButtonText() {

		text = new string[] {"Destroy 2", "Switch", "Rotate 2x2", "Switch 3", "Destroy Row", "Destroy 1", "Dye", "Dye Area", "Blocker", "Switch Row"};
	}

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void Awake() {

		if (instance == null) {

			DontDestroyOnLoad (transform.gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		SetButtonText ();
	}

	void Start() {

		//itemCount = new int[Constants.PowerupCount];
		for (int i = 0; i < itemCount.GetLength(0); i++) {
			itemCount [i] = 10;
		}
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		
		// Update shop
		if (SceneManager.GetActiveScene ().buildIndex == Constants.ShopScene) {

			GameObject.FindGameObjectWithTag ("Money").GetComponentInChildren<Text> ().text = "Money: " + money;
			GameObject[] buttons = GameObject.FindGameObjectsWithTag ("ShopButton");

			foreach (GameObject b in buttons) {
				for (int i = 0; i < text.GetLength(0); i++) {
					// set current powerup according to selected button's text
					if (b.GetComponentInChildren<Text>().text.Equals("+ " + text[i])) {
						
						b.transform.Find ("ItemCount").gameObject.GetComponentInChildren<Text> ().text = "x " +  itemCount [i];
					}
				}
			}
		}
	}

	public void BuyItem() {

		clickedButton = EventSystem.current.currentSelectedGameObject;
		currentItem = clickedButton.GetComponentInChildren<ShopItem> ().item - 1;

		if (money - 10 >= 0) {

			itemCount [currentItem]++;
			money -= 10;

			// Update money and item count
			GameObject.FindGameObjectWithTag ("Money").GetComponentInChildren<Text> ().text = "Money: " + money;
			clickedButton.transform.Find ("ItemCount").gameObject.GetComponentInChildren<Text> ().text = "x " + itemCount [currentItem];
		}
	}
		
	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}*/
}