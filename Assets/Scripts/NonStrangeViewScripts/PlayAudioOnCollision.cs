using UnityEngine;
using System.Collections;

public class PlayAudioOnCollision : MonoBehaviour
{
	Vector2 lastCP;							// lastCP and currCP are collision points
	Transform lastOtherColliderTransform;	// this is generally the player's transform

	const float MinDistanceToPlaySoundAgain = .5f;		// the player has to go at least this far to play the sound again on collision

	bool playerWentFarFromLastCollision;

	void Start ()
	{
		resetLastCP ();
	}

	void Update ()
	{
		// if the player went far away from the contact point
		if (lastOtherColliderTransform != null)
			if (((Vector2) lastOtherColliderTransform.position - lastCP).magnitude > MinDistanceToPlaySoundAgain)
				playerWentFarFromLastCollision = true;
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		lastOtherColliderTransform = coll.collider.gameObject.transform;

		Vector2 currCP = coll.contacts[0].point;

		if ((currCP - lastCP).magnitude > MinDistanceToPlaySoundAgain || playerWentFarFromLastCollision)
		{
			playerWentFarFromLastCollision = false;
			GetComponent<AudioSource>().Play();
		}

		lastCP = currCP;
	}

	void resetLastCP ()
	{
		lastCP = new Vector2 (9999,9999);
	}
}
