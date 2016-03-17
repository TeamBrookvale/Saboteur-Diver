using TeamBrookvale;
using strange.extensions.command.impl;

namespace TeamBrookvale.UI
{
	public class UIPlayAreaPressCommand : Command
	{
		[Inject]
		public PlayerGotoSignal playerGotoSignal {get;set;}

		[Inject]
		public PlayerActionSignal playerActionSignal {get;set;}

		[Inject]
		public TouchScreenPosition touchScreenPosition {get;set;}

		[Inject]
		public ITouchModel touchModel {get;set;}

		[Inject]
		public TeamBrookvale.Game.IInventoryModel inventoryModel {get;set;}

		[Inject]
		public InventoryEventFireSignal inventoryEventFireSignal {get;set;}

		[Inject]
		public TeamBrookvale.Game.ISmokeBombCircleModel smokeBombCircleModel {get;set;}

		[Inject]
		public TeamBrookvale.CurrentInventoryIconSignal currentInventoryIconSignal {get;set;}

		public override void Execute ()
		{
			if (inventoryModel.getCurrentInvItem().id != TeamBrookvale.Game.InvItem.IDType.SmokeBombActive)
				playerGotoSignal.Dispatch (touchScreenPosition);

			// explode a smokebomb for a normal push too
			if (inventoryModel.getCurrentInvItem().id == TeamBrookvale.Game.InvItem.IDType.SmokeBombActive)
			{
				if (smokeBombCircleModel.isTouchWithinEllipse (touchScreenPosition))
				{
					// Drop smoke bomb
					playerActionSignal.Dispatch (touchScreenPosition, true);

					// Dispatch inventory event
					inventoryEventFireSignal.Dispatch (TeamBrookvale.Game.InventoryModel.Events.TouchWithinSmokeBombCircle, touchScreenPosition);
				}
				else
				{
					// Dispatch inventory event
					inventoryEventFireSignal.Dispatch (TeamBrookvale.Game.InventoryModel.Events.TouchOutOfSmokeBombCircle, touchScreenPosition);
				}
			}
		}
	}
}