using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class MuzzleFlashMediator : Mediator
	{
		[Inject]
		public MuzzleFlashView view {get;set;}

		[Inject]
		public IPlayerModel playerModel {get;set;}

		public override void OnRegister ()
		{
			view.init (playerModel.getCachedPosition());
		}
	}
}