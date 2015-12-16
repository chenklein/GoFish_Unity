using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

	//public static Spawner ins;

	public TextMesh FishText;
	public SpriteRenderer mImage;
	//public Image fishImage;

	void Awake()
	{

		if (gameObject.name == "correct(Clone)") {

			FishText.text = GameManager.ins.CorrectLetter;
			mImage.sprite = GameManager.ins.AllFishesSprites[Random.Range(0,GameManager.ins.AllFishesSprites.Count)];
			//fishImage.sprite = GameManager.ins.AllFishesSprites[Random.Range(0,GameManager.ins.AllFishesSprites.Count)];

		} else if (gameObject.name == "wrong(Clone)") {

			int RandomWrongLetter = Random.Range(0, GameManager.ins.WrongLetters.Count);
			FishText.text = GameManager.ins.WrongLetters[RandomWrongLetter];
			mImage.sprite = GameManager.ins.AllFishesSprites[Random.Range(0,GameManager.ins.AllFishesSprites.Count)];
			//fishImage.sprite = GameManager.ins.AllFishesSprites[Random.Range(0,GameManager.ins.AllFishesSprites.Count)];

		} else if (gameObject.name == "Obstacle(Clone)") {

			FishText.text = "";
			mImage.sprite = GameManager.ins.AllObsticlesSprites[Random.Range(0,GameManager.ins.AllObsticlesSprites.Count)];
			//fishImage.sprite = GameManager.ins.AllObsticlesSprites[Random.Range(0,GameManager.ins.AllObsticlesSprites.Count)];


		//	gameObject.GetComponent<BoxCollider2D>().size = new Vector2(3,3);

		}


	}
					                                                             



}
