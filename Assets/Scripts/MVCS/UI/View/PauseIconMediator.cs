using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.UI
{
	public class PauseIconMediator : Mediator
	{
		[Inject]
		public PauseIconView view {get;set;}

		[Inject]
		public GUIElemRectRegisterSignal guiElemRectRegisterSignal {get;set;} 

		[Inject]
		public TeamBrookvale.PauseResumeLevelPassedFailedSignal pauseResumeLevelPassedFailedSignal {get;set;}

		[Inject]
		public TeamBrookvale.Game.IGameModel gameModel {get;set;}

		public override void OnRegister ()
		{
			view._guiElemRectRegisterSignal.AddListener (guiElemRectRegisterSignal.Dispatch);
			view._pauseResumeLevelPassedFailedSignal.AddListener (pauseResumeLevelPassedFailedSignal.Dispatch);

			// init should be after signals subscribed
			view.init (gameModel);
		}
		
		public override void OnRemove ()
		{
			view._guiElemRectRegisterSignal.RemoveListener (guiElemRectRegisterSignal.Dispatch);
			view._pauseResumeLevelPassedFailedSignal.RemoveListener (pauseResumeLevelPassedFailedSignal.Dispatch);
		}
	}
}