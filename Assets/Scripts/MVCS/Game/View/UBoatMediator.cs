using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class UBoatMediator : Mediator
	{
		[Inject]
		public UBoatView view {get;set;}

		[Inject]
		public IUBoatModel model {get;set;}

		[Inject]
		public IPlayerModel playerModel {get;set;}

		[Inject]
		public UBoatPlayerCloseSignal uBoatPlayerCloseSignal {get;set;}

		override public void OnRegister()
		{
			view.uBoatPlayerCloseSignal.AddListener (uBoatPlayerCloseSignal.Dispatch);

			view.init (model, playerModel.getCachedPosition);
		}
		
		override public void OnRemove()
		{
			view.uBoatPlayerCloseSignal.AddListener (uBoatPlayerCloseSignal.Dispatch);
		}
	}
}