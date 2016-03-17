using UnityEngine;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class WaterMediator : Mediator
	{
		[Inject]
		public WaterView view {get;set;}

		[Inject]
		public PanicModeStartedOrEndedSignal panicModeStartedOrEndedSignal {get;set;}


		public override void OnRegister ()
		{
			panicModeStartedOrEndedSignal.AddListener (view.OnPanicModeStartedOrEndedSignal);
		}

		public override void OnRemove ()
		{
			panicModeStartedOrEndedSignal.RemoveListener (view.OnPanicModeStartedOrEndedSignal);
		}
	}
}