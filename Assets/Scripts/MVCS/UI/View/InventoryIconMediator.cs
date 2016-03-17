using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.UI
{
	public class InventoryIconMediator : Mediator
	{
		[Inject]
		public InventoryIconView view {get;set;}

		[Inject]
		public GUIElemRectRegisterSignal guiElemRectRegisterSignal {get;set;} 

		[Inject]
		public TeamBrookvale.Game.IInventoryModel inventoryModel {get;set;}

		[Inject]
		public TeamBrookvale.InventoryIconPressSignal inventoryIconPressSignal {get;set;}

		[Inject]
		public TeamBrookvale.Game.IGameModel gameModel {get;set;}

		public override void OnRegister ()
		{
			view._guiElemRectRegisterSignal.AddListener (guiElemRectRegisterSignal.Dispatch);
			view._inventoryIconPressSignal.AddListener (inventoryIconPressSignal.Dispatch);

			// init should be after signals subscribed
			view.init (inventoryModel, gameModel);
		}
		
		public override void OnRemove ()
		{
			view._guiElemRectRegisterSignal.RemoveListener (guiElemRectRegisterSignal.Dispatch);
			view._inventoryIconPressSignal.RemoveListener (inventoryIconPressSignal.Dispatch);
		}
	}
}