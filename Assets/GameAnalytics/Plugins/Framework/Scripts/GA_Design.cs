// This class handles game design events, such as kills, deaths, high scores, etc.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GA_Design
{
	#region public methods
	
	/// <summary>
	/// Creates a new event
	/// </summary>
	/// <param name='eventName'>
	/// A event string you define
	/// </param>
	/// <param name='eventValue'>
	/// A value of the event, can be null
	/// </param>
	public void NewEvent(string eventName, float? eventValue)
	{
		CreateNewEvent(eventName, eventValue);
	}

	/// <summary>
	/// Creates a new event
	/// </summary>
	/// <param name='eventName'>
	/// A event string you define
	/// </param>
	public void NewEvent(string eventName)
	{
		CreateNewEvent(eventName, null);
	}
	
	#endregion
	
	#region private methods
	
	/// <summary>
	/// Adds a custom event to the submit queue (see GA_Queue)
	/// </summary>
	/// <param name="eventName">
	/// Identifies the event so this should be as descriptive as possible. PickedUpAmmo might be a good event name. EventTwo is a bad event name! <see cref="System.String"/>
	/// </param>
	/// <param name="eventValue">
	/// A value relevant to the event. F.x. if the player picks up some shotgun ammo the eventName could be "PickedUpAmmo" and this value could be "Shotgun". This can be null <see cref="System.Nullable<System.Single>"/>
	/// </param>
	/// <param name="x">
	/// The x coordinate of the event occurence. This can be null <see cref="System.Nullable<System.Single>"/>
	/// </param>
	/// <param name="y">
	/// The y coordinate of the event occurence. This can be null <see cref="System.Nullable<System.Single>"/>
	/// </param>
	private void CreateNewEvent(string eventName, float? eventValue)
	{
		Hashtable parameters = new Hashtable()
		{
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], eventName },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], GA.SettingsGA.CustomArea.Equals(string.Empty)?Application.loadedLevelName:GA.SettingsGA.CustomArea }
		};

		if (eventValue.HasValue)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Value], eventValue.ToString());
		}

		GA_Queue.AddItem(parameters, GA_Submit.CategoryType.GA_Event, false);

		GA_AdSupport.ShowAdStatic(GA_AdSupport.GAEventType.Custom, GA_AdSupport.GAEventCat.Design, eventName);
		
		#if UNITY_EDITOR
		
		if (GA.SettingsGA.DebugAddEvent)
		{
			string options = "";
			if (eventValue.HasValue)
			{
				options = ", value: " + eventValue;
			}

			GA.Log("GA Design Event added: " + eventName + options, true);
		}
		
		#endif
	}
	
	#endregion
}