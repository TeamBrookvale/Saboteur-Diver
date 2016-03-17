using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.Game
{
	public class ShipPatrolMediator : Mediator
	{
		[Inject]
		public ShipPatrolView view {get;set;}

		[Inject]
		public IShipPatrolModel model {get;set;}

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