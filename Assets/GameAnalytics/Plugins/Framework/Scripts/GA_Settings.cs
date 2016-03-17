#define IOS_ID

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// The GA_Settings object contains an array of options which allows you to customize your use of GameAnalytics. Most importantly you will need to fill in your Game Key and Secret Key on the GA_Settings object to use the service.
/// </summary>
/// 
public class GA_Settings : ScriptableObject
{
	#if UNITY_IPHONE && !UNITY_EDITOR && IOS_ID
	[DllImport ("__Internal")]
	private static extern string GetUserID ();
	#endif
	
	/// <summary>
	/// Types of help given in the help box of the GA inspector
	/// </summary>
	public enum HelpTypes { None, IncludeSystemSpecsHelp, ProvideCustomUserID };
	public enum MessageTypes { None, Error, Info, Warning };
	
	/// <summary>
	/// A message and message type for the help box displayed on the GUI inspector
	/// </summary>
	public struct HelpInfo
	{
		public string Message;
		public MessageTypes MsgType;
		public HelpTypes HelpType;
	}
	
	#region public static values
	
	/// <summary>
	/// The version of the GA Unity Wrapper plugin
	/// </summary>
	[HideInInspector]
	public static string VERSION = "0.6.9";
	
	#endregion
	
	#region public values
	
	public int TotalMessagesSubmitted;
	public int TotalMessagesFailed;
	
	public int DesignMessagesSubmitted;
	public int DesignMessagesFailed;
	public int QualityMessagesSubmitted;
	public int QualityMessagesFailed;
	public int ErrorMessagesSubmitted;
	public int ErrorMessagesFailed;
	public int BusinessMessagesSubmitted;
	public int BusinessMessagesFailed;
	public int UserMessagesSubmitted;
	public int UserMessagesFailed;
	
	public string CustomArea = string.Empty;
	
	[SerializeField]
	public string GameKey = "";
	[SerializeField]
	public string SecretKey = "";
	[SerializeField]
	public string Build = "0.1";

	public bool SignUpOpen = true;
	public string FirstName = "";
	public string LastName = "";
	public string StudioName = "";
	public string GameName = "";
	public string PasswordConfirm = "";
	public bool EmailOptIn = true;
	public string EmailGA = "";
	[System.NonSerialized]
	public string PasswordGA = "";
	[System.NonSerialized]
	public string TokenGA = "";
	[System.NonSerialized]
	public string ExpireTime = "";
	[System.NonSerialized]
	public string LoginStatus = "Not logged in.";
	[System.NonSerialized]
	public int SelectedStudio = 0;
	[System.NonSerialized]
	public int SelectedGame = 0;
	[System.NonSerialized]
	public bool JustSignedUp = false;
	[System.NonSerialized]
	public bool HideSignupWarning = false;

	public bool IntroScreen = true;

	[System.NonSerialized]
	public List<Studio> Studios;

	public bool DebugMode = true;
	public bool DebugAddEvent = false;
	public bool SendExampleGameDataToMyGame = false;
	public bool RunInEditorPlayMode = true;
	public bool UseBundleVersion = false;
	
	public bool AllowRoaming = true;
	public bool ArchiveData = true;
	public bool NewSessionOnResume = true;
	public bool AutoSubmitUserInfo = true;
	public bool DelayQuitToSendData = true;

	//bytes
	public long ArchiveMaxFileSize = 2000;
	public bool CustomUserID;
	public float SubmitInterval = 10;
	
	public bool InternetConnectivity;

	//ad support
	public bool Start_AlwaysShowAds = true;
	public bool Start_TimePlayed = false;
	public bool Start_Sessions = false;
	
	public int TimePlayed = 300;
	public int Sessions = 1;
	
	public bool Trigger_AdsEnabled = false;
	public GA_AdSupport.GAAdNetwork Trigger_AdsEnabled_network = GA_AdSupport.GAAdNetwork.Any;
	
	public bool Trigger_SceneChange = true;
	public GA_AdSupport.GAAdNetwork Trigger_SceneChange_network = GA_AdSupport.GAAdNetwork.Any;
	
