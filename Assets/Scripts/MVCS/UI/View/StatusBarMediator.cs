using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.UI
{
	public class StatusBarMediator : Mediator
	{
		[Inject]
		public StatusBarMessageView view {get;set;}

		[Inject]
		public IStatusBarModel model {get;set;}

		[Inject]
		public TeamBrookvale.StatusBarMessageSignal statusBarMessageSignal {get;set;}
		
		[Inject]
		public TeamBrookvale.StatusBarMessageRemoveSignal statusBarMessageRemoveSignal {get;set;}
		
		[Inject]
		public ILevelTextModel levelTextModel {get;set;}

		public override void OnRegister ()
		{
			view.init (model, levelTextModel.AtwriterFont);

			statusBarMessageSignal.AddListener (view.OnStatusBarMessageSignal);
			statusBarMessageRemoveSignal.AddListener (view.OnStatusBarMessageRemoveSignal);
		}

		public override void OnRemove ()
		{
			statusBarMessageSignal.RemoveListener (view.OnStatusBarMessageSignal);
			statusBarMessageRemoveSignal.RemoveListener (view.OnStatusBarMessageRemoveSignal);
		}
	}
}