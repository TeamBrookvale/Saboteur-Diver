using System.Collections.Generic;
using UnityEngine;

namespace TeamBrookvale.Game
{
	public interface IShipDrillableModel
	{
		IDictionary<int,DrillableShipProperties> drillableShipPropertiesDict {get;set;}
		DrillableShipProperties lastDrilledShipProperties {get;set;}
		IList<Vector2> drillAirBubblePositionList {get;set;}
		void shipSunk (int instanceId, Vector2 drillAirBubblePosition);
		bool areAllShipsSunk ();
		int numberOfShipsSunk ();
		int numberOfShips ();
	}

	public class DrillableShipProperties
	{
		public float health = 1;
		public bool isSunk = false;
		public bool isBeingDrilled = true; // set by the PlayerActionCommand
	}
}