using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.UI
{
	public class InventoryCounterIconMediator : Mediator
	{
		[Inject]
		public InventoryCounterIconView view {get;set;}

		[Inject]
		public GUIElemRectRegisterSignal guiElemRectRegisterSignal {get;set;} 

		[Inject]
		public TeamBrookvale.Game.IInventoryModel inventoryModel {get;set;}

		[Inject]
		public ILevelTextModel levelTextModel {get;set;}

		[Inject]
		public TeamBrookvale.Game.IGameModel gameModel {get;set;}

		public override void OnRegister ()
		{
			view._guiElemRectRegisterSignal.AddListener (guiElemRectRegisterSignal.Dispatch);
		
			// init should be after signals subscribed
			view.init (inventoryModel, gameModel, levelTextModel.AtwriterFont);
		}
		
		public override void OnRemove ()
		{
			view._guiElemRectRegisterSignal.RemoveListener (guiElemRectRegisterSignal.Dispatch);
		}
	}
}