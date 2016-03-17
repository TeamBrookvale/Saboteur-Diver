/// <summary>
/// This class handles error and exception messages, and makes sure they are added to the Quality category 
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GA_Debug
{
	public bool SubmitErrors;
	public int MaxErrorCount;
	public bool SubmitErrorStackTrace;
	public bool SubmitErrorSystemInfo;
	
	private int _errorCount = 0;
	
	private static bool _showLogOnGUI = false;
	public List<string> Messages;
	
	/// <summary>
	/// If SubmitErrors is enabled on the GA object this makes sure that any exceptions or errors are submitted to the GA server
	/// </summary>
	/// <param name="logString">
	/// The message <see cref="System.String"/>
	/// </param>
	/// <param name="stackTrace">
	/// The exception stack trace <see cref="System.String"/>
	/// </param>
	/// <param name="type">
	/// The type of the log message (we only submit errors and exceptions to the GA server) <see cref="LogType"/>
	/// </param>
	public void HandleLog(string logString, string stackTrace, LogType type)
	{
		//Only used if the GA_DebugGUI script is added to the GA object (for testing)
		if (_showLogOnGUI)
		{
			if (Messages == null)
			{
				Messages = new List<string>();
			}
			Messages.Add(logString);
		}

		//We only submit exceptions and errors
        if (SubmitErrors && _errorCount < MaxErrorCount && type != LogType.Log)
		{
			_errorCount++;
			
			string lString = logString.Replace('"', '\'').Replace('\n', ' ').Replace('\r', ' ');
			string sTrace = stackTrace.Replace('"', '\'').Replace('\n', ' ').Replace('\r', ' ');
			
			string eventID = "Exception";
			
			if (SubmitErrorStackTrace)
			{
				SubmitError(eventID, lString + " " + sTrace, type);
			}
			else
			{
				SubmitError(eventID, null, type);
			}
			
			if (SubmitErrorSystemInfo)
			{
				List<Hashtable> systemspecs = GA.API.GenericInfo.GetGenericInfo(lString);
			
				foreach (Hashtable spec in systemspecs)
				{
					GA_Queue.AddItem(spec, GA_Submit.CategoryType.GA_Error, false);
				}
			}
		}
    }
	
	public void SubmitError(string eventName, string message, LogType type)
	{
		//GA.API.Quality.NewErrorEvent(eventName, message, target.x, target.y, target.z);
		
		GA_Error.SeverityType severity = GA_Error.SeverityType.info;
		switch (type)
		{
		case LogType.Assert:
			severity = GA_Error.SeverityType.info;
			break;
		case LogType.Error:
			severity = GA_Error.SeverityType.error;
			break;
		case LogType.Exception:
			severity = GA_Error.SeverityType.critical;
			break;
		case LogType.Log:
			severity = GA_Error.SeverityType.debug;
			break;
		case LogType.Warning:
			severity = GA_Error.SeverityType.warning;
			break;
		}
		
		GA.API.Error.NewErrorEvent(severity, message);
	}

	/// <summary>
	/// Used only for testing purposes
	/// </summary>
	public static void EnabledLog ()
	{
		_showLogOnGUI = true;
	}
}
