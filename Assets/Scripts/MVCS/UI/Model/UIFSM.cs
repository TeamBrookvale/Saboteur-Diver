using System;

namespace TeamBrookvale.UI
{
	public class UIFSM : IUIFSM
	{
		[Inject]
		public UIPlayAreaPressSignal uiPlayAreaPressSignal {get;set;}

		[Inject]
		public UIPanBackToPlayerSignal uiPanBackToPlayerSignal {get;set;}

		[Inject]
		public UIPlayerActionSignal uiPlayerActionSignal {get;set;}

		[Inject]
		public ITouchModel touchModel {get;set;}

		States currentState;

		public enum States {
			FollowingPlayer,
			Pan,
			PlayerAction,	// e.g. drilling or smoke bomb
			Menu
		}

		public enum Events {
			Press,
			LongPress,
			Pan,
			Release
		}

		// if no TouchScreenPosition class supplied
		public void fire (Events evt)
		{
			fire (evt, TouchScreenPosition.zero);
		}

		// init invoked from TouchFSM
		public void fire (Events e, TouchScreenPosition t)
		{
			//States oldState = currentState;

			switch (currentState)
			{
			
			case States.FollowingPlayer :
				switch (e)
				{
				case Events.Press :
					if (!touchModel.isGuiItem(t))
						uiPlayAreaPressSignal.Dispatch (t);
					break;
				case Events.Pan :
					currentState = States.Pan;
					break;
				case Events.LongPress :
					currentState = States.PlayerAction;
					uiPlayerActionSignal.Dispatch (t, true);
					break;
				default :
					break;
				}
				break;
			
			case States.Menu :
				break;
			
			case States.Pan :
				switch (e)
				{
				case Events.Press :
					currentState = States.FollowingPlayer;
					uiPanBackToPlayerSignal.Dispatch ();
					break;
				default :
					break;
				}
				break;
			
			case States.PlayerAction :
				switch (e)
				{
				case Events.Release :
					currentState = States.FollowingPlayer;
					uiPlayerActionSignal.Dispatch (t, false);
					break;
				}
				break;
			}

			// UnityEngine.Debug.Log ("UIFSM - " + oldState + " (" + e +") -> " + currentState);
		}
	}
}