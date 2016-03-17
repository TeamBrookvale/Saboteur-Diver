using UnityEngine;
using strange.extensions.mediation.impl;
using System.Collections.Generic;

namespace TeamBrookvale.UI
{
	public class TutorialMediator : Mediator
	{
		[Inject]
		public TutorialView view {get;set;}

		[Inject]
		public TeamBrookvale.Game.ITutorialModel model {get;set;}

		[Inject]
		public TeamBrookvale.Game.IGameModel gameModel {get;set;}

		[Inject]
		public GUIElemRectRegisterSignal guiElemRectRegisterSignal {get;set;}

		public override void OnRegister ()
		{
			view.init (model.getTutorialEntriesForLevel (gameModel.currentLevel));

			view._guiElemRectRegisterSignal.AddListener (guiElemRectRegisterSignal.Dispatch);

			/*foreach (Tutorial model.tutorialEntries.

			view.init (
			Func<List<TutorialEntry>> getcurrentLevelsTutorialEntries)*/
		}

		public override void OnRemove ()
		{
			view._guiElemRectRegisterSignal.RemoveListener (guiElemRectRegisterSignal.Dispatch);
		}
	}
}