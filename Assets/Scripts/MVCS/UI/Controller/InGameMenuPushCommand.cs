using strange.extensions.command.impl;
using UnityEngine;

namespace TeamBrookvale.UI
{
	public class InGameMenuPushCommand : Command
	{
		// signal parameter
		[Inject]
		public InGameMenuModel.Icon icon {get;set;}

		[Inject]
		public TeamBrookvale.PauseResumeLevelPassedFailedSignal pauseResumeLevelPassedFailedSignal {get;set;}

		[Inject]
		public IInGameMenuModel model {get;set;}

		[Inject]
		public LoadNextLevelSignal loadNextLevelSignal {get;set;}

		[Inject]
		public RetryLevelSignal retryLevelSignal {get;set;}

		public override void Execute ()
		{
			switch (icon)
			{
			case InGameMenuModel.Icon.InAppPayment:
				model.isShowingIAPButtons = true;
				break;

			case InGameMenuModel.Icon.Menu:
				Application.LoadLevel ("MainMenuScene");
				break;

			case InGameMenuModel.Icon.Resume:

				// if paused then resume
				if (model.menuState == InGameMenuModel.MenuState.Pause)
					pauseResumeLevelPassedFailedSignal.Dispatch (InGameMenuModel.MenuState.Pause);

				// if mission accomplished then play the next level
				else
					loadNextLevelSignal.Dispatch ();
				break;

			case InGameMenuModel.Icon.Retry:
				retryLevelSignal.Dispatch ();
				break;

			default:
				break;
			}
		}
	}
}