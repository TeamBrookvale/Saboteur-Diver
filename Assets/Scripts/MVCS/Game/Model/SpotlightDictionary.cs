using UnityEngine;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class SpotlightDictionary : ISpotlightDictionary
	{

		// id of the spotlight's view instance ID | the diver was spotted at this time by the spotlight
		public IDictionary<int,ISpotlightModel> spotlights {get;set;}

		public int numberOfSpotlightsPlayerNoticed {get;set;}

		public SpotlightDictionary ()
		{
			spotlights = new Dictionary<int,ISpotlightModel>();
		}

		public void addInstance (int instanceID, ISpotlightModel m)
		{
			spotlights.Add (instanceID, m);
		}

		public bool anySpotlightsCurrentlyNoticedPlayer ()
		{
			// return true if any of the spotlights has the player currently noticed
			foreach (var s in spotlights)
				if (s.Value.isPlayerNoticed)
					return true;

			return false;
		}

		public void updatePlayerNoticed (int instanceID, bool isPlayerNoticed)
		{
			spotlights[instanceID].isPlayerNoticed = isPlayerNoticed;
		}
	}
}