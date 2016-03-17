using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.context.api;

namespace TeamBrookvale.UI
{
	public class PauseResumeLevelPassedFailedCommand : Command
	{
		// signal parameter
		[Inject]
		public InGameMenuModel.MenuState inGameMenuModelState {get;set;}

		[Inject]
		public IInGameMenuModel inGameMenuModel {get;set;}

		[Inject]
		public IRoutineRunner routineRunner {get;set;}

		[Inject]
		public TeamBrookvale.Game.IGameModel gameModel {get;set;}

		[Inject]
		public InGameMenuFadeSignal inGameMenuFadeSignal {get;set;}

		[Inject]
		public InGameMenuShowSignal inGameMenuShowSignal {get;set;}

		public override void Execute ()
		{
			// update the model's state from the signal
			inGameMenuModel.menuState = inGameMenuModelState;

			// if not paused then pause
			switch (gameModel.pauseStatus)
			{
			case TeamBrookvale.Game.PauseStatus.RUN:
				gameModel.pauseStatus = TeamBrookvale.Game.PauseStatus.PAUSE_IN_PROGRESS;
				inGameMenuFadeSignal.Dispatch (true);
				routineRunner.StartCoroutine (ActionDelayed ());
				break;

			case TeamBrookvale.Game.PauseStatus.PAUSE:
				inGameMenuFadeSignal.Dispatch (false);
				routineRunner.StartCoroutine (ActionDelayed ());
				break;

			default: break;
			}
		}

		IEnumerator ActionDelayed ()
		{
			float start = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < start + .25f) // assuming .25 is the length of the background animation fade
			{
				yield return null;
			}

			gameModel.pauseStatus = 
				(gameModel.pauseStatus == TeamBrookvale.Game.PauseStatus.PAUSE_IN_PROGRESS ? TeamBrookvale.Game.PauseStatus.PAUSE : TeamBrookvale.Game.PauseStatus.RUN);

			inGameMenuShowSignal.Dispatch (gameModel.pauseStatus == TeamBrookvale.Game.PauseStatus.PAUSE, inGameMenuModelState);
		}
	}
}