	public bool IAD_foldout = true;
	public static bool IAD_DEFAULT = true;
	public static bool CB_DEFAULT = false;
	public bool IAD_enabled = IAD_DEFAULT;
	public bool CB_enabled = CB_DEFAULT;
	#if (UNITY_IPHONE || UNITY_EDITOR) && (UNITY_4_9 || UNITY_4_8 || UNITY_4_7 || UNITY_4_6 || UNITY_4_5 || UNITY_4_3 || UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0 || UNITY_3_5 || UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0_0 || UNITY_3_0 || UNITY_2_6_1 || UNITY_2_6)
	public ADBannerView.Type IAD_type = ADBannerView.Type.Banner;
	public ADBannerView.Layout IAD_layout = ADBannerView.Layout.Top;
	#elif (UNITY_IPHONE || UNITY_EDITOR)
	public UnityEngine.iOS.ADBannerView.Type IAD_type = UnityEngine.iOS.ADBannerView.Type.Banner;
	public UnityEngine.iOS.ADBannerView.Layout IAD_layout = UnityEngine.iOS.ADBannerView.Layout.Top;
	#endif
	public Vector2 IAD_position = Vector2.zero;
	public float IAD_Duration = 10;

	public bool CB_foldout = true;
	//public string CB_appID;
	//public string CB_appSig;
	
	//These values are used for the GA_Inspector only
	public enum InspectorStates { Account, Basic, Debugging, Pref, Ads }
	public InspectorStates CurrentInspectorState;
	public List<HelpTypes> ClosedHints = new List<HelpTypes>();
	public bool DisplayHints;
	public Vector2 DisplayHintsScrollState;
	public Texture2D Logo;
	public Texture2D UpdateIcon;
	[System.NonSerialized]
	public GUIStyle SignupButton;
	
	[SerializeField]
	public List<GA_CustomAdTrigger> CustomAdTriggers = new List<GA_CustomAdTrigger>();

	#endregion
	
	#region public methods
	
	/// <summary>
	/// Help/hints/tips messages used for the GA inspector hints box. This function decides which hint to display.
	/// Garbos: Depricated because: Was duplicated to return list of messages instead. Use GetHelpMessageList
	/// </summary>
	/// <returns>
	/// The help message.
	/// </returns>
	public List<HelpInfo> GetHelpMessageList()
	{
		List<HelpInfo> result = new List<HelpInfo>();
		
		if (GameKey.Equals("") || SecretKey.Equals(""))
		{
			result.Add( new HelpInfo() { Message = "Please fill in your Game Key and Secret Key, obtained from the GameAnalytics website where you created your game.", MsgType = MessageTypes.Warning, HelpType = HelpTypes.None });
		}
		
		if (Build.Equals(""))
		{
			result.Add( new HelpInfo() { Message = "Please fill in a name for your build, representing the current version of the game. Updating the build name for each version of the game will allow you to filter by build when viewing your data on the GA website.", MsgType = MessageTypes.Warning, HelpType = HelpTypes.None });
		}
		
		if (CustomUserID && !ClosedHints.Contains(HelpTypes.ProvideCustomUserID))
		{
			result.Add( new HelpInfo() { Message = "You have indicated that you will provide a custom user ID - no data will be submitted until it is provided. This should be defined from code through: GA.Settings.SetCustomUserID", MsgType = MessageTypes.Info, HelpType = HelpTypes.ProvideCustomUserID });
		}
		
		return result;
	}
	
