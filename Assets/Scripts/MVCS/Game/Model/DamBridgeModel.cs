using UnityEngine;
using System.Collections.Generic;

namespace TeamBrookvale.Game
{
	public class DamBridgeModel : IDamBridgeModel
	{
		public TouchScreenPosition lastCollisionTouchScreenPosition {get;set;}
		public IList<Vector2> bombsMounted {get;set;}

		public DamBridgeModel ()
		{
			bombsMounted = new List<Vector2> ();
		}

		public bool areAllBombsMounted ()
		{
			return Const.numberOfTimeBombsToMount <= bombsMounted.Count;
		}

		public int numberOfTimeBombsMounted ()
		{
			return bombsMounted.Count;
		}
	}
}