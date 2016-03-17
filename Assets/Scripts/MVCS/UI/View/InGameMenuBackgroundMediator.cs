using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.UI
{
	public class InGameMenuBackgroundMediator : Mediator
	{
		[Inject]
		public InGameMenuBackgroundView view {get;set;}

		[Inject]
		public IInGameMenuModel model {get;set;}

		[Inject]
		public InGameMenuFadeSignal inGameMenuFadeSignal {get;set;}

		public override void OnRegister ()
		{
			view.init ();
		
			inGameMenuFadeSignal.AddListener (view.OnInGameFadeSignal);
		}

		public override void OnRemove ()
		{
			inGameMenuFadeSignal.RemoveListener (view.OnInGameFadeSignal);
		}
	}
}