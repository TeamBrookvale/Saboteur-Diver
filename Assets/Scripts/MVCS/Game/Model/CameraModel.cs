using UnityEngine;

namespace TeamBrookvale.Game
{
	public class CameraModel : ICameraModel
	{
		// this is the only camera in the scene
		public Camera camera {get;set;}

		// set by the CameraPinchZoomPanCommand
		float _proposedCameraOrthographicSize;
		Vector2 _proposedCameraPosition;

		// if new values proposed but the CameraPinchZoomPanCommand then set this true
		public bool isNewProposedCameraOrthographicSize {get; set;}
		public bool isNewProposedCameraPosition {get; set;}

		// true if the camera is following the player, false if the user is panning
		bool _isFollowingPlayer = true;

		// Query invoked by gameobjects
		// if an object is not within Camera Bounds then it's probably safe to turn it off
		public bool isWithinBounds (Vector2 p)
		{
			var lowerLeft  = camera.ScreenToWorldPoint(Vector2.zero);
			var upperRight = camera.ScreenToWorldPoint(new Vector2 (camera.pixelWidth,camera.pixelHeight));

			return	p.x >  lowerLeft.x - Const.CameraBoundsExtraMargin &&
					p.y >  lowerLeft.y - Const.CameraBoundsExtraMargin &&
					p.x < upperRight.x + Const.CameraBoundsExtraMargin &&
					p.y < upperRight.y + Const.CameraBoundsExtraMargin;
		}

		// get and set _isFollowingPlayer
		public bool getIsFollowingPlayer ()	{ return _isFollowingPlayer; }
		public void setIsFollowingPlayer (bool b) {  _isFollowingPlayer = b; }

		// called by the CameraPinchZoomPanCommand
		public void setProposedCameraOrthographicSize (float proposedCameraOrthographicSize)
		{
			isNewProposedCameraOrthographicSize = true;
			this._proposedCameraOrthographicSize = proposedCameraOrthographicSize;
		}

		public void setProposedCameraPosition (Vector2 proposedCameraPosition)
		{
			isNewProposedCameraPosition = true;
			this._proposedCameraPosition = proposedCameraPosition;
		}

		public float getProposedCameraOrthographicSize ()
		{
			isNewProposedCameraOrthographicSize = false;
			return _proposedCameraOrthographicSize;
		}

		public Vector3 getProposedCameraPosition ()
		{
			isNewProposedCameraPosition = false;

			return new Vector3 (
				_proposedCameraPosition.x,
				_proposedCameraPosition.y,
				camera.transform.position.z);
		}
	}
}