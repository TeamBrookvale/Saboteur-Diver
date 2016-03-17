using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.Game
{
	public class DamBridgeView : View
	{
		internal InventoryEventFireSignal _inventoryEventFireSignal = new InventoryEventFireSignal ();

		IDamBridgeModel damBridgeModel;
		IPlayerModel playerModel;

		internal void init (IDamBridgeModel damModel, IPlayerModel playerModel)
		{
			this.damBridgeModel = damModel;
			this.playerModel = playerModel;
		}

		void OnCollisionEnter2D (Collision2D collision2D)
		{
			// Assume there are no bombs mounted nearby
			InventoryModel.Events e = InventoryModel.Events.ApproachBridgeOrDamWithoutLocalBomb;

			// check if any bombs mounted nearby
			foreach (Vector2 bombPos in damBridgeModel.bombsMounted)
				if ((lastCollisionPoint (collision2D) - bombPos).magnitude < Const.minimumTimeBombDistance)
					e = InventoryModel.Events.ApproachBridgeOrDamWithLocalBomb;

			// check if we have any bombs left to mount
			if (damBridgeModel.bombsMounted.Count >= Const.numberOfTimeBombsToMount)
				e = InventoryModel.Events.ApproachBridgeOrDamWithAllBombs;

			// handle the collision
			handleCollision (e, collision2D);
		}

		void OnCollisionStay2D (Collision2D collision2D)
		{
			OnCollisionEnter2D (collision2D);
		}

		void OnCollisionExit2D (Collision2D collision2D)
		{
			if (!playerModel.isMountingTimeBomb)
				handleCollision (InventoryModel.Events.AbandonShipBridgeOrDam, collision2D);
		}

		void handleCollision (InventoryModel.Events e, Collision2D collision2D)
		{
			damBridgeModel.lastCollisionTouchScreenPosition = new TouchScreenPosition ((Vector2) collision2D.contacts[0].point);
		
			_inventoryEventFireSignal.Dispatch (e, TouchScreenPosition.zero);
		}

		Vector2 lastCollisionPoint (Collision2D collision2D)
		{
			return (Vector2) collision2D.contacts[0].point;
		}
			

	}
}