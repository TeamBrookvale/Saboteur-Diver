using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.UI
{
	public class ProgressCounterIconMediator : Mediator
	{
		[Inject]
		public ProgressCounterIconView view {get;set;}

		[Inject]
		public GUIElemRectRegisterSignal guiElemRectRegisterSignal {get;set;} 

		[Inject]
		public ILevelTextModel levelTextModel {get;set;}

		[Inject]
		public TeamBrookvale.Game.IGameModel gameModel {get;set;}

		[Inject]
		public TeamBrookvale.Game.IDamBridgeModel damBridgeModel {get;set;}

		[Inject]
		public TeamBrookvale.Game.IShipDrillableModel shipDrillableModel {get;set;}

		public override void OnRegister ()
		{
			view._guiElemRectRegisterSignal.AddListener (guiElemRectRegisterSignal.Dispatch);
		
			// init should be after signals subscribed
			view.init (
				gameModel,
				levelTextModel.AtwriterFont,
				damBridgeModel.numberOfTimeBombsMounted,
				shipDrillableModel.numberOfShips,
				shipDrillableModel.numberOfShipsSunk);
		}
		
		public override void OnRemove ()
		{
			view._guiElemRectRegisterSignal.RemoveListener (guiElemRectRegisterSignal.Dispatch);
		}
	}
}