using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.signal.impl;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class StatusBarMessagesDispatcherView : View
	{
		internal StatusBarMessageSignal _statusBarMessageSignal = new StatusBarMessageSignal ();
		internal StatusBarMessageRemoveSignal _statusBarMessageRemoveSignal = new StatusBarMessageRemoveSignal ();

		Func<Vector2> playerModelGetCachedPosition;

		Transform parentOfXmlItems;

		List<KeyValuePair<GOType,Transform>> triggerItems;

		bool bridge, buoy, dam, dock, tutorial;

		internal void init (Func<Vector2> playerModelGetCachedPosition, List<KeyValuePair<GOType,Transform>> triggerItems )
		{
			// get the player's position
			this.playerModelGetCachedPosition = playerModelGetCachedPosition;
			this.triggerItems = triggerItems;

			StartCoroutine (OnXMLItemsAndUIContextLoadedSignalDelayed ());
		}

		IEnumerator OnXMLItemsAndUIContextLoadedSignalDelayed ()
		{
			// wait for UI context to settle down and for the first statusbar message to dispatch
			yield return new WaitForSeconds (2f);

			// genetate bool values
			bridge 		= triggerItemsContainsKey (GOType.Bridge);
			buoy 		= triggerItemsContainsKey (GOType.Buoy);
			dam 		= triggerItemsContainsKey (GOType.Dam);
			dock 		= triggerItemsContainsKey (GOType.Dock);
			tutorial 	= triggerItemsContainsKey (GOType.Tutorial);
			
			if (tutorial && buoy)
				_statusBarMessageSignal.Dispatch (Const.StatusBarLocateBuoy);
			else if (tutorial)
				_statusBarMessageSignal.Dispatch (Const.StatusBarLocateTutorial);
			else if (bridge)
				_statusBarMessageSignal.Dispatch (Const.StatusBarLocateBridge);
			else if (dam)
				_statusBarMessageSignal.Dispatch (Const.StatusBarLocateDam);
			else if (dock)
				_statusBarMessageSignal.Dispatch (Const.StatusBarLocateDock);
			else
				Debug.LogError ("StatusBarMessagesDispatcherView should never be in this state");

			StartCoroutine (RareUpdateCoroutine ());
		}

		IEnumerator RareUpdateCoroutine ()
		{
			while (true)
			{
				yield return new WaitForSeconds (UnityEngine.Random.Range (.9f, 1.1f));
				RareUpdate ();
			}
		}

		void RareUpdate ()
		{
			foreach (KeyValuePair <GOType, Transform> kvp in triggerItems)
			{
				//Debug.Log (kvp.Value.name + "  "+((Vector2) kvp.Value.position - playerModelGetCachedPosition ()).magnitude + "  " + (Vector2) kvp.Value.position + "  " + playerModelGetCachedPosition ());

				// if player is close to the current gameobject
				if (playerDist (kvp) < 2f)

					switch(kvp.Key)
					{
					case GOType.Buoy:			_statusBarMessageSignal.Dispatch (Const.StatusBarPlantBomb); break;
					case GOType.Dam:			_statusBarMessageSignal.Dispatch (Const.StatusBarPlantBomb); break;
					case GOType.ShipDrillable:	_statusBarMessageSignal.Dispatch (Const.StatusBarInfiltrateBoat); break;
					case GOType.UBoat:			_statusBarMessageSignal.Dispatch (Const.StatusBarUBoat); break;
					default: break;
					}


				// driver can be a bit further from the Bridge or Dock
				if (playerDist (kvp) < 6f)

					switch (kvp.Key)
					{
					case GOType.Bridge:			_statusBarMessageSignal.Dispatch (Const.StatusBarPlantBomb); break; 
					case GOType.Dock:			if (!bridge && !buoy && !dam)
													_statusBarMessageSignal.Dispatch (Const.StatusBarInfiltrateBoat); break;
					default: break;
					}


				// if far away from the uboat
				if (2.5f < playerDist (kvp) && kvp.Key == GOType.UBoat)
					_statusBarMessageRemoveSignal.Dispatch (Const.StatusBarUBoat);
			}

			//_statusBarMessageSignal.Dispatch (Const.StatusBarInfiltrateBoat);
			// dispatch status bar messages
//			if (e == InventoryModel.Events.ApproachBridgeOrDamWithoutLocalBomb) _statusBarMessageSignal.Dispatch (Const.StatusBarPlantBomb);
//			if (e == InventoryModel.Events.ApproachBridgeOrDamWithLocalBomb) 	_statusBarMessageSignal.Dispatch (Const.StatusBarPlantedBomb);
//			if (e == InventoryModel.Events.ApproachBridgeOrDamWithAllBombs) 	_statusBarMessageSignal.Dispatch (Const.StatusBarBackToGetawayBoat);
			


		}

		bool triggerItemsContainsKey (GOType g)
		{
			foreach (KeyValuePair <GOType, Transform> e in triggerItems)
				if (e.Key == g)
					return true;

			return false;
		}

		float playerDist (KeyValuePair <GOType, Transform> kvp)
		{
			return ((Vector2) kvp.Value.position - playerModelGetCachedPosition ()).magnitude;
		}
	}
}