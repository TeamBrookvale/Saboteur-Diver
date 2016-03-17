using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class MineMediator : Mediator
	{
		[Inject]
		public MineView view {get;set;}

		[Inject]
		public GameLevelFailedSignal gameLevelFailedSignal {get;set;}

		public override void OnRegister ()
		{
			view.gameLevelFailedSignal.AddListener (gameLevelFailedSignal.Dispatch);
		}

		public override void OnRemove ()
		{
			view.gameLevelFailedSignal.RemoveListener (gameLevelFailedSignal.Dispatch);
		}
	}
}