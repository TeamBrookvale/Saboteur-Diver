using UnityEngine;
using System.Collections;

public class LoadGameSceneDelayed : MonoBehaviour {

	public float delay;
	public bool loadOutroSceneAfterThis;

	void Update ()
	{
		if (Time.timeSinceLevelLoad > delay)
			if (loadOutroSceneAfterThis)
				Application.LoadLevel ("OutroScene01");
			else
				Application.LoadLevel ("GameScene");
	}
}
