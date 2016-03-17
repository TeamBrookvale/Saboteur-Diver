using UnityEngine;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class CamaradeBoatView : View
	{
		internal PlayerSwimmedToCamaradeBoatSignal _playerSwimmedToCamaradeBoatSignal = 
			 new PlayerSwimmedToCamaradeBoatSignal();

		void OnCollisionEnter2D (Collision2D collision2D)
		{
			if (collision2D.gameObject.tag == "Player")
				_playerSwimmedToCamaradeBoatSignal.Dispatch ();

		}
	}
}