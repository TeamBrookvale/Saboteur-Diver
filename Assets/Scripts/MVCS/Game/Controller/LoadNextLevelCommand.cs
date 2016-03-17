using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

namespace TeamBrookvale.Game
{
	public class LoadNextLevelCommand : Command {

		[Inject]
		public IGameModel gameModel {get;set;}

		public override void Execute ()
		{
			PlayerPrefs.SetInt ("LevelToLoad", gameModel.currentLevel + 1);

			switch (gameModel.currentLevelType)
			{
			case  LevelType.Bridge:
				Time.timeScale = 1;
				Application.LoadLevel ("LevelCompletedBridgeScene");
				break;

			case LevelType.Dam:
				Time.timeScale = 1;
				Application.LoadLevel ("LevelCompletedDamScene");
				break;

			case LevelType.ShipDrill:
				Time.timeScale = 1;
				Application.LoadLevel ("LevelCompletedShipDrillableScene");
				break;

			default:
				Application.LoadLevel ("GameScene");
				break;
			}
		}
	}
}