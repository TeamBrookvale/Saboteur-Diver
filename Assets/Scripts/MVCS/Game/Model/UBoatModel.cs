using UnityEngine;

namespace TeamBrookvale.Game
{
	// injected as singleton

	public class UBoatModel : IUBoatModel
	{
		public bool isPlayerEmbarked {get;set;}
		public Vector2 cachedPosition {get;set;}
	}
}