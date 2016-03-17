using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.context.api;

namespace TeamBrookvale.UI
{
	public class CameraPinchZoomPanCommand : Command
	{
		[Inject (CrossContextElements.GAME_CAMERA)]
		public Camera cam {get;set;}

		[Inject]
		public ITouchModel touchModel {get;set;}

		[Inject]
		public TeamBrookvale.Game.ICameraModel cameraModel {get;set;}

		// Signal injections
		[Inject]
		public float pinchDeltaMagnitudeDiff {get;set;} // delta difference of finger distances - used for Pinch & Zoom
		
		[Inject]
		public Vector3 panDeltaPosition {get;set;}

		public override void Execute ()
		{
			// Pinch & Zoom
			if (touchModel.getCurrentState() == TouchFSM.States.PinchZoom)
			{
				// ... change the orthographic size based on the change in distance between the touches.
				float proposedCameraOrhographicSize =
					cam.orthographicSize + pinchDeltaMagnitudeDiff * Const.CameraOrthographicZoomSpeed;
				
				// Make sure the orthographic size never drops beyond the limits.
				proposedCameraOrhographicSize = Mathf.Clamp (
					proposedCameraOrhographicSize,
					Const.CameraOrthographicMinSize,
					Const.CameraOrthographicMaxSize);

				// propose the new orthographic size
				cameraModel.setProposedCameraOrthographicSize (proposedCameraOrhographicSize);
			}

			// Pan
			if (touchModel.getCurrentState() == TouchFSM.States.Pan)
			{
				// multiplied by proposedCameraOrhographicSize so panning speed is dependent on zoom
				cameraModel.setProposedCameraPosition (
					cam.transform.position + (Vector3) panDeltaPosition * cam.orthographicSize);

				// camera is not following player anymore as panning
				cameraModel.setIsFollowingPlayer (false);
			}
		}
	}
}