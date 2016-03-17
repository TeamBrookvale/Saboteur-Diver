using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

namespace TeamBrookvale.Game
{
	
	/// <summary>
	/// This class defines our game's economy, which includes virtual goods, virtual currencies
	/// and currency packs, virtual categories
	/// </summary>
	public class IAPAssets : IStoreAssets
	{

		public const string iTunesConnectSharedSecretUnlockAllLevels = "a24c3c72f2dc4add9dd096443f930580";

		public int GetVersion() {
			return 0;
		}
		
		public VirtualCurrency[] GetCurrencies() {
			return new VirtualCurrency[]{};
		}
		
		public VirtualGood[] GetGoods() {
			return new VirtualGood[] { UNLOCK_ALL_LEVELS, MORE_SMOKEBOMBS };
		}
		
		public VirtualCurrencyPack[] GetCurrencyPacks() {
			return new VirtualCurrencyPack[] {};
		}
		
		public VirtualCategory[] GetCategories() {
			return new VirtualCategory[]{};
		}
		
		/*public VirtualGood[] GetLifetimeVGs() {
			return new VirtualGood [] { UNLOCK_ALL_LEVELS, MORE_SMOKEBOMBS };
		}*/
		
		/** Static Final members **/
		
		
		// declare App Store and Google Play ids
		public const string UNLOCK_ALL_LEVELS_PRODUCT_ID 	= "unlock_all_levels";
		public const string MORE_SMOKEBOMBS_ID       		= "more_smokebombs";

		/** Market MANAGED Items **/

		public static VirtualGood UNLOCK_ALL_LEVELS  = new LifetimeVG(
			"Unlock all levels",//Name
			"Unlock all levels in the game forever",//Description
			"unlock_all_levels",//id
			new PurchaseWithMarket(new MarketItem(UNLOCK_ALL_LEVELS_PRODUCT_ID, MarketItem.Consumable.NONCONSUMABLE , 0.99))
			);

		public static VirtualGood MORE_SMOKEBOMBS  = new LifetimeVG(
			"Buy more smokeboms",
			"Use 8 smokeboms / mission forever",
			"more_smokebombs",
			new PurchaseWithMarket(new MarketItem(MORE_SMOKEBOMBS_ID, MarketItem.Consumable.NONCONSUMABLE , 0.99))
			);

	}
}