using UnityEngine;

namespace TeamBrookvale.Game
{
	public interface IPlayerModel
	{
		Vector2 getCachedPosition();
		void	setCachedPosition(Vector2 p);
		Vector2 pathGoal {get;set;}
		bool	isEmbarkedUBoat {get;set;}
		bool 	isMountingTimeBomb {get;set;}
		int 	isHiddenUnderDockOrBridge {get;set;}
	}
}