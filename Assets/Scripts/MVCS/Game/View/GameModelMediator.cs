using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class GameModelMediator : Mediator
	{
		[Inject]
		public GameModelView view {get;set;}

		[Inject]
		public IGameModel model {get;set;}

		public override void OnRegister ()
		{
			view.init (model);
		}
	}
}