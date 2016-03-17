using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using System;

namespace TeamBrookvale.UI
{
	public class InGameMenuIconsView : View
	{
		internal InGameMenuPushSignal _inGameMenuPushSignal = new InGameMenuPushSignal ();
		internal TeamBrookvale.Game.IAPInGameMenuSignal _iapInGameMenuSignal = new TeamBrookvale.Game.IAPInGameMenuSignal ();
		IInGameMenuModel model;
		InGameMenuModel.MenuState state;

		IDictionary<InGameMenuModel.Icon, Texture2D> currentIconDictionary;

		bool isShowingIcons;
		bool isShowingIAPThankYou;
		int iwh; // assuming all icons have the same size and they are all have 1:1 ratio resolutions
		readonly float w = Screen.width;
		readonly float h = Screen.height;
		readonly float gridX = .2f;	// distance between the buttons
		readonly float animLength = .5f; // length of the parabola animation in seconds
		readonly float overshoot = .1f; // overshooting in seconds
		float padX = 0;	// pad the buttons by zero if there are 4 buttons, .1 if there are only three
		float parabolaStartTime;

		float lastIconPushTime; // last icon  push time, only one button in each half a second
		float lastIAPButtonPushTime; // last IAP button push time, only one button in every five seconds

		internal void init (IInGameMenuModel model)
		{
			this.model = model;
	
			iwh = model.iconDictionary [0].width * Screen.height / 640;

		}

		internal void OnInGameMenuShowSignal (bool isShowing, InGameMenuModel.MenuState state)
		{
			parabolaStartTime = Time.realtimeSinceStartup;
			this.isShowingIcons = isShowing;
		}

		internal void OnIAPThankYouSignal ()
		{
			isShowingIAPThankYou = true;
		}

		void OnGUI()
		{
			GUI.depth = -1;

			// In game menu icons and IAP buttons
			if (isShowingIcons)
			{
				float buttonX = 0;

				currentIconDictionary = model.getCurrentIconDictionary();

				// pad the buttons by zero if there are 4 buttons, .1 if there are only three
				padX = currentIconDictionary.Count == 3 ? .1f : 0;

				foreach ( KeyValuePair<InGameMenuModel.Icon, Texture2D> icon in currentIconDictionary)
				{
					buttonX += gridX;

					GUI.DrawTexture (new Rect((padX + buttonX)*w - iwh/2 + 4000 * partParabola (), .7f * h, iwh,iwh), icon.Value);
					if (GUI.RepeatButton (new Rect((padX + buttonX)*w - iwh/2 + 4000 * partParabola (), .7f * h, iwh,iwh), "", ""))
					{
						// only one button in each half a second
						if (lastIconPushTime + .5f < Time.realtimeSinceStartup)
						{
							lastIconPushTime = Time.realtimeSinceStartup;
							_inGameMenuPushSignal.Dispatch (icon.Key);
						}
					}
				}


				// IAP buttons
				if (model.isShowingIAPButtons)
				{
					// currently not thanking for the purchase
					if (!isShowingIAPThankYou)
					{

						int buttonCounter = 0;

						foreach (InGameMenuModel.IAPButton iapButtonType in (InGameMenuModel.IAPButton[]) Enum.GetValues(typeof(InGameMenuModel.IAPButton)))
						{
							Texture2D iapButton = model.getIAPButtonTexture (iapButtonType);

							GUI.DrawTexture(
								new Rect (
								Screen.width / 2 - iapButton.width / 2  * Screen.height / 640,
								Screen.height * (.1f + .25f * buttonCounter),
								iapButton.width  * Screen.height / 640,
								iapButton.height * Screen.height / 640),
								iapButton);

							if (GUI.RepeatButton (
									new Rect (
									Screen.width / 2 - iapButton.width / 2  * Screen.height / 640,
									Screen.height * (.1f + .25f * buttonCounter),
										iapButton.width  * Screen.height / 640,
										iapButton.height * Screen.height / 640),
									"",
									""))
							{
								// only one button in every ten seconds a second
								if (lastIAPButtonPushTime + 10f < Time.realtimeSinceStartup)
								{
									lastIAPButtonPushTime = Time.realtimeSinceStartup;
									_iapInGameMenuSignal.Dispatch (iapButtonType);
								}
							}
							buttonCounter++;
						}
					}
					// currently thanking for the purchase, if clicked then disappears
					else
					{
							GUI.DrawTexture (
								new Rect (
								Screen.width / 2 - model.inAppPaymentThankYouIcon.width / 2 * Screen.height / 640,
								Screen.height * .4f - model.inAppPaymentThankYouIcon.height / 2 * Screen.height / 640,
								model.inAppPaymentThankYouIcon.width  * Screen.height / 640,
								model.inAppPaymentThankYouIcon.height * Screen.height / 640),
								model.inAppPaymentThankYouIcon);

							if (GUI.RepeatButton (
							new Rect (
							Screen.width / 2 - model.inAppPaymentThankYouIcon.width / 2 * Screen.height / 640,
							Screen.height * .4f - model.inAppPaymentThankYouIcon.height / 2 * Screen.height / 640,
							model.inAppPaymentThankYouIcon.width  * Screen.height / 640,
							model.inAppPaymentThankYouIcon.height * Screen.height / 640),
							"",
							""))
						{
							// only one button in each half a second
							if (lastIconPushTime + .5f < Time.realtimeSinceStartup)
							{
								lastIconPushTime = Time.realtimeSinceStartup;
								isShowingIAPThankYou = false;
							}
						}
					}
				}
			}
		}

		float partParabola ()
		{
			float timeElapsed = Mathf.Clamp (Time.realtimeSinceStartup - parabolaStartTime, 0, animLength);

			return	pow2 (animLength - overshoot - timeElapsed) - pow2 (overshoot);
		}

		float pow2 (float a) { return a * a; }
	}
}