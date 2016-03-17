using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class CamaradeBoatMediator : Mediator
	{
		[Inject]
		public CamaradeBoatView view {get;set;}

		[Inject]
		public PlayerSwimmedToCamaradeBoatSignal playerSwimmedToCamaradeBoatSignal {get;set;}
		
		public override void OnRegister ()
		{
			view._playerSwimmedToCamaradeBoatSignal.AddListener (playerSwimmedToCamaradeBoatSignal.Dispatch);
		}

		public override void OnRemove ()
		{
			view._playerSwimmedToCamaradeBoatSignal.RemoveListener (playerSwimmedToCamaradeBoatSignal.Dispatch);
		}
	}
}