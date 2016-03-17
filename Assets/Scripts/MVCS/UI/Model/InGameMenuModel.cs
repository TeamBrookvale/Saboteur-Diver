using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TeamBrookvale.UI
{
	public class InGameMenuModel : IInGameMenuModel
	{
		public enum Icon {
			InAppPayment,
			Menu,
			Retry,
			Resume
		}

		public enum IAPButton {
			UnlockAllLevels,
			MoreSmokeBombs
		}

		public enum MenuState {
			Pause,
			MissionAccomplished,
			MissionFailed
		}

		public bool isShowingIAPButtons {get;set;}

		public IDictionary<Icon, Texture2D> iconDictionary {get; private set;}
		public IDictionary<IAPButton, Texture2D> iapButtonDictionary {get; private set;}
		public IDictionary<IAPButton, Texture2D> iapButtonInactiveDictionary {get; private set;}
		public Texture2D inAppPaymentThankYouIcon {get; private set;}


		public MenuState menuState {get;set;}

		readonly string pathIcon = "UI/InGameMenu/InGameMenuIcon";
		readonly string pathIAP  = "UI/InGameMenu/InAppPayment";

		public InGameMenuModel ()
		{
			// Instantiate dictionary
			iconDictionary = new Dictionary<Icon, Texture2D>();
			iapButtonDictionary = new Dictionary<IAPButton, Texture2D> ();
			iapButtonInactiveDictionary = new Dictionary<IAPButton, Texture2D> ();

			// In game menu icons load
			foreach (Icon iconType in (Icon[]) Enum.GetValues(typeof(Icon)))
			{
				// load icon
				Texture2D icon = Resources.Load<Texture2D> (pathIcon + iconType.ToString());
				
				// if could not be loaded
				if (icon == null)
					Debug.LogError (pathIcon + iconType.ToString() + " could not be loaded.");
				
				// add it to the dictionary
				iconDictionary.Add(iconType, icon);
			}

			// In app payment buttons load
			foreach (IAPButton iapButtonType in (IAPButton[]) Enum.GetValues(typeof(IAPButton)))
			{
				// Active button

				// load icon
				Texture2D iapButton = Resources.Load<Texture2D> (pathIAP + iapButtonType.ToString());
				
				// if could not be loaded
				if (iapButton == null)
					Debug.LogError (pathIcon + iapButtonType.ToString() + " could not be loaded.");
				
				// add it to the dictionary
				iapButtonDictionary.Add(iapButtonType, iapButton);


				// Incative Button

				// load icon
				iapButton = Resources.Load<Texture2D> (pathIAP + iapButtonType.ToString() + "InActive");
				
				// if could not be loaded
				if (iapButton == null)
					Debug.LogError (pathIcon + iapButtonType.ToString() + "InActive could not be loaded.");
				
				// add it to the dictionary
				iapButtonInactiveDictionary.Add(iapButtonType, iapButton);
			}

			// load thank you button
			inAppPaymentThankYouIcon = Resources.Load<Texture2D> (pathIAP + "ThankYou");
			if (inAppPaymentThankYouIcon == null)
				Debug.LogError (pathIAP + "ThankYou could not be loaded.");
		}

		public IDictionary<InGameMenuModel.Icon, Texture2D> getCurrentIconDictionary ()
		{
			// if mission failed then only return three icons skipping the resume icon
			if (menuState == MenuState.MissionFailed)
				return iconDictionary.Where (
					kvp => kvp.Key != Icon.Resume)
						.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			// else return all icons
			else 
				return iconDictionary;
		}

		public Texture2D getIAPButtonTexture (IAPButton iapButtonType)
		{
			if (Soomla.Store.StoreInventory.GetItemBalance ((iapButtonType == IAPButton.MoreSmokeBombs ? "more_smokebombs" : "unlock_all_levels")) == 0)
				return iapButtonDictionary [iapButtonType];
			else
				return iapButtonInactiveDictionary [iapButtonType];
		}
	}
}