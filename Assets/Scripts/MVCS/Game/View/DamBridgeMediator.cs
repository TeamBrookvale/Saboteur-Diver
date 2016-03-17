using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.Game
{
	public class DamBridgeMediator : Mediator
	{
		[Inject]
		public DamBridgeView view {get;set;}

		[Inject]
		public IDamBridgeModel damBridgeModel {get;set;}

		[Inject]
		public InventoryEventFireSignal inventoryEventFireSignal {get;set;}

		[Inject]
		public IPlayerModel playerModel {get;set;}

		public override void OnRegister ()
		{
			view.init (damBridgeModel, playerModel);

			view._inventoryEventFireSignal.AddListener (inventoryEventFireSignal.Dispatch);
		}

		public override void OnRemove ()
		{
			view._inventoryEventFireSignal.RemoveListener (inventoryEventFireSignal.Dispatch);
		}
	}
}