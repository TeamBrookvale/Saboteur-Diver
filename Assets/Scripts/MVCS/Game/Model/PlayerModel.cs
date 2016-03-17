using UnityEngine;

namespace TeamBrookvale.Game
{
	public class PlayerModel : IPlayerModel
	{
		// cached player position updated by the player mediator
		private Vector2 cachedPlayerPosition;

		public bool isEmbarkedUBoat {get;set;}
		public bool isMountingTimeBomb {get;set;}
		public int isHiddenUnderDockOrBridge {get;set;} // OnTriggerEnter2D increases, OnTriggerExit2D decreases the value

		public Vector2 getCachedPosition()
		{
			return cachedPlayerPosition;
		}

		public void	setCachedPosition(Vector2 p)
		{
			cachedPlayerPosition = p;
		}

		// navigation path goal, where the user touched the screen
		public Vector2 pathGoal {get;set;}
	}
}