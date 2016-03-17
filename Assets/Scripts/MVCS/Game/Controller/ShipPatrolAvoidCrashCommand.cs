using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using TeamBrookvale;
using System.Collections.Generic;

namespace TeamBrookvale.Game
{
	public class ShipPatrolAvoidCrashCommand : Command {

		// IRoutineRunner allows use to run Coroutines without having to directly implement the MonoBehavior interface.
		[Inject]
		public IRoutineRunner routineRunner {get;set;}

		[Inject]
		public IShipPatrolModel shipPatrolModel {get;set;}

		void UpdateOnceASecond ()
		{
			foreach (KeyValuePair<int,ShipPatrolProperties> i in shipPatrolModel.shipPatrolPropertiesDict)
				foreach (KeyValuePair<int,ShipPatrolProperties> j in shipPatrolModel.shipPatrolPropertiesDict)
			{
				// do not compare the same ship against itself
				if (i.Key <= j.Key) continue;

				// do the line sections of the ShipPatrols cross each other
				bool lineSectionsCrossEachOther = TBUtil.LineSegmentsIntersect (
					i.Value.getLineSection (true),
					i.Value.getLineSection (false),
					j.Value.getLineSection (true),
					j.Value.getLineSection (false));

				// if the ships' line section do not cross each other then continue and the boat can speed up again
				if (!lineSectionsCrossEachOther)
				{
					i.Value.slowDownAvoidCrash = j.Value.slowDownAvoidCrash = false;
					continue;
				}

				// if one of them is already slowed down then continue
				if (i.Value.slowDownAvoidCrash || j.Value.slowDownAvoidCrash) continue;

				// decide randomly which ship should slow down
				int instanceId = (Random.Range(0,2) == 1 ? i.Key : j.Key);

				// set the bool variable in the model
				shipPatrolModel.shipPatrolPropertiesDict[instanceId].slowDownAvoidCrash = true;
			}
		}

		// these ones below ensure that UpdateOnceASecond is executed once a second

		public override void Execute ()
		{
			routineRunner.StartCoroutine (_UpdateOnceASecond());
		}
		
		IEnumerator _UpdateOnceASecond ()
		{
			while (true)
			{
				yield return new WaitForSeconds (1);
				UpdateOnceASecond ();
			}
		}
	}
}