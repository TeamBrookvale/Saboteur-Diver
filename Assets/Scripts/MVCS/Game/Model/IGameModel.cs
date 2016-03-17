using System.Collections.Generic;
using UnityEngine;

namespace TeamBrookvale.Game
{
	public interface IGameModel
	{
		int currentLevel {get;}
		bool areAllShipsSunkOrBombsMounted {get;set;}
		PauseStatus pauseStatus {get;set;}
		string getCurrentLevelName ();
		void levelPassed ();
		bool isSmokeBombEnabledOnThisLevel ();
		bool isInGameMusicEnabledOnThisLevel ();
		int numberOfSpotLightPlayerNoticedCommands {get;set;}
		int numberOfSpotPanicModeCommands {get;set;}
		LevelType currentLevelType {get;set;}
		List<KeyValuePair<GOType,Transform>> triggerItems {get;set;}
		void postXMLItemsLoaded (Transform parentOfXmlItems);
		int numberOfCurrentPanicModeCommand {get;set;}
	}
}