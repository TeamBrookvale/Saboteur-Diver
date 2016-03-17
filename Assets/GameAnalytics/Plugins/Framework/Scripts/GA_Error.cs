/// <summary>
/// This class handles quality (QA) events, such as crashes, fps, etc.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GA_Error 
{
	public enum SeverityType { critical, error, warning, info, debug }
	
	#region public methods
	
	public void NewEvent(SeverityType severity, string message)
	{
		CreateNewEvent(severity, message, false);
	}
	
	public void NewErrorEvent(SeverityType severity, string message)
	{
		CreateNewEvent(severity, message, true);
	}
	
	#endregion
	
	#region private methods
	
	/// <summary>
	/// Used for player QA events
	/// </summary>
	/// <param name="businessID">
	/// The event identifier. F.x. "FailedToLoadLevel" <see cref="System.String"/>
	/// </param>
	/// <param name="message">
	/// A string detailing the event, F.x. the stack trace from an exception <see cref="System.String"/>
	/// </param>
	/// <param name="stack">
	/// If true any identical messages in the queue will be merged/stacked as a single message, to save server load
	/// </param>
	private  void CreateNewEvent(SeverityType severity, string message, bool stack)
	{
		Hashtable parameters = new Hashtable()
		{
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Severity], severity.ToString() },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Message], message },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], GA.SettingsGA.CustomArea.Equals(string.Empty)?Application.loadedLevelName:GA.SettingsGA.CustomArea }
		};
		
		GA_Queue.AddItem(parameters, GA_Submit.CategoryType.GA_Error, stack);
	}
	
	#endregion
}