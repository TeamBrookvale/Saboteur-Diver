using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class MineView : View
	{
		internal GameLevelFailedSignal gameLevelFailedSignal = new GameLevelFailedSignal ();

		void OnCollisionEnter2D (Collision2D collision2D)
		{
			// remove collider so there is only one explosion
			collider2D.enabled = false;

			// do not do anything if not collided with the player
			if (collision2D.gameObject.tag != "Player") return; 

			// trigger the mine explosion
			GetComponent<Animator> ().SetTrigger ("MineExplosionTrigger");

			// play explosion sound
			GetComponent<AudioSource> ().Play ();

			// dispatch level failed signal
			gameLevelFailedSignal.Dispatch ();
		}
	}
}