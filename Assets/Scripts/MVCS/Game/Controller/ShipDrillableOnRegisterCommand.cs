using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.context.api;

namespace TeamBrookvale.Game
{
	public class ShipDrillableOnRegisterCommand : Command {

		// inject ship's instance ID sent by the mediator
		[Inject]
		public int shipInstanceID {get;set;}

		[Inject]
		public IShipDrillableModel shipDrillableModel {get;set;}

		public override void Execute ()
		{
			shipDrillableModel.drillableShipPropertiesDict.Add(shipInstanceID, new DrillableShipProperties());
		}
	}
}