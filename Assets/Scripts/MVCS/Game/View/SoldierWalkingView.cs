using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.Game
{
	public class SoldierWalkingView : View
	{
		float walkingDistance;					// distance to walk back and forth
		float walkingAngle;						// direction of walking in degrees
		Vector3 startPosition;					// this is where the soldier goes back to
		Vector3 walkingDirectionVector;			// calculated from the angle
		bool comingBack;						// false when going there, true when coming back
		bool slowDownRandom;					// slow down randomly to make the soldier look more human
		Animator animator;						// animation controller
		string lastAnimationTrigger;			// do not keep triggering the same animation trigger
		public float speed = 1;					// walking speed of the soldier
	
		internal void init ()
		{
			// convert rotation and scale_x loaded by XML to meaningful variables
			walkingAngle = transform.rotation.eulerAngles.z; // already in radians
			walkingDirectionVector = Quaternion.AngleAxis(walkingAngle, Vector3.forward) * Vector3.right;
			walkingDistance = transform.localScale.x;
			startPosition = transform.position;
		
			// reset the transform values that were only used to convey data from the XML
			transform.localScale = new Vector3 (1, 1, 1);
			transform.localRotation = Quaternion.identity;

			// cache the animation controller
			animator = GetComponent<Animator> ();

			// slow down randomly to make the soldier look more human
			StartCoroutine (SlowDownRandom ());
		}

		void Update ()
		{
			// turn around if too far
			if ((transform.position - startPosition).magnitude > walkingDistance)
				comingBack = true;

			// turn around if arrived back
			if (((transform.position - startPosition).normalized + walkingDirectionVector).magnitude < 1)
				comingBack = false;

			// slow down randomly
			if (!slowDownRandom)
				transform.position += walkingDirectionVector * speed * Time.deltaTime * (comingBack ? -1 : 1);

			// current walking angle in degrees
			float a = (walkingAngle + (comingBack ? 180 : 0)) % 360;

			// current calculated trigger
			string t = "";
			if ( 0 <= a) t = "WalkRightUp";
			if ( 90 < a) t = "WalkLeftUp";
			if (180 < a) t = "WalkLeftDown";
			if (270 < a) t = "WalkRightDown";
			if (slowDownRandom)	t = "Stop";

			// trigger the trigger if necessary
			if (t != lastAnimationTrigger)
			{
				animator.SetTrigger (t);
				lastAnimationTrigger = t;
			}
		}

		IEnumerator SlowDownRandom ()
		{
			while (true) 
			{
				yield return new WaitForSeconds (Random.Range (8f, 20f));
				slowDownRandom = true;
				yield return new WaitForSeconds (Random.Range (2f, 8f));
				slowDownRandom = false;
			}
		}
	}
}