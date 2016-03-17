using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class ShipPatrolView : View
	{
		public float? speed;		// populated by XML
		ShipPatrolProperties m;		// cache the model
		int counter_spotLightSunkShipNoticedSignalDirectCall;
		Animator wakeAnimator;			// wake animator
		AudioSource audioSource;	// Audio Source

		public void init (ShipPatrolProperties m)
		{
			// cache the model as m
			this.m = m;

			// set model's speed based on data sourced from XML
			m.currentSpeed = m.targetSpeed = m.originalSpeed = (float) speed;

			// we use the model's speed variables going forward
			speed = null;

			// decide if ship's going right or left depending on its name
			if (gameObject.name.Contains ("Right"))
				m.isGoingRight = true;

			// wakeAnimator
			foreach (Animator a in GetComponentsInChildren<Animator> ())
				if (a.name[0] == 'W')
					wakeAnimator = a;

			// audio source cache
			audioSource = GetComponent<AudioSource> ();

			// slow down randomly to feel more human
			StartCoroutine (RandomSlowDown ());

			// play audio delayed
			StartCoroutine (PlayAudioDelayed ());
		}

		IEnumerator PlayAudioDelayed ()
		{
			yield return new WaitForSeconds (2);
			GetComponent <AudioSource> ().Play ();
		}

		void Update ()
		{
			// current speed is calculated based on the target speed
			m.currentSpeed = Mathf.MoveTowards (m.currentSpeed, m.targetSpeed, Time.deltaTime * .3f);

			// move ship left or right depending on its name
			transform.position += m.getDirection() * m.currentSpeed * Time.deltaTime;

			// if out of screen put it back
			ifOutOfScreenPutBack (transform, m.getDirection());

			// cache position in the model
			m.cachedPosition = transform.position;

			// set target speed that may be overriden in the below lines
			m.targetSpeed = m.originalSpeed;

			// slow down if there is a random slowdown event or smoke bomb seen
			if (m.slowDownRandom || m.slowDownSmokeBomb)
				m.targetSpeed = Const.ShipPatrolSlowSpeed;

			// slow down to avoid crashing another boat
			if (m.slowDownAvoidCrash)
				m.targetSpeed = 0;

			// wake animator update if not zeppelin
			if (wakeAnimator != null)
				wakeAnimator.SetFloat ("Speed", m.currentSpeed / m.originalSpeed);
		}

		void FixedUpdate ()
		{
			// audio pitch
			audioSource.pitch = Mathf.MoveTowards (audioSource.pitch, (m.targetSpeed == m.originalSpeed ? 1.2f : .85f), .01f);
		}

		// directly called by the spotlight by finding parent ship
		public void spotLightSunkShipNoticedSignalDirectCall ()
		{
			StartCoroutine (_spotLightSunkShipNoticedSignalDirectCall());
		}

		IEnumerator _spotLightSunkShipNoticedSignalDirectCall ()
		{
			// slowdown and count number of current active slowdown signals 
			m.slowDownSmokeBomb = true;
			counter_spotLightSunkShipNoticedSignalDirectCall++;

			// wait
			yield return new WaitForSeconds (Const.ShipPatrolAlertSlowLength);

			// only disable slowdown if this method has not been called again during waiting
			if (--counter_spotLightSunkShipNoticedSignalDirectCall == 0)
				m.slowDownSmokeBomb = false;
		}

		IEnumerator RandomSlowDown ()
		{
			while (true) 
			{
				yield return new WaitForSeconds (Random.Range (8f, 20f));
				m.slowDownRandom = true;
				yield return new WaitForSeconds (Random.Range (2f, 8f));
				m.slowDownRandom = false;
			}
		}

		
		void ifOutOfScreenPutBack (Transform transform, Vector2 direction)
		{
			if (transform.position.y < -Const.ShipPatrolPutBackOnceFurtherThan)
				transform.position -= (Vector3) direction * 2 * Const.ShipPatrolPutBackOnceFurtherThan; 
			
			if (transform.position.x < -Const.ShipPatrolPutBackOnceFurtherThan && direction.x < 0 ||
			    transform.position.x >  Const.ShipPatrolPutBackOnceFurtherThan && direction.x > 0)
				transform.position -= (Vector3) direction * 2 * Const.ShipPatrolPutBackOnceFurtherThan; 
		}
	}
}