using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TeamBrookvale.Game
{
	public class GameModel : IGameModel
	{
		// this level is or will be loaded
		public int currentLevel {get; private set;}
		public bool areAllShipsSunkOrBombsMounted {get;set;}
		public PauseStatus pauseStatus {get;set;}
		public int numberOfSpotLightPlayerNoticedCommands {get;set;}
		public int numberOfSpotPanicModeCommands {get;set;}
		public LevelType currentLevelType {get;set;}
		public List<KeyValuePair<GOType,Transform>> triggerItems {get;set;} // dam, bridge, ship, tutorial etc items filled by startcommand
		public int numberOfCurrentPanicModeCommand {get;set;}

		string[] levelNames = {
			"Level 1 Tutorial 1\n\nSink or Swim",
			"Level 2 Tutorial 2\n\nKnow the drill", 
			"Level 3 - First Mission\n\nObjective:\nSink all anchored boats", 
			"Level 4 Tutorial 3\n\nDistraction", 
			"Level 5 - Entrance\n\nObjective:\nSink all anchored boats", 
			"Level 6 - Middle Camp\n\nObjective:\nSink all anchored boats",
			"Level 7 - Little Bay Port\n\nObjective:\nSink all anchored boats", 
			"Level 8 - Lagoon\n\nObjective:\nSink all anchored boats", 
			"Level 9 - Das Boot\n\nObjective:\nSink all anchored boats\n\nHint:\nSteal the u-boat and move around unnoticed ", 
			"Level 10 - Rocky Bay\n\nObjective:\nSink all anchored boats", 
			"Level 11 Tutorial 4\n\nTick-tack", 
			"Level 12 - Bridge Bay\n\nObjective:\nPlant bombs on the bridge",
			"Level 13 - Wilson Piers\n\nObjective:\nSink all anchored boats", 
			"Level 14 - Eastern Point Wharf\n\nObjective:\nSink all anchored boats", 
			"Level 15 - Southern Docks\n\nObjective:\nSink all anchored boats",  
			"Level 16 - Boatyard\n\nObjective:\nSink all anchored boats", 
			"Level 17 - White Harbour\n\nObjective:\nSink all anchored boats", 
			"Level 18 - Seafort\n\nObjective:\nSink all anchored boats", 
			"Level 19 - Damolition\n\nObjective:\nPlant bombs on the dam" 
		};

		int[] noSmokeBombOnLevels = { 1, 2, 3 };
		int[] noInGameMusicOnLevels = { 1, 2, 4, 11 }; // no music on tutorial levels

		public GameModel ()
		{
			// -1 means use the <Developer StartLevel="5" /> from the XML
			currentLevel = PlayerPrefs.GetInt ("LevelToLoad");

			// initialize list
			triggerItems = new List<KeyValuePair<GOType, Transform>> ();
		}

		public void levelPassed ()
		{
			// update max level passed
			PlayerPrefs.SetInt ("MaxLevelPassed", Mathf.Max (PlayerPrefs.GetInt ("MaxLevelPassed"), currentLevel));

			// level completed
			PlayerPrefs.SetInt (TBUtil.LevelNumberToKey (currentLevel), 	(int) PlayerPrefsLevelKey.Completed);
			PlayerPrefs.SetInt (TBUtil.LevelNumberToKey (currentLevel + 1), (int) PlayerPrefsLevelKey.Open);
		}

		public string getCurrentLevelName ()
		{
			return levelNames [currentLevel - 1];
		}

		public void loadNextLevel ()
		{
			// load next level if there are levels left
			if (currentLevel < levelNames.Length)
				Application.LoadLevel (currentLevel + 1);

			// no more levels left 
			else
				Debug.Log ("No more levels left");
		}
		
		public bool isSmokeBombEnabledOnThisLevel ()
		{
			return !Array.Exists (noSmokeBombOnLevels, element => element == currentLevel);
		}

		public bool isInGameMusicEnabledOnThisLevel ()
		{
			return !Array.Exists (noInGameMusicOnLevels, element => element == currentLevel);
		}


		public void triggerItemsAdd (GOType g, Transform t)
		{
			triggerItems.Add (new KeyValuePair <GOType, Transform> (g, t));				
		}

		public void postXMLItemsLoaded (Transform parentOfXmlItems)
		{
			// put xml items child into a dictionary
			foreach (Transform t in parentOfXmlItems)
			{
				if (t.GetComponentInChildren <DamBridgeView> () != null)
				{
					if (t.name.Contains ("Bridge"))						triggerItemsAdd (GOType.Bridge, t);
					if (t.name.Contains ("Buoy"))						triggerItemsAdd (GOType.Buoy, t);
					if (t.name.Contains ("Dam"))						triggerItemsAdd (GOType.Dam, t);
				}
				if (t.GetComponent<TutorialGameObjectsView>() != null)	triggerItemsAdd (GOType.Tutorial, t);
				if (t.name.Contains ("Dock"))							triggerItemsAdd (GOType.Dock, t);
				
				if (t.GetComponent <ShipDrillableView> () != null)		triggerItemsAdd (GOType.ShipDrillable, t);
				if (t.GetComponent <UBoatView> () != null)				triggerItemsAdd (GOType.UBoat, t);
			}
			
			// if only tutorial element is in the list then areAllShipsSunkOrBombsMounted is true
			if ((from e in triggerItems where (GOType) e.Key != GOType.Tutorial select e).Count() == 0)
				areAllShipsSunkOrBombsMounted = true;
			
			// determine level type - default is NothingToDemolish
			foreach (KeyValuePair <GOType, Transform> e in triggerItems)
			{
				if (e.Key == GOType.Buoy) 			currentLevelType = LevelType.Buoy;
				if (e.Key == GOType.Bridge)			currentLevelType = LevelType.Bridge;
				if (e.Key == GOType.Dam)			currentLevelType = LevelType.Dam;
				if (e.Key == GOType.ShipDrillable)	currentLevelType = LevelType.ShipDrill;
			}
		}
	}
}