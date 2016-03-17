using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.Game
{
	public class IAPMediator : Mediator
	{
		[Inject]
		public IAPView view {get;set;}

		[Inject]
		public IAPThankYouSignal iapThankYouSignal {get;set;}

		static IAPMediator mediator;

		// called by IAPInit events
		public static void InvokeIAPThankYou ()
		{
			if (mediator != null)
				mediator.iapThankYouSignal.Dispatch ();
		}

		public override void OnRegister ()
		{
			if (mediator == null)
				mediator = this;
			else
				Debug.LogError ("There should not be more than one IAPMediator in the scene");
		}

		public override void OnRemove ()
		{
			mediator = null;
		}
	}
}