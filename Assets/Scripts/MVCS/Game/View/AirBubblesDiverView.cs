using UnityEngine;
using System;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class AirBubblesDiverView : View
	{
		float nextBlowTime;
		bool isEmbarked; // is the player in the uboat

		Func<Vector2> playerModelGetCachedPosition;

		internal void init (Func<Vector2> playerModelGetCachedPosition)
		{
			OnPlayerUBoatEmbarkSignal (false);

			this.playerModelGetCachedPosition = playerModelGetCachedPosition;
		}

		void Update ()
		{
			// if not embarked then the bubbles should be always above the player
			if (!isEmbarked)
			{
				transform.position = 	(Vector3) playerModelGetCachedPosition () + new Vector3(0, .35f); 
				transform.eulerAngles = Vector3.zero;
			}


			if (nextBlowTime < Time.time)
			{
				// continuous blowing if embarked in the uboat
				nextBlowTime = Time.time + (isEmbarked ? 0 : UnityEngine.Random.Range(3f,10f));
				
				GetComponent<Animator>().SetTrigger("Blow");
				
				audio.Play();
			}
		}
		
		internal void OnPlayerUBoatEmbarkSignal (bool isEmbarked)
		{
			this.isEmbarked = isEmbarked;

			if (isEmbarked)
			{
				transform.localPosition = 		new Vector3 (-1,0);
				transform.localEulerAngles =	new Vector3 (0,0,90);
			}
		}
	}
}