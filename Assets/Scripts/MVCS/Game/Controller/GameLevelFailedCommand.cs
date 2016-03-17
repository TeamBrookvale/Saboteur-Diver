using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

namespace TeamBrookvale.Game
{
	public class GameLevelFailedCommand : Command {

		[Inject]
		public IGameModel gameModel {get;set;}

		[Inject]
		public SingletonSoundFxSignal singletonSoundFxSignal {get;set;}

		[Inject]
		public PauseResumeLevelPassedFailedSignal pauseResumeLevelPassedFailedSignal {get;set;}

		public override void Execute ()
		{
			// play alarm audio
			singletonSoundFxSignal.Dispatch(SoundFx.PanicMode, SingletonSoundFxCmd.Play);
			
			// stop game
			pauseResumeLevelPassedFailedSignal.Dispatch (TeamBrookvale.UI.InGameMenuModel.MenuState.MissionFailed);
		}
	}
}