using System;
using System.Collections;
using UnityEngine;
using strange.extensions.mediation.impl;


namespace TeamBrookvale.Game
{
	public class SmokeBombCircleView : View
	{
		Func<Vector2> playerModelGetCachedPosition;

		public void init (Func<Vector2> playerModelGetCachedPosition)
		{
			// do not show circle on start
			showCircle (false);

			this.playerModelGetCachedPosition = playerModelGetCachedPosition;
		}

		public void OnCurrentInventoryIconSignal (InvItem.IDType id)
		{
			// show circle if current item is smokebomb
			showCircle (id == InvItem.IDType.SmokeBombActive);
		}

		void Update ()
		{
			transform.position = playerModelGetCachedPosition ();
		}

		void showCircle (bool b)
		{
			GetComponent<SpriteRenderer>().enabled = b;
		}
	}
}