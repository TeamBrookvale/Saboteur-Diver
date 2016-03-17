using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

namespace TeamBrookvale.Game
{
	public class ShipDrillableSunkCommand : Command {

		[Inject]
		public int instanceIdOfSunkShip {get;set;}

		[Inject]
		public Vector2 drillAirBubblePosition {get;set;}

		[Inject]
		public IShipDrillableModel shipDrillableModel {get;set;}

		[Inject]
		public AllShipsSunkOrBombsMountedSignal allShipsSunkOrBombsMountedSignal {get;set;}

		[Inject]
		public PlaySoundFxSignal playSoundFxSignal {get;set;}

		public override void Execute ()
		{
			shipDrillableModel.shipSunk(instanceIdOfSunkShip, drillAirBubblePosition);

			playSoundFxSignal.Dispatch (SoundFx.DrillSuccess, TouchScreenPosition.zero);

			if (shipDrillableModel.areAllShipsSunk())
				allShipsSunkOrBombsMountedSignal.Dispatch();
		}
	}
}