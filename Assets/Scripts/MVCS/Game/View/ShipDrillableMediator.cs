using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.Game
{
	public class ShipDrillableMediator : Mediator
	{
		[Inject]
		public ShipDrillableView view {get;set;}

		[Inject]
		public ShipSunkSignal shipSunkSignal {get;set;}

		[Inject]
		public ShipDrillableOnRegisterSignal shipDrillableOnRegisterSignal {get;set;}

		[Inject]
		public IShipDrillableModel shipDrillableModel {get;set;}

		[Inject]
		public ShipsHealthReachedZeroSignal shipsHealthReachedZeroSignal {get;set;}

		public override void OnRegister ()
		{
			// dispatch onregister signal so the ships could be registered in the model
			shipDrillableOnRegisterSignal.Dispatch (view.GetInstanceID());

			// initialize view using the object created by the previous line
			view.init (shipDrillableModel, shipDrillableModel.drillableShipPropertiesDict [view.GetInstanceID ()]);

			// if ship drilled signal received from the view then dispatch ShipDrilledSignal
			view._shipSunkSignal.AddListener (shipSunkSignal.Dispatch);
			view._shipsHealthReachedZeroSignal.AddListener (shipsHealthReachedZeroSignal.Dispatch);
		}

		public override void OnRemove ()
		{
			// unsubscribe from the view
			view._shipSunkSignal.RemoveListener (shipSunkSignal.Dispatch);
			view._shipsHealthReachedZeroSignal.RemoveListener (shipsHealthReachedZeroSignal.Dispatch);
		}
	}
}