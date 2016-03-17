using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

/*	
 	"Use arrow keys to navigate\n" +
	"yourself around the boats\n\n" +
	"Press and hold <SPACE> to use drill";


	"CREDITS\n\n" +
	"Programming: Gabe Merke\n" +
	"Game Design: Tom Merke\n" +
	"Soundtrack: Andras Erhard\n\n" +
	"Special thanks to: Freesound.org\nand\n" +
	"Users: Ampul, KizilSungur, Qubodup,\nChristian Muise, Martats, Robinhood76\n\n"+"TEAM BROOKVALE 2014";

	"This is a work of fiction.\n\nCharacters, places and incidents\n" +
	"are either products of the author’s imagination\nor are used fictitiously.\n\n" +
	"Any resemblance to actual events\nor locales or persons, living or dead,\nis entirely coincidental.";

	"\n\nThe heavily damaged enemy fleet\n" +
	"has been forced to turn back and\n" +
	"surrender to invading the port\n" +
	"until next time...";

 */

/*
 * string[] tutorialText = {
			"As a saboteur from the resistance fighters\nyou will sink the enemy's boats.",
			"The goal of your mission is to sneak into\nthe enemy dock and infiltrate\nanchored boats causing them\nto sink slowly",
			"Use arrow keys to navigate\nyourself around the boats",
			"Swim close to the anchored boat then\npress and hold <SPACE>\nto use your drill.",
			"Infiltrate only the anchored big boats!\nSmall patrol boats are a hazard\nbut cannot be in infiltrated",
			"Stay out of the light beam\nto avoid getting caught.",
			"Good luck camarade!",
			""};
		
		string[] firstLevelText = {
			"The enemy fleet has moved to our\ntown's port and according to\ndeciphered communications\nthey will invade us before sunrise.", 
			"However the underground resistance\nhas a plan to sabotage the fleet.",
			"MISSION BRIEF\n\nInfiltrate all anchored boats.\nWatch for guards in small patrol boats.", "" };
			*/

namespace TeamBrookvale.UI
{
	public class TitleTextView : View
	{
		TeamBrookvale.Game.IGameModel gameModel;
		GUIStyle guiStyle = new GUIStyle ();
		string textToShow;

		internal void init (ILevelTextModel model, TeamBrookvale.Game.IGameModel gameModel)
		{
			// initialize text
			guiStyle.font = model.AtwriterFont;
			guiStyle.alignment = TextAnchor.MiddleCenter;
			guiStyle.normal.textColor = Color.white;
			guiStyle.fontSize = TeamBrookvale.Game.TBUtil.CalcFontSize (30);

			// load level name
			this.gameModel = gameModel;

			// set text
			textToShow = gameModel.getCurrentLevelName ();
		}

		void Update ()
		{
			StartCoroutine (ClearTextDelayed ());

		}

		void OnGUI ()
		{
			GUI.Label (
				new Rect(0, Screen.height * .2f, Screen.width, Screen.height * .2f),
				textToShow,
				guiStyle);

		}

		public void OnGameLevelPassed ()
		{
			textToShow = "MISSION ACCOMPLISHED" + getLevelEndStatistics ();
		}
		
		public void OnGameLevelFailed ()
		{
			textToShow = "MISSION FAILED" + getLevelEndStatistics ();
		}

		string getLevelEndStatistics ()
		{
			return "\n\n"+
				"Time: " + Time.timeSinceLevelLoad.ToString("F2") + " seconds\n" +
				"Close calls: " + gameModel.numberOfSpotLightPlayerNoticedCommands + "\n" +
				"Enemy Alerts: " + gameModel.numberOfSpotPanicModeCommands;
		}

		IEnumerator ClearTextDelayed ()
		{
			// wait 3 seconds
			yield return new WaitForSeconds (3f);
			
			// if still the level name is shown then clear it
			if (textToShow == gameModel.getCurrentLevelName())
				textToShow = "";
		}
	}
}
