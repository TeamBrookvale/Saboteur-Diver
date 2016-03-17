using UnityEngine;
using System.Collections;

using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class SpotLightMediator : Mediator
	{
		[Inject]
		public ISpotlightDictionary spotlightDict {get;set;}

		[Inject]
		public ISpotlightModel spotlightModel {get;set;}

		[Inject]
		public SpotlightPlayerNoticedSignal spotlightPlayerNoticedSignal {get;set;}

		[Inject]
		public SpotLightView view {get;set;}

		[Inject]
		public ICameraModel cameraModel {get;set;}

		[Inject]
		public SmokeBombExplodedSignal smokeBombExplodedSignal {get;set;}

		[Inject]
		public IShipDrillableModel shipDrillableModel {get;set;}

		[Inject]
		public SpotlightSunkShipNoticedSignal spotlightSunkShipNoticedSignal {get;set;}

		[Inject]
		public IPlayerModel playerModel {get;set;}

		[Inject]
		public IGameModel gameModel {get;set;}

		// Keep last dipatched signal so not dispatching the same signal again
		enum DispatchType { Noticed, NotVisible }
		DispatchType _lastDispatchType;

		ISpotlightModel _spotlightModel; // this is the current view's spotlightModel;

		public override void OnRegister()
		{
			// Subscribe to view's signals
			view._spotlightPlayerNoticedSignal.AddListener (spotlightPlayerNoticedSignal.Dispatch);
			view._spotlightSunkShipNoticedSignal.AddListener (spotlightSunkShipNoticedSignal.Dispatch);

			// Subscribe to external signals
			smokeBombExplodedSignal.AddListener (view.OnSmokeBombExplodedSignal );

			// get a spotlightmodel and populate fields sourced from the XML
			_spotlightModel = spotlightModel;
			_spotlightModel.spotMedianAngle = view.spotMedianAngle;

			// add current spotlight to the dictionary
			spotlightDict.addInstance (view.GetInstanceID(), _spotlightModel);

			// run view's init method
			view.init (_spotlightModel, shipDrillableModel.drillAirBubblePositionList, playerModel, gameModel);
		}

		public override void OnRemove ()
		{
			// Unsubscribe to view's signals
			view._spotlightPlayerNoticedSignal.RemoveListener (spotlightPlayerNoticedSignal.Dispatch);
			view._spotlightSunkShipNoticedSignal.RemoveListener (spotlightSunkShipNoticedSignal.Dispatch);

			// Subscribe to external signals
			smokeBombExplodedSignal.RemoveListener (view.OnSmokeBombExplodedSignal );
		}
	}
}