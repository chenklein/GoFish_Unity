using UnityEngine;
using System.Collections;

public class PlatformMover : MonoBehaviour {

	private float ScreenLeftBoundary;
	//private float GameObjectWidth;


	void Awake()
	{
		Vector3 LeftCorner = new Vector3 (Screen.width - Screen.width  , Screen.height, 10f);
		Vector3 worldLeftCorner = Camera.main.ScreenToWorldPoint(LeftCorner);
		ScreenLeftBoundary = worldLeftCorner.x;
		//GameObjectWidth = gameObject.GetComponent<Renderer> ().bounds.size.x;
	}



	void Update()
	{

		//float newPosition = Mathf.Repeat (Time.time * BG_scrollSpeed, tileSize);
		//transform.position = Vector2.left * GameManager.ins.GameSpeed * Time.deltaTime;

		transform.Translate (-Vector2.right * GameManager.ins.GameSpeed * Time.deltaTime);
			
			if ((gameObject.transform.position.x + 10) < (ScreenLeftBoundary)) {
				Destroy (gameObject);
			}
	}

	

}
