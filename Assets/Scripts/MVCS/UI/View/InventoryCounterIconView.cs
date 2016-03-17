using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.UI
{
	public class InventoryCounterIconView : View
	{
		public GUIElemRectRegisterSignal _guiElemRectRegisterSignal = new GUIElemRectRegisterSignal();

		TeamBrookvale.Game.IInventoryModel inventoryModel;
		TeamBrookvale.Game.IGameModel gameModel;

		Rect iconRect = new Rect (
			Screen.width  - (Const.ToolIconPixelSizeX - (Const.ToolIconPixelSizeX - Const.CounterIconPixelSizeX) / 2) - Const.IconScreenBorderMargin,
			Screen.height - Const.ToolIconPixelSizeY - Const.InventoryCounterIconPositionOffsetY - Const.IconScreenBorderMargin,
			Const.CounterIconPixelSizeX,
			Const.CounterIconPixelSizeY);

		Rect textRect;

		Texture2D counterIcon;
		GUIStyle textGuiStyle = new GUIStyle ();

		internal void init (TeamBrookvale.Game.IInventoryModel inventoryModel, TeamBrookvale.Game.IGameModel gameModel, Font font)
		{
			this.inventoryModel = inventoryModel;
			this.gameModel = gameModel;

			_guiElemRectRegisterSignal.Dispatch (iconRect, true);

			counterIcon = Resources.Load<Texture2D> ("UI/Counters/InventoryCounterBackgroundIcon"); // this one is used as a background for the counter
			if (counterIcon == null) Debug.LogError ("InventoryCounterBackgroundIcon not found");

			// initialize text
			textGuiStyle.font = font;
			textGuiStyle.alignment = TextAnchor.MiddleCenter;
			textGuiStyle.normal.textColor = Color.white;
			textGuiStyle.fontSize = TeamBrookvale.Game.TBUtil.CalcFontSize (30);
	
			// move the text a few pixels up from the middle of the circle
			textRect = new Rect (
				iconRect.xMin,
				iconRect.yMin - 20 * Screen.height / 640,
				iconRect.width,
				iconRect.height);

		}

		void OnGUI () {
			// if game is not paused
			if (gameModel.pauseStatus == TeamBrookvale.Game.PauseStatus.RUN)
			{
				if (inventoryModel.getCurrentInvItem().id == TeamBrookvale.Game.InvItem.IDType.SmokeBombActive ||
				    inventoryModel.getCurrentInvItem().id == TeamBrookvale.Game.InvItem.IDType.SmokeBombInactive/* ||
				    inventoryModel.getCurrentInvItem().id == TeamBrookvale.Game.InvItem.IDType.TimeBombActive ||
				    inventoryModel.getCurrentInvItem().id == TeamBrookvale.Game.InvItem.IDType.TimeBombInactive ||
				    inventoryModel.getCurrentInvItem().id == TeamBrookvale.Game.InvItem.IDType.TimeBombInactiveTick*/)
				{
					// place behind the tool icon
					GUI.depth = 1;
					// not button, but should appear the same as the others
					GUI.DrawTexture (iconRect, counterIcon);

					// show the text i.e. "1"
					GUI.Label (
						textRect,
						inventoryModel.getCurrentInvItemText(),
						textGuiStyle);
				}
			}
		}
	}
}