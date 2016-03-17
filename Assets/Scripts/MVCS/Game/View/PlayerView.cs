using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class PlayerView : View
	{

		Vector2 lastCollisionContactPoint;
		ShipDrillableView lastCollisionShip;	// null if last collision did not occur with a ship
		float lastDiverShipDamOrBridgeAngle; 				// vector from the player to the ship at the time of the collison
		Animator animator;						// cache the animator
		string _lastAnimationTrigger; 			// do not invoke trigger again if this was the last trigger
		bool _isDrilling; 						// is the player drilling a boat, not drilling at start
		bool _isGameLevelFailedSignalTriggered;
		bool _isSwimForceApplied;
		internal bool isDrillingGameInputEventReceived; // set by mediator

		// signal to mediator
		internal ShipPatrolDrilledSignal _shipPatrolDrilledSignal = new ShipPatrolDrilledSignal ();
		internal InventoryEventFireSignal _inventoryEventFireSignal = new InventoryEventFireSignal ();

		// store last dispatched signal so it does not get dispatched all the time
		InventoryModel.Events lastDispatchedEvent = InventoryModel.Events.AbandonShipBridgeOrDam;

		// Models provided by the mediator in the init method
		IPlayerModel playerModel;
		IShipPatrolModel shipPatrolModel;

		// last non-stuck position in case the player is stuck somewhere
		// lastMemorizedPosition becomes lastNonStuckPosition if the player moves enough to justify he is not stuck
		Vector2 _sLastMemorizedPosition, _sLastNonStuckPosition;
		bool _sIsSwimForceAppliedSecondAgo;
		const float IsStuckMinMoveDistance = .05f; // if the player does not move at least this much in a second then considered stuck

		internal void init (IPlayerModel playerModel, IShipPatrolModel shipPatrolModel)
		{
			this.playerModel = playerModel;
			this.shipPatrolModel = shipPatrolModel;

			// initialize to a value that is enough far away
			lastCollisionContactPoint = new Vector2(999999,999999);

			// cache animator
			animator = GetComponent<Animator>();

			// update once a second
			InvokeRepeating("UpdatePerSecond", 1, 1);
		}

		// swimforce independent of frame rate
		void FixedUpdate ()
		{
			// update player position in the model so other commands and mediators can use it
			playerModel.setCachedPosition ((Vector2) transform.position);

			// calculate and apply swim force
			applySwimForce ();
		}

		void Update ()
		{
			// if ship is close and space hit start drilling
			if(isDrillingGameInputEventReceived && isCloseToBoat())
			{
				// do not move during drilling
				rigidbody2D.isKinematic = true;

				// look at the ship
				lookTowards2D (lastDiverShipDamOrBridgeAngle);

				// drill the ship and request if it's still drillable i.e. not sink
				_isDrilling = lastCollisionShip.drill (lastCollisionContactPoint);

				// trigger the animator
				setAnimationTrigger (getTriggerBasedOnAngle (lastDiverShipDamOrBridgeAngle));
			}

			// if attempting to drill a patrol boat or something else
			else if (isDrillingGameInputEventReceived && !isCloseToBoat())
			{
				foreach (ShipPatrolProperties s in shipPatrolModel.shipPatrolPropertiesDict.Values)
					if ((s.cachedPosition - (Vector2) transform.position).magnitude < .5f)
					{
						_shipPatrolDrilledSignal.Dispatch (s);
						_isDrilling = true;
					}
			}

			// if mounting a time bomb
			else if (playerModel.isMountingTimeBomb)
			{
				// do not move during drilling
				rigidbody2D.isKinematic = true;

				// look at the dam or bridge
				lookTowards2D (lastDiverShipDamOrBridgeAngle);

				// trigger the animator
				if (lastDiverShipDamOrBridgeAngle < 90 || 270 < lastDiverShipDamOrBridgeAngle)
					setAnimationTrigger ("BombMountingRightDown");
				else
					setAnimationTrigger ("BombMountingLeftDown");
			}
			
			// if finished drilling or in cruising mode
			else
			{
				// start move away from ship
				rigidbody2D.isKinematic = false;

				// stop playing drill sound as ship does not know when drilling is finished
				if (_isDrilling)
					_isDrilling = false;
			}

			// if boat became far then icon should change
			if (!isCloseToBoat() && lastDispatchedEvent != InventoryModel.Events.AbandonShipBridgeOrDam)
			{
				lastDispatchedEvent = InventoryModel.Events.AbandonShipBridgeOrDam;
				_inventoryEventFireSignal.Dispatch (lastDispatchedEvent, TouchScreenPosition.zero);
			}

			// Rotate diver into velocity direction
			if (rigidbody2D.velocity.magnitude > .08)
			{
				Vector2 lookDirection = (Vector2) (Quaternion.Euler(0, 0, Const.PlayerRotationTrim) * (Vector3) rigidbody2D.velocity);
				lookTowards2D (lookDirection);
			}
		}
		
		void UpdatePerSecond ()
		{
			// if stuck then put a bit back
			if (_sIsSwimForceAppliedSecondAgo
			    && _isSwimForceApplied
			    && ((playerModel.getCachedPosition() - _sLastMemorizedPosition).magnitude < IsStuckMinMoveDistance)
			    && !playerModel.isMountingTimeBomb
			    && !_isDrilling)
			{
				transform.position = playerModel.pathGoal = _sLastMemorizedPosition = 
					playerModel.getCachedPosition() + (_sLastNonStuckPosition - playerModel.getCachedPosition()) * .1f;
			}

			// if not stuck then update the variables
			else
			{
				_sLastNonStuckPosition = _sLastMemorizedPosition;
				_sLastMemorizedPosition = transform.position;
			}
			_sIsSwimForceAppliedSecondAgo = _isSwimForceApplied;
		}

		void applySwimForce ()
		{
			// if currently drilling then do not apply force and do not trigger animations
			if (_isDrilling || playerModel.isMountingTimeBomb || _isGameLevelFailedSignalTriggered) return;
			
			// calculate swimforce
			Vector2 swimForce = Vector2.zero;

			// if not at the destination then apply force
			if ((playerModel.pathGoal - playerModel.getCachedPosition()).magnitude > .5f)
				swimForce = (playerModel.pathGoal - playerModel.getCachedPosition()).normalized * Const.PlayerSwimSpeed;

			// apply force
			rigidbody2D.AddForce(swimForce);

			// trigger the animator whether going left or right
			if (swimForce.magnitude == 0)
			{
				setAnimationTrigger("Cruise");
				_isSwimForceApplied = false;
			}
			else
			{
				if (rigidbody2D.velocity.normalized.x > 0 && rigidbody2D.velocity.normalized.y > 0) setAnimationTrigger ("SwimRightUp");
				if (rigidbody2D.velocity.normalized.x <=0 && rigidbody2D.velocity.normalized.y > 0) setAnimationTrigger ("SwimLeftUp");
				if (rigidbody2D.velocity.normalized.x <=0 && rigidbody2D.velocity.normalized.y <=0) setAnimationTrigger ("SwimLeftDown");
				if (rigidbody2D.velocity.normalized.x > 0 && rigidbody2D.velocity.normalized.y <=0) setAnimationTrigger ("SwimRightDown");
				_isSwimForceApplied = true;
			}
		}

		public void spriteEnabled (bool isSpriteEnabled)
		{
			GetComponent<SpriteRenderer>().enabled = isSpriteEnabled; 
		}

		void OnCollisionEnter2D (Collision2D collision)
		{
			// get last collision point with the boat
			lastCollisionContactPoint = ((ContactPoint2D[]) collision.contacts)[0].point;

			// get last collision's transform
			Transform lastCollisionTransform = ((ContactPoint2D[]) collision.contacts)[0].collider.transform;

			// get Ship script if collided with a boat, null otherwise
			lastCollisionShip = lastCollisionTransform.GetComponent<ShipDrillableView>();

			// check if collided to a dam or bridge
			bool isCollisionWithBridgeOrDam = lastCollisionTransform.GetComponent<DamBridgeView>() != null;

			// change icon to drill and calculate lastRoundedDiverShipAngle
			if (lastCollisionShip != null)
			{
				// if the ship is not sunk yet
				if (!lastCollisionShip.isSunk && lastDispatchedEvent != InventoryModel.Events.ApproachDrillableShip)
				{
					lastDispatchedEvent = InventoryModel.Events.ApproachDrillableShip;
					_inventoryEventFireSignal.Dispatch (lastDispatchedEvent, TouchScreenPosition.zero);
				}

				// if the ship is sunk
				if (lastCollisionShip.isSunk && lastDispatchedEvent != InventoryModel.Events.ApproachSunkShip)
				{
					lastDispatchedEvent = InventoryModel.Events.ApproachSunkShip;
					_inventoryEventFireSignal.Dispatch (lastDispatchedEvent, TouchScreenPosition.zero);
				}
			}

			if (lastCollisionShip != null || isCollisionWithBridgeOrDam)
				// angle of the vector pointing from the diver to the collision point
				lastDiverShipDamOrBridgeAngle = 
					180 + Mathf.Rad2Deg * Mathf.Atan2 (
						collision.contacts[0].normal.y,
						collision.contacts[0].normal.x);
			
			// round the value to 45, 135, etc
			//lastRoundedDiverShipAngle = (int) diverShipAngle / 90 % 4 * 90 + 45;
			//Debug.Log (diverShipAngle + "  " + lastRoundedDiverShipAngle);
		}

		// select the trigger based on "a" which is the angle of the vector pointing from the diver to the last collision point
		string getTriggerBasedOnAngle (float a)
		{
			// return value
			string trigger = "";

			// define possible triggers
			if (a <  90) return "DrillRightUp";
			if (a < 180) return "DrillLeftUp";
			if (a < 270) return "DrillLeftDown";
			if (a < 360) return "DrillRightDown";

			return trigger;
		}

		void lookTowards2D (Vector2 dir)
		{
			// calculate direction's angle
			lookTowards2D (Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg);
		}
		
		void lookTowards2D (float angle)
		{
			// smooth LERP rotation
			angle = Mathf.LerpAngle(transform.eulerAngles.z, angle, 0.1f);
			
			// apply angle in 2D
			transform.eulerAngles = new Vector3 (0, 0, angle);
		}

		bool isCloseToBoat ()
		{
			return ((Vector2) transform.position - lastCollisionContactPoint).magnitude < .4f && lastCollisionShip != null;
		}

		void setAnimationTrigger (string trigger)
		{
			if (_lastAnimationTrigger == trigger) return;

			animator.SetTrigger(trigger);
			_lastAnimationTrigger = trigger;
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			playerModel.isHiddenUnderDockOrBridge++;
		}

		void OnTriggerExit2D (Collider2D other)
		{
			playerModel.isHiddenUnderDockOrBridge--;

			if (playerModel.isHiddenUnderDockOrBridge < 0)
				Debug.LogError ("Hidden under dock or bridge count reached -1. Maybe the player started already hidden under something?");
		}

		// Called by mediator. Player should return to idle especially on mine explosion.
		internal void OnGameLevelFailedSignal ()
		{
			_isGameLevelFailedSignalTriggered = true;
			setAnimationTrigger ("Cruise");
		}

		internal void OnTimeBombMountingSignal (bool startOrStop)
		{
			playerModel.isMountingTimeBomb = startOrStop;
		}
	}
}