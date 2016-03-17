using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.context.api;

namespace TeamBrookvale.Game
{
	public class InventoryIconPressCommand : Command {

		[Inject]
		public TeamBrookvale.InventoryEventFireSignal inventoryEventFireSignal {get;set;}

		[Inject]
		public bool isButtonPushedOrReleased {get;set;}

		[Inject]
		public IInventoryModel inventoryModel {get;set;}

		[Inject]
		public IPlayerModel playerModel {get;set;}

		[Inject]
		public PlayerUBoatEmbarkSignal playerUBoatEmbarkSignal {get;set;}

		[Inject]
		public PlayerActionSignal playerActionSignal {get;set;}

		[Inject]
		public IDamBridgeModel damBridgeModel {get;set;}

		public override void Execute ()
		{
			if (isButtonPushedOrReleased)
				inventoryEventFireSignal.Dispatch (InventoryModel.Events.ButtonPush, TouchScreenPosition.zero);

			// embark / disembark uboat
			if (isButtonPushedOrReleased && inventoryModel.getCurrentInvItem().id == InvItem.IDType.UBoat)
				playerUBoatEmbarkSignal.Dispatch (!playerModel.isEmbarkedUBoat);

			// drill boat with pressing and holding icon
			if (inventoryModel.getCurrentInvItem().id == InvItem.IDType.DrillActive)
				playerActionSignal.Dispatch (TouchScreenPosition.zero, isButtonPushedOrReleased);

			// mount bomb
			if (inventoryModel.getCurrentInvItem().id == InvItem.IDType.TimeBombActive)
				playerActionSignal.Dispatch (damBridgeModel.lastCollisionTouchScreenPosition, isButtonPushedOrReleased);
		}
	}
}