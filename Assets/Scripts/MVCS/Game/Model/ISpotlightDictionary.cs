using System.Collections.Generic;

namespace TeamBrookvale.Game
{
	public interface ISpotlightDictionary
	{
		IDictionary<int,ISpotlightModel> spotlights {get;set;}
		void addInstance (int instanceID, ISpotlightModel m);
		bool anySpotlightsCurrentlyNoticedPlayer ();
		void updatePlayerNoticed (int instanceID, bool isPlayerNoticed); 
		int numberOfSpotlightsPlayerNoticed {get;set;}
	}
}