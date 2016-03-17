namespace TeamBrookvale.UI
{
	public class TouchFSM : ITouchFSM
	{
		[Inject]
		public IUIFSM uiFSM {get;set;}

		/*bool isInitialized;

		PassiveStateMachine<States, Events> fsm {get;set;}
		TeamBrookvale.PassiveStateMachineExtension<States, Events> fsmExtension;
		*/

		States currentState;

		public enum States { 
			Idle,
			FirstFingerBegan,
			Pan,
			SecondFingerBegan,
			PinchZoom,
			LongPress
		}
		
		public enum Events { 
			FirstFingerBegan,
			FirstFingerMoved,
			FirstFingerLongTouch,
			SecondFingerBegan,
			PinchZoom,
			AnyFingersEnded
		}

		// Fired by FireTouchEventCommand
		public States fireTouchEvent (Events e, TouchScreenPosition t)
		{
			//States oldState = currentState;

			switch (currentState)
			{
			case States.Idle :
				switch (e)
				{
				case Events.FirstFingerBegan :
					currentState = States.FirstFingerBegan;
					break;
				default :
					break;
				}
				break;

			case States.FirstFingerBegan :
				switch (e)
				{
				case Events.FirstFingerMoved :
					currentState = States.Pan;
					uiFSM.fire (UIFSM.Events.Pan, t);
					break;
				case Events.AnyFingersEnded :
					currentState = States.Idle;
					uiFSM.fire (UIFSM.Events.Press, t);
					break;
				case Events.SecondFingerBegan :
					currentState = States.SecondFingerBegan;
					break;
				case Events.FirstFingerLongTouch :
					currentState = States.LongPress;
					uiFSM.fire (UIFSM.Events.LongPress, t);
					break;
				default :
					break;
				}
				break;
			
			case States.SecondFingerBegan :
				switch (e)
				{
				case Events.AnyFingersEnded :
					currentState = States.Idle;
					break;
				case Events.FirstFingerMoved :
					currentState = States.Pan;
					uiFSM.fire (UIFSM.Events.Pan, t);
					break;
				case Events.PinchZoom :
					currentState = States.PinchZoom;
					break;
				default :
					break;
				}
				break;

			case States.Pan :
				switch (e)
				{
				case Events.PinchZoom :
					currentState = States.PinchZoom;
					break;
				case Events.AnyFingersEnded :
					currentState = States.Idle;
					break;
				default :
					break;
				}
				break;

			case States.PinchZoom :
				switch (e)
				{
				case Events.AnyFingersEnded :
					currentState = States.Idle;
					break;
				default:
					break;
				}
				break;

			case States.LongPress :
				switch (e)
				{
				case Events.AnyFingersEnded :
					uiFSM.fire (UIFSM.Events.Release, t);
					currentState = States.Idle;
					break;
				default :
					break;
				}
				break;

			default:
				break;
			}

			// UnityEngine.Debug.Log ("TouchFSM - " + oldState + " (" + e +") -> " + currentState);

			return currentState;
		}
	}
}