	/// <summary>
	/// Help/hints/tips messages used for the GA inspector hints box. This function decides which hint to display.
	/// Garbos: Depricated because: Was duplicated to return list of messages instead. Use GetHelpMessageList
	/// </summary>
	/// <returns>
	/// The help message.
	/// </returns>
	public HelpInfo GetHelpMessage()
	{
		if (GameKey.Equals("") || SecretKey.Equals(""))
		{
			return new HelpInfo() { Message = "Please fill in your Game Key and Secret Key, obtained from the GameAnalytics website where you created your game.", MsgType = MessageTypes.Warning, HelpType = HelpTypes.None };
		}
		else if (Build.Equals(""))
		{
			return new HelpInfo() { Message = "Please fill in a name for your build, representing the current version of the game. Updating the build name for each version of the game will allow you to filter by build when viewing your data on the GA website.", MsgType = MessageTypes.Warning, HelpType = HelpTypes.None };
		}
		
		else if (CustomUserID && !ClosedHints.Contains(HelpTypes.ProvideCustomUserID))
		{
			return new HelpInfo() { Message = "You have indicated that you will provide a custom user ID - no data will be submitted until it is provided. This should be defined from code through: GA.Settings.SetCustomUserID", MsgType = MessageTypes.Info, HelpType = HelpTypes.ProvideCustomUserID };
		}
		
		return new HelpInfo() { Message = "No hints to display. The \"Reset Hints\" button resets closed hints.", MsgType = MessageTypes.None, HelpType = HelpTypes.None };
	}
	
	/// <summary>
	/// Checks the internet connectivity, and sets INTERNETCONNECTIVITY
	/// </summary>
	public IEnumerator CheckInternetConnectivity(bool startQueue)
	{
		// Application.internetReachability returns the type of Internet reachability currently possible on the device, but does not check if the there is an actual route to the network. So we can check this instantly.
		if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork && !GA.SettingsGA.AllowRoaming)
		{
			InternetConnectivity = false;
		}
		else
		{
			//Try to ping the server
			WWW www = new WWW(GA_Submit.GetBaseURL(true) + "/ping");
				
			//Wait for response
			yield return www;
			
			try
			{
				if (GA_Submit.CheckServerReply(www))
				{
					InternetConnectivity = true;
				}
				else if (!string.IsNullOrEmpty(www.error))
				{
					InternetConnectivity = false;
				}
				else
				{
					//Get the JSON object from the response
					Hashtable returnParam = (Hashtable)GA_MiniJSON.JsonDecode(www.text);
					
					//If the response contains the key "status" with the value "ok" we know that we are connected
					if (returnParam != null && returnParam.ContainsKey("status") && returnParam["status"].ToString().Equals("ok"))
					{
						InternetConnectivity = true;
					}
					else
					{
						InternetConnectivity = false;
					}
				}
			}
			catch
			{
				InternetConnectivity = false;
			}
		}
		
