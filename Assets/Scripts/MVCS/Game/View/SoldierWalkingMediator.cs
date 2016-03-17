using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.Game
{
	public class SoldierWalkingMediator : Mediator
	{
		[Inject]
		public SoldierWalkingView view {get;set;}

		public override void OnRegister ()
		{
			view.init ();
		}

		public override void OnRemove ()
		{
		}
	}
}