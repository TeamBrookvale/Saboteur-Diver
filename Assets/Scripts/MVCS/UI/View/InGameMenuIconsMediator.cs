using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.UI
{
	public class InGameMenuIconsMediator : Mediator
	{
		[Inject]
		public InGameMenuIconsView view {get;set;}

		[Inject]
		public IInGameMenuModel model {get;set;}

		[Inject]
		public InGameMenuShowSignal inGameMenuShowSignal {get;set;}

		[Inject]
		public InGameMenuPushSignal inGameMenuPushSignal {get;set;}

		[Inject]
		public TeamBrookvale.Game.IAPInGameMenuSignal iapInGameMenuSignal {get;set;}

		[Inject]
		public TeamBrookvale.Game.IAPThankYouSignal iapThankYouSignal {get;set;}

		public override void OnRegister ()
		{
			view.init (model);
		
			inGameMenuShowSignal.AddListener				(view.OnInGameMenuShowSignal);
			iapThankYouSignal.AddListener					(view.OnIAPThankYouSignal);

			view._inGameMenuPushSignal.AddListener			(inGameMenuPushSignal.Dispatch);
			view._iapInGameMenuSignal.AddListener			(iapInGameMenuSignal.Dispatch);
		}

		public override void OnRemove ()
		{
			inGameMenuShowSignal.RemoveListener				(view.OnInGameMenuShowSignal);
			iapThankYouSignal.RemoveListener	(view.OnIAPThankYouSignal);

			view._inGameMenuPushSignal.RemoveListener		(inGameMenuPushSignal.Dispatch);
			view._iapInGameMenuSignal.RemoveListener		(iapInGameMenuSignal.Dispatch);
		}
	}
}