		if (startQueue)
		{
			if (InternetConnectivity)
				GA.Log("GA has confirmed connection to the server..");
			else
				GA.Log("GA has no connection to the server..");
			
			//Try to add additional IDs
			if (AddUniqueIDs())
			{
				#if UNITY_EDITOR

				//Start the submit queue for sending messages to the server
				GA.RunCoroutine(GA_Queue.SubmitQueue());
				GA.Log("GameAnalytics: Submission queue started.");

				//GameObject gaTracking = new GameObject("GA_Tracking");
				//gaTracking.AddComponent<GA_Tracking>();
				
				#else

				while (GA.SettingsGA.CustomUserID && GA.API.GenericInfo.UserID == string.Empty)
				{
					yield return new WaitForSeconds(5f);
				}

				GA_Queue.ForceSubmit();
				GameObject fbGameObject = new GameObject("GA_FacebookSDK");
				fbGameObject.AddComponent<GA_FacebookSDK>();
				
				#endif
			}
			else
			{
				GA.LogWarning("GA failed to add unique IDs and will not send any data. If you are using iOS or Android please see the readme file in the iOS/Android folder in the GameAnalytics/Plugins directory.");
			}
		}
	}
	
	private bool AddUniqueIDs()
	{
		bool returnValue = false;
		
		#if !UNITY_EDITOR && UNITY_WEBPLAYER
		
		if (Application.absoluteURL.StartsWith("http"))
		{
			Application.ExternalEval(
				"try{var __scr = document.createElement('script'); __scr.setAttribute('async', 'true'); __scr.type = 'text/javascript'; __scr.src = 'https://d17ay18sztndlo.cloudfront.net/resources/js/ga_sdk_tracking.js'; ((document.getElementsByTagName('head') || [null])[0] || document.getElementsByTagName('script')[0].parentNode).appendChild(__scr);}catch(e){}"
				);
		}
		
		#endif
		
		#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
		
		string device = "PC";
		
		#elif !UNITY_EDITOR
		
		string device = SystemInfo.deviceModel;
		
		#endif
		
		#if !UNITY_EDITOR
		
		string os = "";
		string[] osSplit = SystemInfo.operatingSystem.Split(' ');

			#if UNITY_IPHONE
			
			if (osSplit.Length > 0)
				os = "iOS " + osSplit[2].Substring(0, 1);
			
			#elif UNITY_ANDROID
			
			if (osSplit.Length > 0)
			{
				string[] osvSplit = osSplit[2].Split('.');
				os = osSplit[0] + " " + osvSplit[0] + "." + osvSplit[1];
			}
			
			#else
			
			if (osSplit.Length > 0)
				os = osSplit[0];
			
			#endif

		#endif

		#if UNITY_IPHONE && !UNITY_EDITOR && IOS_ID

		string os_minor = "";
		string[] osmSplit = SystemInfo.operatingSystem.Split(' ');

		if (osmSplit.Length > 0)
			os_minor = "iOS " + osmSplit[2];
		
		try
		{
			string iOSid = GetUniqueIDiOS();
			if (iOSid != null && iOSid != string.Empty)
			{
				if (iOSid.StartsWith("VENDOR-"))
					GA.API.User.NewUser(GA_User.Gender.Unknown, null, null, null, null, AutoSubmitUserInfo?GA_GenericInfo.GetSystem():null, AutoSubmitUserInfo?device:null, AutoSubmitUserInfo?os:null, AutoSubmitUserInfo?os_minor:null, "GA Unity SDK " + VERSION, null);
				else
					GA.API.User.NewUser(GA_User.Gender.Unknown, null, null, iOSid, null, AutoSubmitUserInfo?GA_GenericInfo.GetSystem():null, AutoSubmitUserInfo?device:null, AutoSubmitUserInfo?os:null, AutoSubmitUserInfo?os_minor:null, "GA Unity SDK " + VERSION, null);
				
				returnValue = true;
			}
		}
		catch
		{ }
		
		#elif UNITY_ANDROID && !UNITY_EDITOR

		string os_minor = "";
		string[] osmSplit = SystemInfo.operatingSystem.Split(' ');
		
		if (osmSplit.Length > 0)
			os_minor = osmSplit[0] + " " + osmSplit[2];

		try
		{
			string androidAdID = GetAdvertisingIDAndroid();
			if (androidAdID != null && androidAdID != string.Empty)
			{
				GA.API.User.NewUser(GA_User.Gender.Unknown, null, null, null, null, AutoSubmitUserInfo?GA_GenericInfo.GetSystem():null, AutoSubmitUserInfo?device:null, AutoSubmitUserInfo?os:null, AutoSubmitUserInfo?os_minor:null, "GA Unity SDK " + VERSION, androidAdID);
				returnValue = true;
			}
		}
		catch
		{ }
		
		#elif UNITY_FLASH && !UNITY_EDITOR
		
		GA.API.User.NewUser(GA_User.Gender.Unknown, null, null, null, null, AutoSubmitUserInfo?GA_GenericInfo.GetSystem():null, "Flash", AutoSubmitUserInfo?os:null, AutoSubmitUserInfo?SystemInfo.operatingSystem:null, "GA Unity SDK " + VERSION, null);
		returnValue = true;
		
		#elif !UNITY_EDITOR && !UNITY_IPHONE && !UNITY_ANDROID
		
		GA.API.User.NewUser(GA_User.Gender.Unknown, null, null, null, null, AutoSubmitUserInfo?GA_GenericInfo.GetSystem():null, AutoSubmitUserInfo?device:null, AutoSubmitUserInfo?os:null, AutoSubmitUserInfo?SystemInfo.operatingSystem:null, "GA Unity SDK " + VERSION, null);
		returnValue = true;
		
		#elif UNITY_IPHONE && UNITY_EDITOR && !IOS_ID
		
		GetUniqueIDiOS ();
		returnValue = true;
		
		#elif UNITY_EDITOR
		
		returnValue = true;
		
		#endif
		
		return returnValue;
	}
	
	public string GetUniqueIDiOS ()
	{
		string uid = "";
		
		#if UNITY_IPHONE && !UNITY_EDITOR && IOS_ID
		uid = GetUserID();
		#endif
		
		#if UNITY_IPHONE && UNITY_EDITOR && !IOS_ID
		GA.LogWarning("GA Warning: Remember to read the iOS_Readme in the GameAnalytics > Plugins > iOS folder, for information on how to setup advertiser ID for iOS. GA will not work on iOS if you do not follow these steps.");
		#endif
		
		return uid;
	}
	
	public string GetAdvertisingIDAndroid ()
	{
		string uid = null;
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		try
		{
			AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject> ("currentActivity");
			AndroidJavaClass client = new AndroidJavaClass ("com.google.android.gms.ads.identifier.AdvertisingIdClient");
			AndroidJavaObject adInfo = client.CallStatic<AndroidJavaObject> ("getAdvertisingIdInfo",currentActivity);
			if (!adInfo.Call<bool> ("isLimitAdTrackingEnabled"))
			{
				uid = adInfo.Call<string> ("getId").ToString();
			}
		}
		catch
		{ 
			AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject> ("currentActivity");
			AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject> ("getContentResolver");
			AndroidJavaClass secure = new AndroidJavaClass ("android.provider.Settings$Secure");
			uid = secure.CallStatic<string> ("getString", contentResolver, "android_id");
		}
		#endif

		return uid;
	}
		
	/// <summary>
	/// Sets a custom user ID.
	/// Make sure each user has a unique user ID. This is useful if you have your own log-in system with unique user IDs.
	/// NOTE: Only use this method if you have enabled "Custom User ID" on the GA inspector!
	/// </summary>
	/// <param name="customID">
	/// The custom user ID - this should be unique for each user
	/// </param>
	public void SetCustomUserID(string customID)
	{
		if (customID != string.Empty)
		{
			GA.API.GenericInfo.SetCustomUserID(customID);
		}
	}
	
		
	/// <summary>
	/// Sets a custom area string. An area is often just a level, but you can set it to whatever makes sense for your game. F.x. in a big open world game you will probably need custom areas to identify regions etc.
	/// By default, if no custom area is set, the Application.loadedLevelName string is used.
	/// </summary>
	/// <param name="customID">
	/// The custom area.
	/// </param>
	public void SetCustomArea(string customArea)
	{
		CustomArea = customArea;
	}

	public void SetKeys (string gamekey, string secretkey)
	{
		GA.API.Submit.SetupKeys(gamekey, secretkey);
		GameKey = gamekey;
		SecretKey = secretkey;
	}

	#endregion
}

