using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBrookvale.UI
{
	public interface IInGameMenuModel
	{
		IDictionary<InGameMenuModel.Icon, Texture2D> iconDictionary {get;}
		IDictionary<InGameMenuModel.Icon, Texture2D> getCurrentIconDictionary ();

		IDictionary<InGameMenuModel.IAPButton, Texture2D> iapButtonDictionary {get;}
		IDictionary<InGameMenuModel.IAPButton, Texture2D> iapButtonInactiveDictionary {get;}

		InGameMenuModel.MenuState menuState {get;set;}
		bool isShowingIAPButtons {get;set;}

		Texture2D inAppPaymentThankYouIcon {get;}
		Texture2D getIAPButtonTexture (InGameMenuModel.IAPButton iapButtonType);
	}
}