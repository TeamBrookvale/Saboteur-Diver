using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using Soomla.Store;

namespace TeamBrookvale.Game
{
	public class IAPInGameMenuCommand : Command
	{
		[Inject]
		public IGameModel gameModel {get;set;}

		// in case the signal comes from the in game menu
		[Inject]
		public TeamBrookvale.UI.InGameMenuModel.IAPButton iapButton {get;set;}

		[Inject]
		public IInventoryModel inventoryModel {get;set;}

		public override void Execute ()
		{
			string storeID = "";

			if (iapButton == TeamBrookvale.UI.InGameMenuModel.IAPButton.UnlockAllLevels)
				storeID = "unlock_all_levels";

			if (iapButton == TeamBrookvale.UI.InGameMenuModel.IAPButton.MoreSmokeBombs)
			{
				storeID = "more_smokebombs";

				// add more smokebombs just for this level even if the player has not paid for it
				inventoryModel.moreSmokeBombsForJustNow ();
			}

			StoreInventory.BuyItem (storeID);
		}
	}
}