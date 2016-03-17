using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.UI
{
	public class StatusBarMessageView : View
	{
		Texture2D statusBarTexture;
		Rect rect;
		float rectShownY;
		float rectHiddenY;
		GUIStyle textStyle;
		Font font;
		IStatusBarModel model;

		internal void init (IStatusBarModel model, Font font)
		{
			this.model = model;

			statusBarTexture = Resources.Load<Texture2D> ("UI/StatusBar");

			if (statusBarTexture == null) Debug.LogError ("StatusBar could not be loaded");

			rectShownY = Screen.height - Const.ToolIconPixelSizeY / 2;
			rectHiddenY= Screen.height;

			rect = new Rect (
				0,
				rectHiddenY,
				Screen.width - Const.IconScreenBorderMargin * 3 - Const.ToolIconPixelSizeX,
				Const.ToolIconPixelSizeY / 2);

			textStyle = GUIStyle.none;
			textStyle.font = font;
			textStyle.fontSize = TeamBrookvale.Game.TBUtil.CalcFontSize (25);
			textStyle.normal.textColor = Color.white; 

		}

		internal void OnStatusBarMessageSignal (string message)
		{
			model.addNewMessage (message);
		}

		internal void OnStatusBarMessageRemoveSignal (string message)
		{
			model.removeMessage (message);
		}

		void OnGUI ()
		{
			rect = new Rect (
				rect.x,
					Mathf.Lerp (
						model.isStatusBarVisible() ? rectHiddenY: rectShownY,
						model.isStatusBarVisible() ? rectShownY : rectHiddenY,
						(Time.time - model.statusBarToggledTime) * 3),
				rect.width,
				rect.height);

			GUI.DrawTexture (rect, statusBarTexture);

			GUI.Label (textRect (), model.getCurrentMessage (), textStyle);
		}

		Rect textRect ()
		{
			return new Rect (
				rect.x + rect.width * .05f,
				rect.y + rect.height * .30f,
				rect.width * .8f,
				rect.height * .8f);
		}
	}
}