using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.UI
{
	public class PauseIconView : View
	{
		public GUIElemRectRegisterSignal _guiElemRectRegisterSignal = new GUIElemRectRegisterSignal();
		public TeamBrookvale.PauseResumeLevelPassedFailedSignal _pauseResumeLevelPassedFailedSignal = new TeamBrookvale.PauseResumeLevelPassedFailedSignal();

		TeamBrookvale.Game.IGameModel gameModel;
		bool prevIsButtonPushed, isButtonPushed;
		Texture2D icon;

		Rect iconRect = new Rect (
			Screen.width  - Const.ToolIconPixelSizeX / 2 - Const.IconScreenBorderMargin,
			Const.IconScreenBorderMargin,
			Const.ToolIconPixelSizeX / 2,
			Const.ToolIconPixelSizeY / 2);

		public static Rect getIconRect {get; private set;}
				
		internal void init (TeamBrookvale.Game.IGameModel gameModel)
		{
			this.gameModel = gameModel;

			_guiElemRectRegisterSignal.Dispatch (iconRect, true);
			getIconRect = iconRect;

			string iconPath = "UI/PauseIcon";
			icon = Resources.Load<Texture2D> (iconPath);

			if (icon == null)
				Debug.LogError (iconPath + " is missing");
		}

		void OnGUI ()
		{
			// Draw the Active Tool Icon if not paused
			if (gameModel.pauseStatus == TeamBrookvale.Game.PauseStatus.RUN)
			{
				GUI.DrawTexture (iconRect, icon);
				isButtonPushed = GUI.RepeatButton (iconRect, "", "");
			}
		}

		void Update ()
		{
			if (isButtonPushed != prevIsButtonPushed)
			{
				prevIsButtonPushed = isButtonPushed;

				if (isButtonPushed)
					_pauseResumeLevelPassedFailedSignal.Dispatch (InGameMenuModel.MenuState.Pause);
			}
		}
	}
}