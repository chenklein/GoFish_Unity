using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

	public TextMesh FishText;
	public SpriteRenderer mImage;
	private float ScreenLeftBoundary;

	void Awake()
	{
		Vector3 LeftCorner = new Vector3 (Screen.width - Screen.width  , Screen.height, 10f);
		Vector3 worldLeftCorner = Camera.main.ScreenToWorldPoint(LeftCorner);
		ScreenLeftBoundary = worldLeftCorner.x;

		if (gameObject.name == "correct(Clone)") {

			FishText.text = GameManager.ins.CorrectLetter;
			mImage.sprite = GameManager.ins.AllFishesSprites[Random.Range(0,GameManager.ins.AllFishesSprites.Count)];

		} else if (gameObject.name == "wrong(Clone)") {

			int RandomWrongLetter = Random.Range(0, GameManager.ins.WrongLetters.Count);
			FishText.text = GameManager.ins.WrongLetters[RandomWrongLetter];
			mImage.sprite = GameManager.ins.AllFishesSprites[Random.Range(0,GameManager.ins.AllFishesSprites.Count)];

		} else if (gameObject.name == "Obstacle(Clone)") {

			FishText.text = "";
			mImage.sprite = GameManager.ins.AllObsticlesSprites[Random.Range(0,GameManager.ins.AllObsticlesSprites.Count)];

		}


	}
	

	void Update()
	{
		transform.Translate (Vector2.left * GameManager.ins.GameSpeed * Time.deltaTime);
		
		if ((gameObject.transform.position.x + 10) < (ScreenLeftBoundary)) {
			Destroy (gameObject);
		}
	}

}
