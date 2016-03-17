using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour {

	int frameCount;
	float dt;
	float fps;
	float updateRate = 2f;  // 4 updates per sec.
	
	void Update () {
		frameCount++;
		dt += Time.deltaTime;
		if (dt > 1f / updateRate)
		{
			fps = frameCount / dt;
			frameCount = 0;
			dt -= 1f / updateRate;
		}

		GetComponent<GUIText>().text = ((int)fps).ToString();
	}
}
