using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.context.api;

namespace TeamBrookvale.UI
{
	public class UIStartCommand : Command
	{
		[Inject]
		public TeamBrookvale.Game.IGameModel gameModel {get;set;}

		[Inject]
		public TeamBrookvale.Game.IInventoryModel inventoryModel {get;set;}

		public override void Execute ()
		{
			// cache if smokebomb is enabled on this level because if no then hide the smokebomb icon
			inventoryModel.cachedIsSmokeBombEnabledOnThisLevel = gameModel.isSmokeBombEnabledOnThisLevel ();
		}
	}
}