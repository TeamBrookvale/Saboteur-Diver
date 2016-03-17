#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class GA_Tracking
{
	private static string _publicTestKey = "22b6cdbae8239f60ada6086f38bc6ca3";
	private static string _privateTestKey = "ef81b1774ac0e8bf967da627f38ef562cd670093";

	private static string _userID = GA_GenericInfo.GetUserUUID();
	private static string _sessionID = GA_GenericInfo.GetSessionUUID();

	public static void Setup ()
	{
		EditorUserBuildSettings.activeBuildTargetChanged += BuildTargetChanged;
		BuildTargetChanged ();
		SendUserEvent();
	}

	public static void SendUserEvent ()
	{
		GA_Submit.CategoryType category = GA_Submit.CategoryType.GA_User;

		string os = "";
		string[] osSplit = SystemInfo.operatingSystem.Split(' ');
		if (osSplit.Length > 0)
			os = osSplit[0];

		#if UNITY_IPHONE
		bool idfa = CheckIDFA();
		#endif

		List<GA_Submit.Item> queue = new List<GA_Submit.Item>();
		Hashtable parameters = new Hashtable() {
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Platform], GA_GenericInfo.GetSystem() },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Device], SystemInfo.deviceModel },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Os], os },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.OsVersion], SystemInfo.operatingSystem },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Sdk], "GA Unity SDK " + GA_Settings.VERSION },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.InstallPublisher], "UnityVersion:" + UnityEditorInternal.InternalEditorUtility.GetFullUnityVersion() },
			#if UNITY_IPHONE
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.InstallSite], "IDFA:"+idfa.ToString() },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.InstallCampaign], "iAD:" + EditorPrefs.GetBool("GA_IAD"+"-"+Application.dataPath, GA_Settings.IAD_DEFAULT) },
			#endif
			#if UNITY_IPHONE || UNITY_ANDROID
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.InstallAdgroup], "Chartboost:" + EditorPrefs.GetBool("GA_CB"+"-"+Application.dataPath, GA_Settings.CB_DEFAULT) },
			#endif
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], _userID },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], _sessionID },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA_Settings.VERSION }
		};
		queue.Add(new GA_Submit.Item() { AddTime = 0, Count = 1, Type = category, Parameters = parameters});
		Dictionary<GA_Submit.CategoryType, List<GA_Submit.Item>> dict = new Dictionary<GA_Submit.CategoryType, List<GA_Submit.Item>>();
		dict.Add(category, queue);
		
		WWW www = GetEventWWW(category, parameters);
		GA_ContinuationManager.StartCoroutine(GA_Submit.SendWWW(www, Submitted, SubmitError, true, "", "", queue), () => www.isDone);
	}

	public static void SendEvent (string eventID)
	{
		GA_Submit.CategoryType category = GA_Submit.CategoryType.GA_Event;
		
		List<GA_Submit.Item> queue = new List<GA_Submit.Item>();
		Hashtable parameters = new Hashtable() {
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], eventID },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], "Unity Editor" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], _userID },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], _sessionID },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA_Settings.VERSION }
		};
		queue.Add(new GA_Submit.Item() { AddTime = 0, Count = 1, Type = category, Parameters = parameters});
		Dictionary<GA_Submit.CategoryType, List<GA_Submit.Item>> dict = new Dictionary<GA_Submit.CategoryType, List<GA_Submit.Item>>();
		dict.Add(category, queue);

		WWW www = GetEventWWW(category, parameters);
		GA_ContinuationManager.StartCoroutine(GA_Submit.SendWWW(www, Submitted, SubmitError, true, "", "", queue), () => www.isDone);
	}

	public static void BuildTargetChanged ()
	{
		SendEvent ("BuildPlatform:"+EditorUserBuildSettings.activeBuildTarget);
	}

	private static WWW GetEventWWW (GA_Submit.CategoryType category, Hashtable parameters)
	{
		string url = GA_Submit.GetURL(GA_Submit.Categories[category], _publicTestKey);
		string json = GA_Submit.DictToJson(new List<Hashtable>() { parameters });
		string jsonHash = GA_Submit.CreateMD5Hash(json + _privateTestKey);
		return GA_Submit.CreateSubmitWWW(url, json, jsonHash);
	}
	
	public static void EventSuccess (List<GA_Submit.Item> items)
	{
		List<GA_Submit.Item> queue = new List<GA_Submit.Item>();
		Hashtable parameters = new Hashtable() {
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], "DataSent" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], "Unity Editor" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], _userID },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], _sessionID },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA_Settings.VERSION }
		};
		queue.Add(new GA_Submit.Item() { AddTime = 0, Count = 1, Type = GA_Submit.CategoryType.GA_Event, Parameters = parameters});

		bool xyzData = false;
		foreach (GA_Submit.Item item in items)
		{
			if (!xyzData && (item.Parameters.ContainsKey("x") || item.Parameters.ContainsKey("y") || item.Parameters.ContainsKey("z")))
			{
				xyzData = true;
			}

			GA_Submit.CategoryType categoryType = item.Type;
			if (categoryType == GA_Submit.CategoryType.GA_Event || categoryType == GA_Submit.CategoryType.GA_Purchase || categoryType == GA_Submit.CategoryType.GA_Error)
			{
				Hashtable parametersCat = new Hashtable() {
					{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], "DataSent:Type:" + GA_Submit.Categories[categoryType]},
					{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], "Unity Editor" },
					{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], _userID },
					{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], _sessionID },
					{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA_Settings.VERSION }
				};
				queue.Add(new GA_Submit.Item() { AddTime = 0, Count = 1, Type = GA_Submit.CategoryType.GA_Event, Parameters = parametersCat});
			}
		}

		if (xyzData)
		{
			Hashtable parametersXYZ = new Hashtable() {
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], "DataSent:Heatmap" },
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], "Unity Editor" },
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], _userID },
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], _sessionID },
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA_Settings.VERSION }
			};
			queue.Add(new GA_Submit.Item() { AddTime = 0, Count = 1, Type = GA_Submit.CategoryType.GA_Event, Parameters = parametersXYZ});
		}

		Dictionary<GA_Submit.CategoryType, List<GA_Submit.Item>> dict = new Dictionary<GA_Submit.CategoryType, List<GA_Submit.Item>>();
		dict.Add(GA_Submit.CategoryType.GA_Event, queue);
		GA.API.Submit.SubmitQueue(queue, Submitted, SubmitError, true, _publicTestKey, _privateTestKey);
	}

	private static bool CheckIDFA ()
	{
		StreamReader reader = new StreamReader (Application.dataPath + "/Plugins/iOS/GA_UserID.mm");
		string content = reader.ReadLine ();
		reader.Close ();

		if (content.Contains("AdSupport"))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	private static void Submitted(List<GA_Submit.Item> items, bool success)
	{
		//Debug.Log("submit success " + items.Count + " " + success);
	}
	
	private static void SubmitError(List<GA_Submit.Item> items)
	{
		//Debug.Log("submit error " + items.Count);
	}
}

#endif