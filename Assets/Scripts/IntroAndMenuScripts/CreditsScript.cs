using UnityEngine;
using System.Collections;

namespace TeamBrookvale.NonMVCS
{
	public class CreditsScript : MenuSuperClass {

		string credits = "Programming & Game Design - Gabe Merke\nArt & Game Design - Tom Merke\nMusic - Andras Erhard\n\nSpecial thanks to\nfreesound.org and users\nKizilsungur, Robinhood76, Pgi, Rocktopus, Tommccann,\nQubodup, Ampul, Flathill, Mikejedw, Davdud101\n\nTeam Brookvale\nSydney | Australia | 2015";

		float scriptAddedTime;

		void OnGUI ()
		{
			textGUIStyle.alignment = TextAnchor.MiddleCenter;
			textGUIStyle.fontSize = base.largeFontSize;

			GUI.Label (
				new Rect (
					Screen.width  * .2f,
					Screen.height * .12f,
					Screen.width  * (1 - 2 * .2f),
					Screen.height * 0),
				"CREDITS",
				textGUIStyle);

			textGUIStyle.alignment = TextAnchor.UpperCenter;
			textGUIStyle.fontSize = base.smallFontSize;

			GUI.Label (
					new Rect (
						Screen.width  * .2f,
						Screen.height * .23f,
				        Screen.width  * (1 - 2 * .2f),
						Screen.height * (1 - 2 * .23f)),
					credits,
					textGUIStyle);
		}

		void Update ()
		{
			if (GameObject.Find ("-OutroManager-"))
			    return;

			// load main menu on any click after 0.2 seconds after load
			if (scriptAddedTime == 0) scriptAddedTime = Time.time;
			if (scriptAddedTime + .2f < Time.time)
				if (0 < Input.touchCount || Input.GetMouseButtonDown(0))
					AddScriptAndRemoveThisOne ("MainMenuScript");
		}
	}
}