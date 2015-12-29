using UnityEngine;
using System.Collections;

public class ButtomScrolle : MonoBehaviour
{
	public static ButtomScrolle ins;
	
	public float Buttom_scrollSpeed;

	private float tileSize;
	
	private Vector3 startPosition;

	protected void Awake()
	{
		ins = this;
	}

	void Start ()
	{
		tileSize = gameObject.GetComponent<Renderer> ().bounds.size.x;
		startPosition = transform.position;
	}
	
	void FixedUpdate ()
	{
			float newPosition = Mathf.Repeat (Time.time * Buttom_scrollSpeed, tileSize);
			transform.position = startPosition + Vector3.left * newPosition;
	}
}
