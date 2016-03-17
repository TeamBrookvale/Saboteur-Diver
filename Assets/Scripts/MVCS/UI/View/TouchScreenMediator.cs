using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.UI
{
	public class TouchScreenMediator : Mediator
	{
		[Inject]
		public TouchScreenView view {get;set;}

		[Inject (CrossContextElements.GAME_CAMERA)]
		public Camera cam {get;set;}

		[Inject]
		public FireTouchEventSignal fireTouchEventSignal {get;set;}

		public override void OnRegister ()
		{
			view._fireTouchEventSignal.AddListener (fireTouchEventSignal.Dispatch);

			view.init (cam);
		}

		public override void OnRemove ()
		{
			view._fireTouchEventSignal.RemoveListener (fireTouchEventSignal.Dispatch);
		}
	}
}