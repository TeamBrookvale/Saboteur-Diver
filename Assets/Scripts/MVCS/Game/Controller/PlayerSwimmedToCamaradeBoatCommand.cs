using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

namespace TeamBrookvale.Game
{
	public class PlayerSwimmedToCamaradeBoatCommand : Command
	{
		[Inject]
		public IGameModel gameModel {get;set;}

		[Inject]
		public GameLevelPassedSignal gameLevelPassedSignal {get;set;}

		public override void Execute ()
		{
			if (gameModel.areAllShipsSunkOrBombsMounted)
				gameLevelPassedSignal.Dispatch ();
		}	
	}
}