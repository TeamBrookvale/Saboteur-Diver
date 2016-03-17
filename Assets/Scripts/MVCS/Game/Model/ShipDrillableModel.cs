using System.Collections.Generic;
using UnityEngine;

namespace TeamBrookvale.Game
{
	public class ShipDrillableModel : IShipDrillableModel
	{
		// int is the id of the ship's instance id in the view
		public IDictionary<int,DrillableShipProperties> drillableShipPropertiesDict {get;set;}
		public IList<Vector2> drillAirBubblePositionList {get;set;}
		public DrillableShipProperties lastDrilledShipProperties {get;set;}

		public ShipDrillableModel ()
		{
			drillableShipPropertiesDict = new Dictionary<int,DrillableShipProperties>();
			drillAirBubblePositionList = new List<Vector2>();
		}

		public void shipSunk (int instanceId, Vector2 drillAirBubblePosition)
		{
			drillableShipPropertiesDict[instanceId].isSunk = true;
			drillAirBubblePositionList.Add (drillAirBubblePosition);
		}

		public bool areAllShipsSunk ()
		{
			foreach (var d in drillableShipPropertiesDict)
				if (!d.Value.isSunk)
					return false;

			return true;
		}

		public int numberOfShipsSunk ()
		{
			int sunk = 0;
			foreach (var d in drillableShipPropertiesDict)
				if (!d.Value.isSunk)
					sunk++;

			return sunk;
		}

		public int numberOfShips ()
		{
			return drillableShipPropertiesDict.Count;
		}
	}
}