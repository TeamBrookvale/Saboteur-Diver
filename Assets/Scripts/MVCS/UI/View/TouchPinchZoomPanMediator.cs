using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.UI
{
	public class TouchPinchZoomPanMediator : Mediator
	{
		[Inject]
		public TouchPinchZoomPanView view {get;set;}

		[Inject]
		public TouchPinchZoomPanSignal touchPinchZoomPanSignal {get;set;}

		public override void OnRegister ()
		{
			view._touchPinchZoomPanSignal.AddListener (touchPinchZoomPanSignal.Dispatch);
		}
		
		public override void OnRemove ()
		{
			view._touchPinchZoomPanSignal.RemoveListener (touchPinchZoomPanSignal.Dispatch);
		}
	}
}