using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using System;

namespace TeamBrookvale.UI
{
	public class ProgressCounterIconView : View
	{
		public GUIElemRectRegisterSignal _guiElemRectRegisterSignal = new GUIElemRectRegisterSignal();

		TeamBrookvale.Game.IGameModel gameModel;
		Func<int> numberOfTimeBombsMounted;
		Func<int> numberOfShips;
		Func<int> numberOfShipsSunk;
		
		Rect iconRect = new Rect (
			Screen.width  - (Const.ToolIconPixelSizeX - (Const.ToolIconPixelSizeX - Const.CounterIconPixelSizeX) / 2) - Const.IconScreenBorderMargin,
			Screen.height - Const.ToolIconPixelSizeY - Const.InventoryCounterIconPositionOffsetY - Const.IconScreenBorderMargin - Const.ProgressCounterIconPositionOffsetY,
			Const.CounterIconPixelSizeX,
			Const.CounterIconPixelSizeY);

		public static Rect getIconRect {get; private set;}

		Rect iconPressTextRect;

		Texture2D counterIcon;
		GUIStyle textGuiStyle = new GUIStyle ();

		internal void init (
			TeamBrookvale.Game.IGameModel gameModel,
			Font font,
			Func<int> numberOfTimeBombsMounted,
			Func<int> numberOfShips,
			Func<int> numberOfShipsSunk)
		{
			this.gameModel = gameModel;
			this.numberOfTimeBombsMounted = numberOfTimeBombsMounted;
			this.numberOfShips = numberOfShips;
			this.numberOfShipsSunk = numberOfShipsSunk;

			// initialize text
			textGuiStyle.font = font;
			textGuiStyle.alignment = TextAnchor.MiddleCenter;
			textGuiStyle.normal.textColor = Color.white;
			textGuiStyle.fontSize = TeamBrookvale.Game.TBUtil.CalcFontSize (30);
			
			iconPressTextRect = new Rect 
				(iconRect.xMin - Screen.width * .55f,
				 iconRect.yMin,
				 Screen.width / 2,
				 iconRect.height);

			// decide which icon to use based on the level type
			switch (gameModel.currentLevelType)
			{
			case TeamBrookvale.Game.LevelType.NothingToDemolish :
				counterIcon = null;
				break;
				
			case TeamBrookvale.Game.LevelType.Dam :
			case TeamBrookvale.Game.LevelType.Buoy :
			case TeamBrookvale.Game.LevelType.Bridge :
				counterIcon = Resources.Load<Texture2D> ("UI/Counters/TimeBombCounterIcon"); // this one is used as a background for the counter
				if (counterIcon == null) Debug.LogError ("TimeBombCounterIcon not found");
				break;
				
			case TeamBrookvale.Game.LevelType.ShipDrill :
				counterIcon = Resources.Load<Texture2D> ("UI/Counters/SunkShipCounterIcon"); // this one is used as a background for the counter
				if (counterIcon == null) Debug.LogError ("SunkShipCounterIcon not found");
				break;
				
			default: break;
			}
			
			if (counterIcon != null)
				_guiElemRectRegisterSignal.Dispatch (iconRect, true);
			
			getIconRect = iconRect;
		}

		void OnGUI ()
		{

			// only draw icon if it is not null
			if (counterIcon == null) return;

			// if game is not paused
			if (gameModel.pauseStatus == TeamBrookvale.Game.PauseStatus.RUN)
			{
//				if (inventoryModel.getCurrentInvItem().id == TeamBrookvale.Game.InvItem.IDType.SmokeBombActive ||
//				    inventoryModel.getCurrentInvItem().id == TeamBrookvale.Game.InvItem.IDType.SmokeBombInactive ||
//				    inventoryModel.getCurrentInvItem().id == TeamBrookvale.Game.InvItem.IDType.TimeBombActive ||
//				    inventoryModel.getCurrentInvItem().id == TeamBrookvale.Game.InvItem.IDType.TimeBombInactive ||
//				    inventoryModel.getCurrentInvItem().id == TeamBrookvale.Game.InvItem.IDType.TimeBombInactiveTick)
				{
					// show the icon
					GUI.DrawTexture (iconRect, counterIcon);
					if (GUI.RepeatButton (iconRect, "", ""))
						GUI.Label (iconPressTextRect, getIconPressText (), textGuiStyle);

					// show the text i.e. "1/3"
					GUI.Label (iconRect, getText (), textGuiStyle);
				}
			}
		}

		string getText ()
		{
			string text;

			switch (gameModel.currentLevelType)
			{
			case TeamBrookvale.Game.LevelType.Dam :
			case TeamBrookvale.Game.LevelType.Buoy :
			case TeamBrookvale.Game.LevelType.Bridge :
				text = numberOfTimeBombsMounted () + "/" + Const.numberOfTimeBombsToMount;
				break;
				
			case TeamBrookvale.Game.LevelType.ShipDrill :
				text = numberOfShips () - numberOfShipsSunk () + "/" + numberOfShips ();
				break;
				
			default:
				Debug.LogError ("This state should never be reached");
				text = "";
				break;
			}

			return text;
		}

		string getIconPressText ()
		{
			string text;
			
			switch (gameModel.currentLevelType)
			{
			case TeamBrookvale.Game.LevelType.Dam :
			case TeamBrookvale.Game.LevelType.Bridge :
				text = "Bombs planted: " + numberOfTimeBombsMounted () + " of " + Const.numberOfTimeBombsToMount;
				break;
				
			case TeamBrookvale.Game.LevelType.ShipDrill :
				text = "Ships infiltrated: " + (numberOfShips () - numberOfShipsSunk ()) + " of " + numberOfShips ();
				break;
				
			default:
				Debug.LogError ("This state should never be reached");
				text = "";
				break;
			}
			
			return text;
		}
	}
}