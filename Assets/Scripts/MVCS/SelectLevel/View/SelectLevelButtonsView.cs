using TeamBrookvale.Game;

using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using Soomla.Store;

namespace TeamBrookvale.SelectLevel
{
	public class SelectLevelButtonsView : View
	{
		// signals
		internal LoadLevelSignal loadLevelSignal = new LoadLevelSignal ();

		// size of the button and gaps between them in pixels
		float buttonSize, margin, buttonGapX, buttonGapY;

		// model storing the icons
		ISelectLevelModel m;

		// text GUI style
		GUIStyle textGUIStyle = new GUIStyle ();

		// unlock all levels iap purchased?
		bool isUnlockAllLevelsIAPpurchased;

		internal void init (ISelectLevelModel m)
		{
			this.m = m;
		
			if (IAPInit.isIAPInitialized)
				isUnlockAllLevelsIAPpurchased = StoreInventory.GetItemBalance ("unlock_all_levels") > 0;

			buttonSize = Screen.width * Const.SelectLevelButtonSizeXinScreenPercentage / 100;
			margin = Screen.width * Const.SelectLevelButtonMarginXinScreenPercentage / 100;
			buttonGapX = (Screen.width - 2 * margin - 5 * buttonSize) / (5-1); // 5 buttons per column
			buttonGapY = (Screen.height- 2 * margin - 4 * buttonSize) / (4-1); // 4 button per row

			textGUIStyle.font = m.AtwriterFont;
			textGUIStyle.alignment = TextAnchor.MiddleCenter;
			textGUIStyle.fontSize = TBUtil.CalcFontSize (20);
		}

		void OnGUI ()
		{
			for (int i = 0; i < Const.MaxLevelNumber + 1; i++)
			{
				Rect iconRect = new Rect (
					margin + (buttonSize + buttonGapX) * (i % 5),
					margin + (buttonSize + buttonGapY) * (i / 5),
					buttonSize,
					buttonSize);

				int levelNumber = i + 1;

				Texture2D currentIcon;

				// if any of the levels clicked apart from the return to main menu
				if (i < Const.MaxLevelNumber)
				{
					switch (PlayerPrefs.GetInt (TBUtil.LevelNumberToKey (levelNumber)))
					{
					// open
					case (int) PlayerPrefsLevelKey.Open: 		currentIcon = m.LevelIconBackground; break;

					// completed
					case (int) PlayerPrefsLevelKey.Completed:	currentIcon = m.LevelIconBackgroundCompleted; break;
					
					// locked
					case (int) PlayerPrefsLevelKey.Locked:		currentIcon = m.LevelIconLocked; break;

					default:
						currentIcon = null; 
						Debug.LogError ("This state should never be reached. PlayerPrefs Key: " +
						                TBUtil.LevelNumberToKey (levelNumber) + "   Value : " + 
						                PlayerPrefs.GetInt (TBUtil.LevelNumberToKey (levelNumber)));
						break;
					}

					// override locked if unlock all levels IAP has been purchased
					if (isUnlockAllLevelsIAPpurchased && currentIcon == m.LevelIconLocked)
						currentIcon = m.LevelIconBackground;

					// show the level icon and clickable if not locked or if in debug mode
					GUI.DrawTexture (iconRect, currentIcon);
					if (GUI.Button (iconRect, "", "")
					    	&& 
					    		((PlayerPrefs.GetInt (TBUtil.LevelNumberToKey (levelNumber)) != (int) PlayerPrefsLevelKey.Locked)
					 			|| isUnlockAllLevelsIAPpurchased))
					{
						loadLevelSignal.Dispatch (levelNumber);
					}

					// show the level number
					GUI.Label (iconRect, levelNumber.ToString(), textGUIStyle);
				}

				// if the return to main menu icon clicked
				else
				{
					GUI.DrawTexture (iconRect, m.LevelIconReturnMainMenu);
					if (GUI.RepeatButton (iconRect, "", ""))
					    Application.LoadLevel ("MainMenuScene");
				}


			}
		}
	}
}