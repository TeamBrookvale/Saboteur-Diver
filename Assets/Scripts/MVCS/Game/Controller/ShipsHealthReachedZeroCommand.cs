using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

namespace TeamBrookvale.Game
{
	public class ShipsHealthReachedZeroCommand : Command
	{
		[Inject]
		public PlayerActionSignal playerActionSignal {get;set;}

		[Inject]
		public InventoryEventFireSignal inventoryEventFireSignal {get;set;}

		public override void Execute ()
		{
			playerActionSignal.Dispatch (TouchScreenPosition.zero, false);
			inventoryEventFireSignal.Dispatch (InventoryModel.Events.ApproachSunkShip, TouchScreenPosition.zero);
		}
	}
}