using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.Game
{
	public class PlayerMediator : Mediator
	{
		[Inject]
		public IPlayerModel playerModel {get;set;}

		[Inject]
		public PlayerView view {get;set;}

		[Inject]
		public PlayerGotoSignal playerGotoSignal {get;set;}

		[Inject]
		public PlayerActionSignal playerActionSignal {get;set;}

		[Inject]
		public IInventoryModel inventoryModel {get;set;}

		[Inject]
		public PlayerUBoatEmbarkSignal playerUBoatEmbarkSignal {get;set;}

		[Inject]
		public IShipPatrolModel shipPatrolModel {get;set;}

		[Inject]
		public ShipPatrolDrilledSignal shipPatrolDrilledSignal {get;set;}

		[Inject]
		public InventoryEventFireSignal inventoryEventFireSignal {get;set;}

		[Inject]
		public GameLevelFailedSignal gameLevelFailedSignal {get;set;}

		[Inject]
		public TimeBombMountingSignal timeBombMountingSignal {get;set;}

		public override void OnRegister ()
		{
			view.init (playerModel, shipPatrolModel);

			// if ship drilled signal received from the view then dispatch ShipDrilledSignal
			view._shipPatrolDrilledSignal.AddListener (shipPatrolDrilledSignal.Dispatch);
			view._inventoryEventFireSignal.AddListener (inventoryEventFireSignal.Dispatch);

			// input from user
			playerGotoSignal.AddListener(OnPlayerGotoSignal);
			playerActionSignal.AddListener(OnPlayerActionSignal);
			playerUBoatEmbarkSignal.AddListener (OnPlayerUBoatEmbarkSignal);

			// end of game
			gameLevelFailedSignal.AddListener (view.OnGameLevelFailedSignal);

			// bomb mounting
			timeBombMountingSignal.AddListener (view.OnTimeBombMountingSignal);

			// do not go anywhere yet
			StartCoroutine (DoNotGoAnywhereYetDelayed ());

		}

		IEnumerator DoNotGoAnywhereYetDelayed ()
		{
			yield return new WaitForSeconds (.1f);
			playerModel.pathGoal = transform.position;
		}

		public override void OnRemove ()
		{
			// unsubscribe from the view
			view._shipPatrolDrilledSignal.RemoveListener (shipPatrolDrilledSignal.Dispatch);
			view._inventoryEventFireSignal.RemoveListener (inventoryEventFireSignal.Dispatch);

			playerGotoSignal.RemoveListener (OnPlayerGotoSignal);
			playerActionSignal.RemoveListener(OnPlayerActionSignal);
			playerUBoatEmbarkSignal.RemoveListener (OnPlayerUBoatEmbarkSignal);

			gameLevelFailedSignal.RemoveListener (view.OnGameLevelFailedSignal);
			timeBombMountingSignal.AddListener (view.OnTimeBombMountingSignal);

		}

		void OnPlayerGotoSignal (TouchScreenPosition touchScreenPosition)
		{
			playerModel.pathGoal = touchScreenPosition.world;
		}

		void OnPlayerActionSignal (TouchScreenPosition notUsedHere, bool startOrStop)
		{
			// if current inventory item is drill then start or stop drilling
			if (inventoryModel.getCurrentInvItem().id == InvItem.IDType.DrillActive)
				view.isDrillingGameInputEventReceived = startOrStop;

			// stop drilling if this is false even if not the drill is active
			if (!startOrStop)
				view.isDrillingGameInputEventReceived = false;
		}

		void OnPlayerUBoatEmbarkSignal (bool isEmbarked)
		{
			view.spriteEnabled (!isEmbarked);
		}
	}
}