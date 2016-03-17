using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.UI
{
	public class TouchPinchZoomPanView : View
	{
		public TouchPinchZoomPanSignal _touchPinchZoomPanSignal = new TouchPinchZoomPanSignal();

		void Update()
		{
			float deltaMagnitudeDiff = 0;
			Vector3 panDeltaPosition = Vector3.zero;

			// Pinch & Zoom
			if (Input.touchCount >= 2)
			{

				// Store both touches.
				Touch touchZero = Input.GetTouch(0);
				Touch touchOne = Input.GetTouch(1);
				
				// Find the position in the previous frame of each touch.
				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
				
				// Find the magnitude of the vector (the distance) between the touches in each frame.
				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
				
				// Find the difference in the distances between each frame.
				deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
			}

			// Pan
			if (Input.touchCount >= 1)
			{
				panDeltaPosition = Const.CameraPanSpeed * new Vector3 (
					- Input.GetTouch(0).deltaPosition.x,
					- Input.GetTouch(0).deltaPosition.y);

				// Dispatch signal if Pinch & Zoom or Pan
				_touchPinchZoomPanSignal.Dispatch (deltaMagnitudeDiff, panDeltaPosition);
			}
		}
	}
}