/// <summary>
/// This class handles business events, such as ingame purchases.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GA_Business 
{
	#region public methods

	public  void NewEvent(string eventName, string currency, int amount)
	{
		CreateNewEvent(eventName, currency, amount);
	}
	
	#endregion
	
	#region private methods
	
	/// <summary>
	/// Used for player purchases
	/// </summary>
	/// <param name="businessID">
	/// The business identifier. F.x. "Rocket Launcher Upgrade" <see cref="System.String"/>
	/// </param>
	/// <param name="currency">
	/// Abbreviation of the currency used for the transaction. F.x. USD (U.S. Dollars) <see cref="System.String"/>
	/// </param>
	/// <param name="amount">
	/// The value of the transaction in the lowest currency unit. F.x. if currency is USD then amount should be in cent <see cref="System.Int32"/>
	/// </param>
	private  void CreateNewEvent(string eventName, string currency, int amount)
	{
		Hashtable parameters = new Hashtable()
		{
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], eventName },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Currency], currency },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Amount], amount.ToString() },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], GA.SettingsGA.CustomArea.Equals(string.Empty)?Application.loadedLevelName:GA.SettingsGA.CustomArea }
		};
		
		GA_Queue.AddItem(parameters, GA_Submit.CategoryType.GA_Purchase, false);

		GA_AdSupport.ShowAdStatic(GA_AdSupport.GAEventType.Custom, GA_AdSupport.GAEventCat.Business, eventName);
		
		#if UNITY_EDITOR
		
		if (GA.SettingsGA.DebugAddEvent)
		{
			GA.Log("GA Business Event added: " + eventName + ", currency: " + currency + ", amount: " + amount, true);
		}
		
		#endif
	}
	
	#endregion
}