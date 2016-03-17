using UnityEngine;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class InGameMusicMediator : Mediator
	{
		[Inject]
		public InGameMusicView view {get;set;}

		[Inject]
		public IGameModel gameModel {get;set;}

		public override void OnRegister ()
		{
			view.init (gameModel.isInGameMusicEnabledOnThisLevel ());
		}
	}
}