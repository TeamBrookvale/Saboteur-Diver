using strange.extensions.command.impl;

namespace TeamBrookvale.Game
{
	public class UBoatPlayerCloseCommand : Command
	{
		[Inject]
		public bool isPlayerClose {get;set;}

		[Inject]
		public InventoryEventFireSignal inventoryEventFireSignal {get;set;}

		public override void Execute ()
		{
			inventoryEventFireSignal.Dispatch (isPlayerClose ? InventoryModel.Events.ApproachUBoat : InventoryModel.Events.AbandonUBoat, TouchScreenPosition.zero);
		}
	}
}