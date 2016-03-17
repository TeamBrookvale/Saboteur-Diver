using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class SmokeBombCircleMediator : Mediator
	{
		[Inject]
		public SmokeBombCircleView view {get;set;}

		[Inject]
		public ISmokeBombCircleModel smokeBombCircleModel {get;set;}

		[Inject]
		public IPlayerModel playerModel {get;set;}

		[Inject]
		public CurrentInventoryIconSignal currentInventoryIconSignal {get;set;}

		[Inject]
		public IInventoryModel inventoryModel {get;set;}

		public override void OnRegister ()
		{
			currentInventoryIconSignal.AddListener (view.OnCurrentInventoryIconSignal);

			view.init (playerModel.getCachedPosition);

			// cache smoke bomb circle model
			smokeBombCircleModel.smokeBombCircleGameObject = view.gameObject;
		}
		
		public override void OnRemove ()
		{
			currentInventoryIconSignal.RemoveListener (view.OnCurrentInventoryIconSignal);		
		}
	}
}