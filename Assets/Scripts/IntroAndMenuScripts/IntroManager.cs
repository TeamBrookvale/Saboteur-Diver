using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBrookvale.NonMVCS
{
	public class IntroManager : MonoBehaviour
	{
		bool[] isLevelLoaded = { false, false, false };
		float[] loadAtTime = { 4, 8, 12 };
		string[] scenes = { "IntroScene02", "IntroScene03", "IntroScene04" };

		IList keepGameObjects;

		void Start ()
		{
			keepGameObjects = new List<GameObject> ();
			keepGameObjects.Add (this.gameObject);
			keepGameObjects.Add (GameObject.Find ("IntroMusic"));
			keepGameObjects.Add (GameObject.Find ("Main Camera"));
			keepGameObjects.Add (GameObject.Find ("BlackPixel"));

			if (keepGameObjects.Contains (null))
				Debug.LogError ("IntroMusic could not be cached for non deletion");

			Time.timeScale = 1;
		}
		
		void Update ()
		{
			for (int i = 0; i < 3; i++)
			{
				if (Time.timeSinceLevelLoad > loadAtTime [i] && !isLevelLoaded [i])
				{
					isLevelLoaded [i] = true;
					destroyGameObjects ();
					Application.LoadLevelAdditive (scenes [i]);
				}
			}
		}

		void destroyGameObjects ()
		{
			foreach (GameObject o in (GameObject[]) GameObject.FindObjectsOfType (typeof (GameObject)))
				if (!keepGameObjects.Contains (o))
					GameObject.Destroy ((GameObject) o);
		}
	}
}