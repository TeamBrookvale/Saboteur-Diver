using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TeamBrookvale.Game
{
	public class InventoryModel : IInventoryModel
	{
		IList<InvItem> invItems = new List<InvItem>();
		InvItem.IDType currentStateId;
		bool isButtonPushed;
		string lastGetCurrentInvItemText;

		public int smokeBombsLeft {get;set;}
		public bool cachedIsSmokeBombEnabledOnThisLevel {get;set;}

		// States are InvItem.IDType
		public enum Events {
			ButtonPush,
			TouchWithinSmokeBombCircle,
			TouchOutOfSmokeBombCircle,
			AbandonShipBridgeOrDam,
			ApproachDrillableShip,
			ApproachSunkShip,
			ApproachBridgeOrDamWithoutLocalBomb,
			ApproachBridgeOrDamWithLocalBomb,
			ApproachBridgeOrDamWithAllBombs,
			ApproachUBoat,
			AbandonUBoat,
			MoreSmokeBombs
		}

		public InventoryModel ()
		{
			// fill up the list with the contents of the InvItem.IDType enum
			foreach (InvItem.IDType idType in (InvItem.IDType[]) Enum.GetValues(typeof(InvItem.IDType)))
			{
				// add item to list and enable
				invItems.Add (new InvItem (idType));
			}
		}
	
		// SHOULD NOT BE CALLED DIRECTLY, only through InventoryEventFireSignal
		public InvItem.IDType fire (Events e, TouchScreenPosition t)
		{
			// InvItem.IDType prevStateId = currentStateId;

			// any state transitions except in uboat

			if (currentStateId != InvItem.IDType.UBoat)
				switch (e)
				{
				case Events.ApproachUBoat :
					currentStateId = InvItem.IDType.UBoat;
					break;

				case Events.ApproachDrillableShip :
					currentStateId = InvItem.IDType.DrillActive;
					break;

				case Events.ApproachSunkShip :
					currentStateId = InvItem.IDType.DrillInactive;
					break;
					
				case Events.ApproachBridgeOrDamWithoutLocalBomb :
					currentStateId = InvItem.IDType.TimeBombActive;
					break;
				
				case Events.ApproachBridgeOrDamWithLocalBomb :
					currentStateId = InvItem.IDType.TimeBombInactive;
					break;

				case Events.ApproachBridgeOrDamWithAllBombs :
					currentStateId = InvItem.IDType.TimeBombInactiveTick;
					break;

				case Events.AbandonShipBridgeOrDam :
					currentStateId = InvItem.IDType.SmokeBombInactive;
					break;

				default :
					break;
				}

				// state dependent transitions
				switch (currentStateId)
				{
				case InvItem.IDType.SmokeBombInactive :
					if ((e == Events.ButtonPush || e == Events.MoreSmokeBombs) && smokeBombsLeft > 0 && cachedIsSmokeBombEnabledOnThisLevel)
						currentStateId = InvItem.IDType.SmokeBombActive;
					break;

				// Smoke bomb explosion is handled by PlayerActionCommand
				case InvItem.IDType.SmokeBombActive :
					if (e == Events.ButtonPush || e == Events.TouchOutOfSmokeBombCircle || e == Events.TouchWithinSmokeBombCircle)
						currentStateId = InvItem.IDType.SmokeBombInactive;
					break;

				case InvItem.IDType.UBoat :
					if (e == Events.AbandonUBoat)
						currentStateId = InvItem.IDType.SmokeBombInactive;
					break;

				default :
					break;
				}

			// Debug.Log (prevStateId + " ("+e+") -> " + currentStateId);

			return currentStateId;
		}

		public InvItem getCurrentInvItem ()
		{
			InvItem currentInvItem	= invItems.Where (elem => elem.id == currentStateId).First ();

			// if smokebomb is enabled then just return currentInvItem
			if (cachedIsSmokeBombEnabledOnThisLevel)
				return currentInvItem;

			// if smokebomb is not enabled on this level then the default invitem is the inactive drill
			else
				return	currentInvItem.id == InvItem.IDType.SmokeBombInactive ?
						invItems.Where (elem => elem.id == InvItem.IDType.DrillInactive).First () :
						currentInvItem;
		}

		public string getCurrentInvItemText ()
		{
			if (currentStateId == InvItem.IDType.SmokeBombActive || 
			    currentStateId == InvItem.IDType.SmokeBombInactive)
					return smokeBombsLeft.ToString();

			return "";
		}

		public void moreSmokeBombsForJustNow ()
		{
			// increase the smokebombs in this level play
			smokeBombsLeft = 8;

			fire (Events.MoreSmokeBombs, TouchScreenPosition.zero);
		}
	}
}