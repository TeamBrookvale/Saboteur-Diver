using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public struct TouchScreenPosition
{
	public Vector2 pixel;
	public Vector2 screen;
	public Vector2 world;

	public TouchScreenPosition (Vector2 pixel, Vector2 screen, Vector2 world)
	{
		this.pixel = pixel; this.screen = screen; this.world = world;
	}

	// constructor with only one parameter will use world and will not care about the others
	public TouchScreenPosition (Vector2 world)
	{
		this.pixel = Vector2.zero;
		this.screen = Vector2.zero;
		this.world = world;
	}

	public static TouchScreenPosition zero { get { return new TouchScreenPosition (Vector2.zero, Vector2.zero, Vector2.zero); }}
}

namespace TeamBrookvale.UI
{
	public class TouchScreenView : View
	{
		internal FireTouchEventSignal _fireTouchEventSignal = new FireTouchEventSignal ();

		float firstFingerBeganTime; // time used for longpress
		Vector2 firstFingerBeganPixelPosition, secondFingerBeganPixelPosition;
		Vector2 firstSecondFingersBeganPixelDistanceVector;
		Camera cam;

		internal void init (Camera cam)
		{
			this.cam = cam;
		}

		void Update ()
		{
			TouchFSM.Events? touchEventToFire = null;
			Vector2? firstFingerPixelPosition = null;
			Vector2? secondFingerPixelPosition = null;

			// handle mouse click JUST DEVELOPER MODE
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0) || Input.GetMouseButton(0))
			{
				firstFingerPixelPosition = Input.mousePosition;

				// Mouse down
				if (Input.GetMouseButtonDown(0))
				{
					firstFingerBeganTime = Time.time;
					touchEventToFire = TouchFSM.Events.FirstFingerBegan;
				}

				// Mouse hold
				if (Input.GetMouseButton(0))
				{
					if (Time.time > firstFingerBeganTime + Const.LongPressTimeThreshold)
						touchEventToFire = TouchFSM.Events.FirstFingerLongTouch;
				}

				// Mouse up
				if (Input.GetMouseButtonUp(0)) touchEventToFire = TouchFSM.Events.AnyFingersEnded;
			}

			// handle touches
			if (Input.touchCount > 0)
			{
				firstFingerPixelPosition = Input.GetTouch(0).position;

				switch (Input.GetTouch(0).phase)
				{
				case TouchPhase.Began :
					touchEventToFire = TouchFSM.Events.FirstFingerBegan;
					firstFingerBeganPixelPosition = (Vector2) firstFingerPixelPosition;
					firstFingerBeganTime = Time.time;
					break;

				case TouchPhase.Stationary :
					goto case TouchPhase.Moved;

				case TouchPhase.Moved :
					// Check for move
					float firstFingerMovedDistance = 
						((Vector2) firstFingerPixelPosition -
						 firstFingerBeganPixelPosition).magnitude;
				
					if (firstFingerMovedDistance > Const.FirstFingerMovePixelThreshold)
						touchEventToFire = TouchFSM.Events.FirstFingerMoved;

					// Check for long press
					else if (Time.time > firstFingerBeganTime + Const.LongPressTimeThreshold)
						touchEventToFire = TouchFSM.Events.FirstFingerLongTouch;


					break;

				case TouchPhase.Ended :
					touchEventToFire = TouchFSM.Events.AnyFingersEnded;
					break;

				default : break;
				}
			}

			if (touchEventToFire != null)
				dispatchFireTouchEventSignal ((TouchFSM.Events) touchEventToFire, (Vector2) firstFingerPixelPosition);

			// second finger used
			if (Input.touchCount > 1)
			{
				secondFingerPixelPosition = Input.GetTouch(1).position;

				switch (Input.GetTouch(1).phase)
				{
				case TouchPhase.Began :
					secondFingerBeganPixelPosition = (Vector2) secondFingerPixelPosition;
					firstSecondFingersBeganPixelDistanceVector = (Vector2)(firstFingerPixelPosition - secondFingerPixelPosition);

					touchEventToFire = TouchFSM.Events.SecondFingerBegan;
					break;

				// decide if Pinch & Zoom or Pan
				case TouchPhase.Moved :
					Vector2 firstFingerDeltaPixelPosition = (Vector2) firstFingerPixelPosition - (Vector2) firstFingerBeganPixelPosition;
					Vector2 secondFingerDeltaPixelPosition = (Vector2) secondFingerPixelPosition - (Vector2) secondFingerBeganPixelPosition;
					Vector2 firstSecondFingersPixelDistanceVector = (Vector2)(firstFingerPixelPosition - secondFingerPixelPosition);

					//Debug.Log ("(firstSecondFingersPixelDistanceVector - firstSecondFingersBeganPixelDistanceVector).magnitude = " + (firstSecondFingersPixelDistanceVector - firstSecondFingersBeganPixelDistanceVector).magnitude +
					//          "      firstFingerDeltaPixelPosition = " + firstFingerDeltaPixelPosition.magnitude + 
					//           "      secondFingerDeltaPixelPosition = " + secondFingerDeltaPixelPosition.magnitude);

					// if at least one finger moved a bit
					if (firstFingerDeltaPixelPosition.magnitude > 10 || secondFingerDeltaPixelPosition.magnitude > 10)

						// did they move the same direction or different
						if ((firstSecondFingersPixelDistanceVector - firstSecondFingersBeganPixelDistanceVector).magnitude > 30)
							touchEventToFire = TouchFSM.Events.PinchZoom;
					break;

				case TouchPhase.Ended :
					touchEventToFire = TouchFSM.Events.AnyFingersEnded;
					break;
				}
			}
		
			if (touchEventToFire != null)
				dispatchFireTouchEventSignal ((TouchFSM.Events) touchEventToFire, (Vector2) firstFingerPixelPosition);
		}

		void dispatchFireTouchEventSignal (TouchFSM.Events e, Vector2 firstFingerPixelPosition)
		{
			TouchScreenPosition p = new TouchScreenPosition() {
				
				pixel = firstFingerPixelPosition,
				
				screen = new Vector2(
					(firstFingerPixelPosition).x / Screen.width,
					(firstFingerPixelPosition).y / Screen.height),
				
				world = (Vector2) cam.ScreenToWorldPoint (new Vector3(
					(firstFingerPixelPosition).x,
					(firstFingerPixelPosition).y
					))};
			
			_fireTouchEventSignal.Dispatch (e, p);
		}
	}
}