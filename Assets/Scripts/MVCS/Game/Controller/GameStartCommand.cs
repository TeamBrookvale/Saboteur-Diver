using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using Soomla.Store;

namespace TeamBrookvale.Game
{
	public class GameStartCommand : Command {

		[Inject(ContextKeys.CONTEXT_VIEW)]
		public GameObject contextView {get;set;}

		[Inject]
		public IGameModel gameModel {get;set;}

		[Inject]
		public IShipDrillableModel shipDrillableModel {get;set;}

		[Inject]
		public IInventoryModel inventoryModel {get;set;}

		[Inject]
		public IRoutineRunner routineRunner {get;set;}

		[Inject]
		public IDeserializedLevelsLoaderModel deserializedLevelsLoaderModel {get;set;}

		public override void Execute ()
		{
			// Load level from XML
			deserializedLevelsLoaderModel.generateItems (gameModel.currentLevel);

			// populate trigger items in the game model and determine level type
			gameModel.postXMLItemsLoaded (deserializedLevelsLoaderModel.parentOfXmlItems);

			// Bootstrap UI Context later as it relies on the Game and Inventory Models
			Application.LoadLevelAdditive ("UIContextScene");

			// start or restart game as it's zero on level passed or failed
			Time.timeScale = 1;

			// populate the number of smoke bombs but delay a bit to make sure Soomla initialized
			routineRunner.StartCoroutine (PopulateNumberOfSmokeBombsDelayed ());
		}

		IEnumerator PopulateNumberOfSmokeBombsDelayed ()
		{
			// delay
			yield return new WaitForEndOfFrame ();
	
			// populate the number of smoke bombs
			inventoryModel.smokeBombsLeft = (StoreInventory.GetItemBalance ("more_smokebombs") == 0 ? 3 : 8);
		}
	}
}