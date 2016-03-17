using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

namespace TeamBrookvale.Game
{
	public class GameLevelPassedCommand : Command {

		[Inject]
		public IGameModel gameModel {get;set;}

		[Inject]
		public SingletonSoundFxSignal singletonSoundFxSignal {get;set;}

		[Inject]
		public PauseResumeLevelPassedFailedSignal pauseResumeLevelPassedFailedSignal {get;set;}
		
		public override void Execute ()
		{
			// set next level to load
			gameModel.levelPassed();
			
			// play alarm audio
			singletonSoundFxSignal.Dispatch(SoundFx.LevelPassed, SingletonSoundFxCmd.Play);

			// show game end menu
			pauseResumeLevelPassedFailedSignal.Dispatch (TeamBrookvale.UI.InGameMenuModel.MenuState.MissionAccomplished);
		}
	}
}