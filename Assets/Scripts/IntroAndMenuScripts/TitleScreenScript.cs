using UnityEngine;
using System.Collections;

namespace TeamBrookvale.NonMVCS
{
	public class TitleScreenScript : MonoBehaviour
	{

		void Start ()
		{
			// set dpi for text
			TeamBrookvale.Game.TBUtil.ScreenDPI = Screen.dpi;
			TeamBrookvale.Game.TBUtil.ScreenHeight = Screen.height;
			Const.ScreenHeight = Screen.height;

			Camera camera = GameObject.Find ("Main Camera").GetComponent<Camera>();

			if (camera == null) Debug.LogError ("Camera not found");

			if ((float) Screen.width / Screen.height > 1.5f)
				camera.orthographicSize = 3.15f;	// 16:9
			else
				camera.orthographicSize = 3.8f;  //  4:3

			if (!PlayerPrefs.HasKey (Const.PlayerPrefsMusicOn))
				PlayerPrefs.SetInt (Const.PlayerPrefsMusicOn, 1);
	    }

		void Update ()
		{
			if (2 < Time.time || 0 < Input.touchCount || Input.GetMouseButtonDown(0))
				Application.LoadLevel ("MainMenuScene");
		}
	}
}