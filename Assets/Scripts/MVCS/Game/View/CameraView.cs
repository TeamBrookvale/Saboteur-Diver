using UnityEngine;
using System;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.Game
{
	// camera states : FollowingPlayer, PinchZoomOrPan, Menu

	public class CameraView : View
	{
		// is the camera following the player
		ICameraModel model;

		// player's cached position
		Func<Vector2> playerModelGetCachedPosition;

		// the level's bounds
		ILevelBoundsModel levelBounds;

		internal void init (ICameraModel model, Func<Vector2> playerModelGetCachedPosition, ILevelBoundsModel levelBounds)
		{
			// get delegates from the mediator
			this.model = model;
			this.playerModelGetCachedPosition = playerModelGetCachedPosition;
			this.levelBounds = levelBounds;

			// initialize camera
			camera.orthographicSize = 20;

			// zoom on player on start
			StartCoroutine (ZoomOnPlayerOnStart ());
		}

		IEnumerator ZoomOnPlayerOnStart ()
		{
			// wait half a second first
			yield return new WaitForSeconds (.5f);
			
			// ultimately the orthographicSize should be 5 but we target 4 to make sure 5 is reached
			while (camera.orthographicSize > 5)
			{
				camera.orthographicSize -= (camera.orthographicSize - 4) * .05f;
				yield return new WaitForFixedUpdate ();
			}
		}

		void FixedUpdate ()
		{
			// propose the values set by CameraPinchZoomPanCommand
			if (model.isNewProposedCameraOrthographicSize)
				camera.orthographicSize = model.getProposedCameraOrthographicSize ();

			if (model.isNewProposedCameraPosition)
				transform.position = model.getProposedCameraPosition ();

			// if following player then smooth transition of the camera to the player
			if (model.getIsFollowingPlayer())
				transform.position = transform.position + 
					(new Vector3(
							playerModelGetCachedPosition().x,
							playerModelGetCachedPosition().y,
							transform.position.z)
					 - transform.position) * .1f;

			// if zoomed out too much then zoom in really quickly
			camera.orthographicSize = Mathf.Min (
				camera.orthographicSize,
				(levelBounds.y_max - levelBounds.y_min) / 2,
				(levelBounds.x_max - levelBounds.x_min) / 2 * Screen.height / Screen.width); 

			// make sure the camera does not go beyond the level boundaries
			float cameraX = transform.position.x;
			float cameraY = transform.position.y;

			// calculate and apply coordinates with clamp
			cameraY = Mathf.Clamp (
				cameraY,
				levelBounds.y_min + camera.orthographicSize,
				levelBounds.y_max - camera.orthographicSize);

			cameraX = Mathf.Clamp (
				cameraX,
				levelBounds.x_min + camera.orthographicSize * Screen.width / Screen.height,
				levelBounds.x_max - camera.orthographicSize * Screen.width / Screen.height);

			transform.position = new Vector3 (
				cameraX,
				cameraY,
				transform.position.z);
		}
	}
}