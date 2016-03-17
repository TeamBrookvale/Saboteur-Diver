using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBrookvale.NonMVCS
{
	public class MainMenuScript : MenuSuperClass {

		Dictionary <string,string> items = new Dictionary <string,string> ();
		float margin = .13f; // top and bottom margin in percent

		protected override void Start ()
		{
			base.Start ();

			items.Add ("START GAME",	"StartGameScript");
			items.Add ("OPTIONS",		"OptionsScript");
			items.Add ("CREDITS",		"CreditsScript");
		}

		void OnGUI ()
		{
			int i = 0;

			foreach (KeyValuePair <string,string> kvp in items)
			{
				if (GUI.RepeatButton (
						new Rect (
							0,
							Screen.height * margin + i / 3f * (Screen.height * (1 - 2 * margin)),
					        Screen.width,
							1f / 3f * (Screen.height * (1 - 2 * margin))),
						kvp.Key,
						textGUIStyle))
				{
					// e.g. add CreditsScript and destroy the current one
					AddScriptAndRemoveThisOne (kvp.Value);
				}

				i++;
			}
		}
	}
}