using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class StatusBarMessagesDispatcherMediator : Mediator
	{
		[Inject]
		public StatusBarMessagesDispatcherView view {get;set;}

		[Inject]
		public StatusBarMessageSignal statusBarMessageSignal {get;set;}

		[Inject]
		public StatusBarMessageRemoveSignal statusBarMessageRemoveSignal {get;set;}

		[Inject]
		public IPlayerModel playerModel {get;set;}

		[Inject]
		public IGameModel gameModel {get;set;}

		public override void OnRegister ()
		{
			view.init (playerModel.getCachedPosition, gameModel.triggerItems);
			
			view._statusBarMessageSignal.AddListener (statusBarMessageSignal.Dispatch);
			view._statusBarMessageRemoveSignal.AddListener (statusBarMessageRemoveSignal.Dispatch);
		}
		
		public override void OnRemove ()
		{
			view._statusBarMessageSignal.RemoveListener (statusBarMessageSignal.Dispatch);
			view._statusBarMessageRemoveSignal.RemoveListener (statusBarMessageRemoveSignal.Dispatch);

		}

	}
}