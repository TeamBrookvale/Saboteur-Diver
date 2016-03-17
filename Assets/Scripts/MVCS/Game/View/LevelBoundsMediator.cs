using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.Game
{
	public class LevelBoundsMediator : Mediator
	{
		[Inject]
		public LevelBoundsView view {get;set;}

		[Inject]
		public ILevelBoundsModel model {get;set;}

		public override void OnRegister ()
		{
			view.init (model);
		}

		public override void OnRemove ()
		{
		}
	}
}