using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {

	public GameObject mGameObject;
	public SpriteRenderer mSpriteRenderer;
	public MeshRenderer mMeshRenderer;

	public void DisableGameObject()
	{
		mSpriteRenderer.enabled = false;
		mMeshRenderer.enabled = false;
	}

	public void DestroyGameObject()
	{
		Destroy (mGameObject);
	}
}
