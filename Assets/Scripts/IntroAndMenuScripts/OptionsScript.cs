using UnityEngine;
using System.Collections;
using TeamBrookvale;

namespace TeamBrookvale.NonMVCS
{
	public class OptionsScript : MenuSuperClass {

		float lastPressOrLoadTime;
		bool isMusicPressed;
		bool isRestoreIAPPressed;
		bool isRestoreIAPInProgress;
		bool isBackPressed;

		// populated by the IAPInit's events
		public static string RestoreIAPText = "Restore in-app purchases";

		protected override void Start ()
		{
			base.Start ();

			lastPressOrLoadTime = Time.time;
		}

		void OnGUI ()
		{
			textGUIStyle.fontSize = base.largeFontSize;

			GUI.Label (
				new Rect (
					Screen.width  * .5f,
					Screen.height * .2f,
					0,0),
				"OPTIONS",
				textGUIStyle);

			textGUIStyle.fontSize = base.smallFontSize;
			
			if (GUI.RepeatButton (
					new Rect (
					0,
					Screen.height * .28f,
					Screen.width,
					Screen.height * .2f),
					"Music " + (PlayerPrefs.GetInt (Const.PlayerPrefsMusicOn) == 1 ? "On" : "Off"),
					textGUIStyle))
				isMusicPressed = true;

			if (GUI.RepeatButton (
					new Rect (
					0,
					Screen.height * .5f,
					Screen.width,
					Screen.height * .2f),
					RestoreIAPText,
					textGUIStyle))
			    isRestoreIAPPressed = true;

			if (GUI.RepeatButton (
					new Rect (
					0,
					Screen.height * .7f,
					Screen.width,
					Screen.height * .2f),
					"Back",
					textGUIStyle))
				isBackPressed = true;
		}

		void Update ()
		{
			// load main menu on any click after 0.2 seconds after load
			if (lastPressOrLoadTime + .4f < Time.time)
			{
				lastPressOrLoadTime = Time.time;

				if (isRestoreIAPPressed && !isRestoreIAPInProgress)
				{
					isRestoreIAPInProgress = true;
					RestoreIAPText = "Restoring in-app purchases in progress";
					Soomla.Store.SoomlaStore.RestoreTransactions();
				}

				if (isMusicPressed)
					PlayerPrefs.SetInt (Const.PlayerPrefsMusicOn, 1 - PlayerPrefs.GetInt (Const.PlayerPrefsMusicOn));

				if (isBackPressed)
					AddScriptAndRemoveThisOne ("MainMenuScript");

				isMusicPressed = isRestoreIAPPressed = isBackPressed = false;
			}
		}
	}
}