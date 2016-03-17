using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using strange.extensions.pool.api;

namespace TeamBrookvale.Game
{
	public class PlayerActionCommand : Command
	{
		[Inject]
		public TouchScreenPosition touchScreenPosition {get;set;}

		[Inject]
		public bool startOrStop {get;set;}

		[Inject]
		public IInventoryModel inventoryModel {get;set;}

		[Inject]
		public ISmokeBombCircleModel smokeBombCircleModel {get;set;}

		[Inject]
		public PlayerUBoatEmbarkSignal playerUBoatEmbarkSignal {get;set;}

		[Inject]
		public IPlayerModel playerModel {get;set;}

		[Inject]
		public SmokeBombExplodeSignal smokeBombExplodeSignal {get;set;}

		[Inject]
		public SpawnPrefabSignal spawnPrefabSignal {get;set;}

		[Inject]
		public IDamBridgeModel damBridgeModel {get;set;}

		[Inject]
		public InventoryEventFireSignal inventoryEventFireSignal {get;set;}

		[Inject]
		public DrillingSignal drillingSignal {get;set;}

		[Inject]
		public TimeBombMountingSignal timeBombMountingSignal {get;set;}

		[Inject]
		public IGameModel gameModel {get;set;}

		public override void Execute()
		{
			// only do anything if the game is running
			if (gameModel.pauseStatus != PauseStatus.RUN) return;

			// cache the current inventory item
			InvItem currentInvItem = inventoryModel.getCurrentInvItem();

			// if the current invetory item is smoke bomb and the action is at the start phase then spawn one
			if (currentInvItem.id == InvItem.IDType.SmokeBombActive &&
			    startOrStop &&
				smokeBombCircleModel.isTouchWithinEllipse (touchScreenPosition) &&
				TBUtil.IsTouchOnWater (touchScreenPosition) &&
			    inventoryModel.smokeBombsLeft > 0)
					smokeBombExplodeSignal.Dispatch (touchScreenPosition);

			// embark or disembark submarine
			if (currentInvItem.id == InvItem.IDType.UBoat && startOrStop)
				playerUBoatEmbarkSignal.Dispatch (!playerModel.isEmbarkedUBoat);

			// mount bomb
			if (currentInvItem.id == InvItem.IDType.TimeBombActive)
				timeBombMountingSignal.Dispatch (startOrStop);

			// drill
			if (currentInvItem.id == InvItem.IDType.DrillActive)
				drillingSignal.Dispatch (startOrStop);
		}
	}
}