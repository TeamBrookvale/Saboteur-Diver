using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.Game
{
	public class ZeppelinMediator : Mediator
	{
		[Inject]
		public ZeppelinView view {get;set;}

		[Inject]
		public IZeppelinModel model {get;set;}

		public override void OnRegister ()
		{
			var p = model.registerShipPatrol (view.GetInstanceID ());
			view.init (p);
		}
		
		public override void OnRemove ()
		{
		}

	}
}