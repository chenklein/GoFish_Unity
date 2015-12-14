using UnityEngine;
using System.Collections;

public class parralax : MonoBehaviour {

	public static parralax ins;

	public float ParallaxSpeedBackground;
	public float ParallaxSpeedButtom;
	private float newOffset;

	protected void Awake()
	{
		ins = this;
	}
	
	protected void FixedUpdate()
	{
		if (gameObject.name == "ParralaxBG")
		{
			newOffset += ParallaxSpeedBackground * Time.deltaTime;
			Vector2 offset = new Vector2(newOffset, 0);
			GetComponent<Renderer> ().material.mainTextureOffset = offset;
		}
		else if (gameObject.name == "Parralaxbottom")
		{
			newOffset += ParallaxSpeedButtom * Time.deltaTime;
			Vector2 offset = new Vector2(newOffset, 0);
			GetComponent<Renderer> ().material.mainTextureOffset = offset;
		}
	}

}
