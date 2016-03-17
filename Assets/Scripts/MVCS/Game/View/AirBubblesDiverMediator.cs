using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class AirBubblesDiverMediator : Mediator
	{
		[Inject]
		public AirBubblesDiverView view {get;set;}

		[Inject]
		public PlayerUBoatEmbarkSignal playerUBoatEmbarkSignal {get;set;}

		[Inject]
		public IPlayerModel playerModel {get;set;}

		public override void OnRegister ()
		{
			playerUBoatEmbarkSignal.AddListener (view.OnPlayerUBoatEmbarkSignal);

			view.init (playerModel.getCachedPosition);
		}

		public override void OnRemove ()
		{
			playerUBoatEmbarkSignal.RemoveListener (view.OnPlayerUBoatEmbarkSignal);
		}
	}
}