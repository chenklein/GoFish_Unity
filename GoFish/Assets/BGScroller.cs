using UnityEngine;
using System.Collections;

public class BGScroller : MonoBehaviour
{
	public static BGScroller ins;

	public float scrollSpeed;
	public float tileSizeZ;
	
	private Vector2 savedOffset;
	private Vector3 startPosition;
	
	void Start ()
	{
		ins = this;
		startPosition = transform.position;
		savedOffset = GetComponent<Renderer>().sharedMaterial.GetTextureOffset ("_MainTex");
	}
	
	void Update ()
	{
		float x = Mathf.Repeat (Time.time * scrollSpeed, tileSizeZ * 4);
		x = x / tileSizeZ;
		x = Mathf.Floor (x);
		x = x / 4;
		Vector2 offset = new Vector2 (x, savedOffset.y);
		GetComponent<Renderer>().sharedMaterial.SetTextureOffset ("_MainTex", offset);
		float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
		transform.position = startPosition + Vector3.left * newPosition;
	}
	
	void OnDisable () {
		GetComponent<Renderer>().sharedMaterial.SetTextureOffset ("_MainTex", savedOffset);
	}


//	public static BGScroller ins;
//
//	public float BG_scrollSpeed;
//	private float tileSize;
//	
//	private Vector3 startPosition;
//
//	protected void Awake()
//	{
//		ins = this;
//	}
//
//	void Start ()
//	{
//		tileSize = gameObject.GetComponent<Renderer> ().bounds.size.x;
//		startPosition = transform.position;
//	}
//	
//	void LateUpdate ()
//	{
//
//			float newPosition = Mathf.Repeat (Time.time * BG_scrollSpeed, tileSize);
//			transform.position = startPosition + Vector3.left * newPosition;
//
//	}
}
