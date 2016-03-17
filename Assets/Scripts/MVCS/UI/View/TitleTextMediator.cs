using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using TeamBrookvale.Game;

namespace TeamBrookvale.UI
{
	public class TitleTextMediator : Mediator
	{
		[Inject]
		public TitleTextView view {get;set;}

		[Inject]
		public ILevelTextModel model {get;set;}

		[Inject]
		public GameLevelPassedSignal gameLevelPassedSignal {get;set;}

		[Inject]
		public GameLevelFailedSignal gameLevelFailedSignal {get;set;}

		[Inject]
		public IGameModel gameModel {get;set;} 

		public override void OnRegister ()
		{
			view.init (model, gameModel);

			gameLevelPassedSignal.AddListener(view.OnGameLevelPassed);
			gameLevelFailedSignal.AddListener(view.OnGameLevelFailed);
		}

		public override void OnRemove ()
		{
			gameLevelPassedSignal.RemoveListener(view.OnGameLevelPassed);
			gameLevelFailedSignal.RemoveListener(view.OnGameLevelFailed);
		}
	}
}