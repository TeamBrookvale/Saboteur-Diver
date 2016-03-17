using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

namespace TeamBrookvale.Game
{
	public class AllShipsSunkOrBombsMountedCommand : Command
	{
		[Inject]
		public IGameModel gameModel {get;set;}

		[Inject]
		public StatusBarMessageSignal statusBarMessageSignal {get;set;}

		public override void Execute ()
		{
			// update the game model
			gameModel.areAllShipsSunkOrBombsMounted = true;

			// dispatch a status bar message signal
			statusBarMessageSignal.Dispatch (Const.StatusBarBackToGetawayBoat);
		}
	}
}