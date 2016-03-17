using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.context.api;

namespace TeamBrookvale.Game
{
	public class InventoryEventFireCommand : Command {

		[Inject]
		public IInventoryModel model {get;set;}

		[Inject]
		public InventoryModel.Events e {get;set;}

		[Inject]
		public TouchScreenPosition t {get;set;}

		[Inject]
		public CurrentInventoryIconSignal currentInventoryIconSignal {get;set;}

		public override void Execute ()
		{
			// Fire event in the model
			InvItem.IDType currentStateId = model.fire (e, t);

			// Dispatch the new icon
			currentInventoryIconSignal.Dispatch (currentStateId);
		}
	}
}