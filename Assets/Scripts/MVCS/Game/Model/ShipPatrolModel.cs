using System.Collections.Generic;
using UnityEngine;

namespace TeamBrookvale.Game
{
	public class ShipPatrolModel : IShipPatrolModel
	{
		// int is the id of the ship's instance id in the view
		public IDictionary<int,ShipPatrolProperties> shipPatrolPropertiesDict {get;set;}

		public ShipPatrolModel ()
		{
			shipPatrolPropertiesDict = new Dictionary<int,ShipPatrolProperties>();
		}

		public ShipPatrolProperties registerShipPatrol (int instanceId)
		{
			ShipPatrolProperties p = new ShipPatrolProperties ();

			shipPatrolPropertiesDict.Add (instanceId, p);

			return p;
		}
	}

	public class ShipPatrolProperties
	{
		public Vector2 cachedPosition;
		public float currentSpeed, originalSpeed, targetSpeed;
		public bool isGoingRight;
		public bool slowDownAvoidCrash;
		public bool slowDownSmokeBomb;
		public bool slowDownRandom;	// randomly slow down sometimes

		Vector3 _cachedDirection;

		public Vector3 getDirection ()
		{
			if (_cachedDirection == Vector3.zero)
			{
				float angle = isGoingRight ? Const.ShipPatrolDirectionRightAngle : Const.ShipPatrolDirectionLeftAngle;

				float x = Mathf.Cos (Mathf.PI / 180 * angle);
				float y = Mathf.Sin (Mathf.PI / 180 * angle);

				_cachedDirection = new Vector3 (isGoingRight ? x : -x, -y).normalized;
			}

			return _cachedDirection;
		}

		// generates a line section that goes behind and beyond the ship, it is used to avoid crashing
		public Vector2 getLineSection (bool backOrFrontOfTheShip)
		{
			return cachedPosition + (backOrFrontOfTheShip ? -2 : 12) * (Vector2) this.getDirection();
		}
	}
}