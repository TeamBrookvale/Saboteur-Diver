using UnityEngine;
using System.Collections.Generic;

namespace TeamBrookvale.Game
{
	public interface IDamBridgeModel
	{
		TouchScreenPosition lastCollisionTouchScreenPosition {get;set;}
		IList<Vector2> bombsMounted {get;set;}
		bool areAllBombsMounted ();
		int numberOfTimeBombsMounted ();
	}
}