//[System.Serializable]
public class Studio
{
	public string Name { get; private set; }
	
	//[SerializeField]
	public List<string> Games { get; private set; }

	//[SerializeField]
	public List<string> Tokens { get; private set; }

	//[SerializeField]
	public List<int> GameIDs { get; private set; }

	public Studio (string name, List<string> games, List<string> tokens, List<int> ids)
	{
		Name = name;
		Games = games;
		Tokens = tokens;
		GameIDs = ids;
	}
	
	public static string[] GetStudioNames (List<Studio> studios)
	{
		if (studios == null)
		{
			return new string[] { "-" };
		}
		
		string[] names = new string[studios.Count + 1];
		names[0] = "-";

		string spaceAdd = "";
		for (int i = 0; i < studios.Count; i++)
		{
			names[i+1] = studios[i].Name + spaceAdd;
			spaceAdd += " ";
		}
		
		return names;
	}
	
	public static string[] GetGameNames (int index, List<Studio> studios)
	{
		if (studios == null || studios[index].Games == null)
		{
			return new string[] { "-" };
		}
		
		string[] names = new string[studios[index].Games.Count + 1];
		names[0] = "-";
		
		string spaceAdd = "";
		for (int i = 0; i < studios[index].Games.Count; i++)
		{
			names[i+1] = studios[index].Games[i] + spaceAdd;
			spaceAdd += " ";
		}
		
		return names;
	}
}