using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.UI
{
	public class CursorMediator : Mediator
	{
		[Inject]
		public PlayerGotoSignal playerGotoSignal {get;set;}

		[Inject]
		public CursorView view {get;set;}

		[Inject]
		public TeamBrookvale.Game.IGameModel gameModel {get;set;}

		public override void OnRegister ()
		{
			playerGotoSignal.AddListener (OnPlayerGotoSignal);
		}

		public override void OnRemove ()
		{
			playerGotoSignal.RemoveListener (OnPlayerGotoSignal);
		}

		void OnPlayerGotoSignal (TouchScreenPosition p)
		{
			view.playClickAnimationAt (p.world, gameModel.pauseStatus == TeamBrookvale.Game.PauseStatus.RUN);
		}
	}
}