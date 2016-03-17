using UnityEngine;
using System.Collections;
using Soomla;
using Soomla.Store;
using System.Collections.Generic;

namespace TeamBrookvale.Game
{
	public class IAPInit : MonoBehaviour
	{
		public static bool isIAPInitialized = false;

		// Use this for initialization
		void Start () {

			if (isIAPInitialized)
			{
				Debug.LogError ("IAPInit should not be initialized more than once");
				return;
			}

			StoreEvents.OnMarketPurchaseStarted     += OnMarketPurchaseStarted;
			StoreEvents.OnMarketPurchase            += OnMarketPurchase;
			StoreEvents.OnItemPurchaseStarted       += OnItemPurchaseStarted;
			StoreEvents.OnItemPurchased             += OnItemPurchased;
			StoreEvents.OnSoomlaStoreInitialized	+= OnSoomlaStoreInitialized;
			StoreEvents.OnUnexpectedErrorInStore    += OnUnexpectedErrorInStore;
			StoreEvents.OnRestoreTransactionsFinished+=OnRestoreTransactionsFinished;

			SoomlaStore.Initialize (new IAPAssets());

			isIAPInitialized = true;
		}

		string s = "<nothing>";
		
		public void OnMarketPurchaseStarted (PurchasableVirtualItem pvi) {
			Debug.Log( "OnMarketPurchaseStarted: " + pvi.ItemId );
			s += "OnMarketPurchaseStarted: " + pvi.ItemId;

#if UNITY_EDITOR
			TeamBrookvale.Game.IAPMediator.InvokeIAPThankYou ();
#endif
		}
		
		public void OnMarketPurchase( PurchasableVirtualItem pvi, string s, Dictionary<string, string> d) {
			Debug.Log( "OnMarketPurchase: " + pvi.ItemId );
			s += "OnMarketPurchase: " + pvi.ItemId;

			TeamBrookvale.Game.IAPMediator.InvokeIAPThankYou ();
		}
		
		public void OnItemPurchaseStarted( PurchasableVirtualItem pvi ) {
			Debug.Log( "OnItemPurchaseStarted: " + pvi.ItemId );
			s += "OnItemPurchaseStarted: " + pvi.ItemId;
		}
		
		public void OnItemPurchased( PurchasableVirtualItem pvi, string s ) {
			Debug.Log( "OnItemPurchased: " + pvi.ItemId );
			s += "OnItemPurchased: " + pvi.ItemId;
		}
		
		public void OnSoomlaStoreInitialized( ) {
			Debug.Log( "OnSoomlaStoreInitialized" );
			s += "OnSoomlaStoreInitialized";
		}
		
		public void OnUnexpectedErrorInStore( string err ) {
			Debug.Log( "OnUnexpectedErrorInStore" + err );
			s += "OnUnexpectedErrorInStore" + err;
		}

		public void OnRestoreTransactionsStarted ()
		{
			Debug.Log ("Restore transactions started");
		}

		public void OnRestoreTransactionsFinished (bool isSuccessful)
		{
			Debug.Log ("Restoring transactions: " + isSuccessful);

			TeamBrookvale.NonMVCS.OptionsScript.RestoreIAPText =
				isSuccessful ?
					"Restoring in-app purchases successful" :
					"Restoring in-app purchases unsuccessful - Please try later";
		}
	}
}