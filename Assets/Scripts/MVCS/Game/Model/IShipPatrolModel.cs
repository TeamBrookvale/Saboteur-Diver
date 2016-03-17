using System.Collections.Generic;
using UnityEngine;

namespace TeamBrookvale.Game
{
	public interface IShipPatrolModel
	{
		IDictionary<int,ShipPatrolProperties> shipPatrolPropertiesDict {get;set;}
		ShipPatrolProperties registerShipPatrol (int instanceId);
	}
}