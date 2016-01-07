using UnityEngine;
using System.Collections;

public class KillFish : MonoBehaviour {

	public GameObject mGameObject;

	public void DestroyGameObject()
	{

		Destroy (mGameObject);
	}
}
