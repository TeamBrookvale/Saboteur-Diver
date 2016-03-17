/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using UnityEngine;
using System.Collections;

namespace Soomla.Store {

	/// <summary>
	/// This class represents an item in the market.
	/// <c>MarketItem</c> is only used for <c>PurchaseWithMarket</c> purchase type.
	/// </summary>
	public class MarketItem {

		/// <summary
		/// Each product in the catalog can be MANAGED, UNMANAGED, or SUBSCRIPTION.
		/// MANAGED means that the product can be purchased only once per user (such as a new level in
		/// a game). This purchase is remembered by the Market and can be restored if this
		/// application is uninstalled and then re-installed.
		/// UNMANAGED is used for products that can be used up and purchased multiple times (such as
		/// "gold coins"). It is up to the application to keep track of UNMANAGED products for the user.
		/// SUBSCRIPTION is just like MANAGED except that the user gets charged periodically (monthly
		/// or yearly).
		/// </summary>
		public enum Consumable{
			NONCONSUMABLE,
			CONSUMABLE,
			SUBSCRIPTION
		}

		/// <summary>
		/// The product id as defined in itunesconnect or Google Play
		/// </summary>
		public string ProductId;
		/// <summary>
		/// The type of the item associated with this item on itunesconnect or Google Play.
		/// </summary>
		public Consumable consumable;
		/// <summary>
		/// A default price for the item in case the fetching of information from the App Store or Google Play fails.
		/// </summary>
		public double Price;

		/** These variable will contain information about the item as fetched from the App Store or Google Play. **/
		public string MarketPriceAndCurrency;
		public string MarketTitle;
		public string MarketDescription;
		public string MarketCurrencyCode;
		public long MarketPriceMicros;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="productId">The id of the current item in the market.</param>
		/// <param name="consumable">The type of the current item in the market.</param>
		/// <param name="price">The actual $$ cost of the current item in the market.</param>
		public MarketItem(string productId, Consumable consumable, double price){
			this.ProductId = productId;
			this.consumable = consumable;
			this.Price = price;
		}

		/// <summary>
		/// Constructor.
		/// Generates an instance of <c>MarketItem</c> from a <c>JSONObject<c>.
		/// </summary>
		/// <param name="jsonObject">A <c>JSONObject</c> representation of the wanted 
		/// <c>MarketItem</c>.</param>
		public MarketItem(JSONObject jsonObject) {
			string keyToLook = "";
#if UNITY_IOS && !UNITY_EDITOR
			keyToLook = StoreJSONConsts.MARKETITEM_IOS_ID;
#elif UNITY_ANDROID && !UNITY_EDITOR
			keyToLook = StoreJSONConsts.MARKETITEM_ANDROID_ID;
#endif
			if (!string.IsNullOrEmpty(keyToLook) && jsonObject.HasField(keyToLook)) {
				ProductId = jsonObject[keyToLook].str;
			} else {
				ProductId = jsonObject[StoreJSONConsts.MARKETITEM_PRODUCT_ID].str;
			}
			Price = jsonObject[StoreJSONConsts.MARKETITEM_PRICE].n;
			int cOrdinal = System.Convert.ToInt32(((JSONObject)jsonObject[StoreJSONConsts.MARKETITEM_CONSUMABLE]).n);
			if (cOrdinal == 0) {
				this.consumable = Consumable.NONCONSUMABLE;
			} else if (cOrdinal == 1){
				this.consumable = Consumable.CONSUMABLE;
			} else {
				this.consumable = Consumable.SUBSCRIPTION;
			}

			if (jsonObject[StoreJSONConsts.MARKETITEM_MARKETPRICE]) {
				this.MarketPriceAndCurrency = jsonObject[StoreJSONConsts.MARKETITEM_MARKETPRICE].str;
			} else {
				this.MarketPriceAndCurrency = "";
			}
			if (jsonObject[StoreJSONConsts.MARKETITEM_MARKETTITLE]) {
				this.MarketTitle = jsonObject[StoreJSONConsts.MARKETITEM_MARKETTITLE].str;
			} else {
				this.MarketTitle = "";
			}
			if (jsonObject[StoreJSONConsts.MARKETITEM_MARKETDESC]) {
				this.MarketDescription = jsonObject[StoreJSONConsts.MARKETITEM_MARKETDESC].str;
			} else {
				this.MarketDescription = "";
			}
			if (jsonObject[StoreJSONConsts.MARKETITEM_MARKETCURRENCYCODE]) {
				this.MarketCurrencyCode = jsonObject[StoreJSONConsts.MARKETITEM_MARKETCURRENCYCODE].str;
			} else {
				this.MarketCurrencyCode = "";
			}
			if (jsonObject[StoreJSONConsts.MARKETITEM_MARKETPRICEMICROS]) {
				this.MarketPriceMicros = System.Convert.ToInt64(jsonObject[StoreJSONConsts.MARKETITEM_MARKETPRICEMICROS].n);
			} else {
				this.MarketPriceMicros = 0;
			}
		}

		/// <summary>
		/// Converts the current <c>MarketItem</c> to a <c>JSONObject</c>.
		/// </summary>
		/// <returns>A <c>JSONObject</c> representation of the current 
		/// <c>MarketItem</c>.</returns>
		public JSONObject toJSONObject() {
			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
			obj.AddField (Soomla.JSONConsts.SOOM_CLASSNAME, SoomlaUtils.GetClassName (this));
			obj.AddField(StoreJSONConsts.MARKETITEM_PRODUCT_ID, this.ProductId);
			obj.AddField(StoreJSONConsts.MARKETITEM_CONSUMABLE, (int)(consumable));
			obj.AddField(StoreJSONConsts.MARKETITEM_PRICE, (float)this.Price);

			obj.AddField(StoreJSONConsts.MARKETITEM_MARKETPRICE, this.MarketPriceAndCurrency);
			obj.AddField(StoreJSONConsts.MARKETITEM_MARKETTITLE, this.MarketTitle);
			obj.AddField(StoreJSONConsts.MARKETITEM_MARKETDESC, this.MarketDescription);
			obj.AddField(StoreJSONConsts.MARKETITEM_MARKETCURRENCYCODE, this.MarketCurrencyCode);
			obj.AddField(StoreJSONConsts.MARKETITEM_MARKETPRICEMICROS, (float)this.MarketPriceMicros);

			return obj;
		}

	}
}
