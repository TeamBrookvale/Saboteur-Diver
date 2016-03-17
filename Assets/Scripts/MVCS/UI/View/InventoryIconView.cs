using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.UI
{
	public class InventoryIconView : View
	{
		public GUIElemRectRegisterSignal _guiElemRectRegisterSignal = new GUIElemRectRegisterSignal();
		public TeamBrookvale.InventoryIconPressSignal _inventoryIconPressSignal = new TeamBrookvale.InventoryIconPressSignal();

		TeamBrookvale.Game.IInventoryModel inventoryModel;
		TeamBrookvale.Game.IGameModel gameModel;
		bool prevIsButtonPushed, isButtonPushed;

		Rect iconRect = new Rect (
			Screen.width  - Const.ToolIconPixelSizeX - Const.IconScreenBorderMargin,
			Screen.height - Const.ToolIconPixelSizeY - Const.IconScreenBorderMargin,
			Const.ToolIconPixelSizeX,
			Const.ToolIconPixelSizeY);

		bool[] last10isButtonPushed = new bool [10];

		public static Rect getIconRect {get; private set;}

		GUIStyle guiStyle = new GUIStyle ();

		internal void init (TeamBrookvale.Game.IInventoryModel inventoryModel, TeamBrookvale.Game.IGameModel gameModel)
		{
			this.inventoryModel = inventoryModel;
			this.gameModel = gameModel;

			_guiElemRectRegisterSignal.Dispatch (iconRect, true);
			getIconRect = iconRect;

			guiStyle.alignment = TextAnchor.MiddleCenter;
		}

		void OnGUI ()
		{
			// if not paused
			if (gameModel.pauseStatus == TeamBrookvale.Game.PauseStatus.RUN)
			{
				// place in front of the counter icon
				GUI.depth = 0;

				// Draw the Active Tool Icon
				GUI.DrawTexture (iconRect, inventoryModel.getCurrentInvItem().icon);
				isButtonPushed = GUI.RepeatButton (iconRect, "", "");
			}
		}

		void Update ()
		{
			////// MONKEYPATCH, ha az utolsó 10 alkalommal volt legalább egy megnyomás, akkor olyan, mintha fel se lett volna engedve
			for (int i = 0; i < last10isButtonPushed.Length - 1; i++)
				last10isButtonPushed[i] = last10isButtonPushed[i+1];
			last10isButtonPushed [last10isButtonPushed.Length - 1] = isButtonPushed;
			// if at least one button pushed, then it's considered one button pushed
			foreach (bool b in last10isButtonPushed)
				if (b)
					isButtonPushed = true;
			////// MONKEYPATCH, ha az utolsó 10 alkalommal volt legalább egy megnyomás, akkor olyan, mintha fel se lett volna engedve


			if (isButtonPushed != prevIsButtonPushed)
			{
				// if button push toggled dipatch signal so mediator can update the icon
				_inventoryIconPressSignal.Dispatch (isButtonPushed);
				prevIsButtonPushed = isButtonPushed;
			}
		}
	}
}