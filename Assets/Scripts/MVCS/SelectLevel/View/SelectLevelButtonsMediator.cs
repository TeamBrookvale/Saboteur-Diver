using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.SelectLevel
{
	public class SelectLevelButtonsMediator : Mediator
	{
		[Inject]
		public SelectLevelButtonsView view { get; set; }

		[Inject]
		public ISelectLevelModel model { get; set; }

		[Inject]
		public LoadLevelSignal loadLevelSignal { get; set; }

		public override void OnRegister ()
		{
			view.init (model);

			view.loadLevelSignal.AddListener (loadLevelSignal.Dispatch);
		}

		public override void OnRemove ()
		{
			view.loadLevelSignal.RemoveListener (loadLevelSignal.Dispatch);
		}
	}
}