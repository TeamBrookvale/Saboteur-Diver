/// <summary>
/// The inspector for the GA prefab.
/// </summary>

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System;

[CustomEditor(typeof(GA_Settings))]
public class GA_Inspector : Editor
{
	private GUIContent _homeLink				= new GUIContent("Home", "Opens your GameAnalytics home page on the GameAnalytics web page");
	private GUIContent _documentationLink		= new GUIContent("Support", "Opens the GameAnalytics Unity3D package documentation page in your browser.");
	private GUIContent _guideLink				= new GUIContent("Guide", "Opens the GameAnalytics Unity3D guide page.");
	//private GUIContent _createAccount			= new GUIContent("Sign Up", "Opens the GameAnalytics web page, and let's you create an account or login.");
	private GUIContent _switchToSignUp			= new GUIContent("Sign Up", "Create a GameAnalytics account, and start tracking data immediately!");
	private GUIContent _switchToLogin			= new GUIContent("Login", "Login to your GameAnalytics account to select your studio and game, and start tracking data immediately!");
	private GUIContent _publicKeyLabel			= new GUIContent("Game Key", "Your GameAnalytics Game Key - copy/paste from the GA website.");
	private GUIContent _privateKeyLabel			= new GUIContent("Secret Key", "Your GameAnalytics Secret Key - copy/paste from the GA website.");
	private GUIContent _firstNameLabel			= new GUIContent("First Name", "Your first name.");
	private GUIContent _lastNameLabel			= new GUIContent("Last Name", "Your last name (surname).");
	private GUIContent _studioNameLabel			= new GUIContent("Studio Name", "Your studio's name. You can add more studios and games on the GameAnalytics website.");
	private GUIContent _gameNameLabel			= new GUIContent("Game Name", "Your game's name. You can add more studies and games on the GameAnalytics website.");
	private GUIContent _passwordConfirmLabel	= new GUIContent("Confirm Pass", "Your GameAnalytics user account password.");
	private GUIContent _emailOptInLabel			= new GUIContent("Subscribe to release updates, news and tips and tricks.", "If enabled GameAnalytics may send you news about updates, cool tips and tricks, and other news to help you get the most out of our service.");
	private GUIContent _emailLabel				= new GUIContent("Email", "Your GameAnalytics user account email.");
	private GUIContent _passwordLabel			= new GUIContent("Password", "Your GameAnalytics user account password. Must be at least 8 characters in length.");
	private GUIContent _studiosLabel			= new GUIContent("Studio", "Studios tied to your GameAnalytics user account.");
	private GUIContent _gamesLabel				= new GUIContent("Game", "Games tied to the selected GameAnalytics studio.");
	private GUIContent _build					= new GUIContent("Build", "The current version of the game. Updating the build name for each test version of the game will allow you to filter by build when viewing your data on the GA website.");
	//private GUIContent _useBundleVersion		= new GUIContent("Use Bundle Version", "Uses the Bundle Version from Player Settings instead of the Build field above (only works for iOS, Android, and Blackberry).");
	private GUIContent _debugMode				= new GUIContent("Debug Mode", "Show additional debug messages from GA in the unity editor console when submitting data.");
	private GUIContent _debugAddEvent			= new GUIContent("Debug Add Event", "Shows additional debug information every time an event is added to the queue.");
	private GUIContent _sendExampleToMyGame		= new GUIContent("Get Example Game Data", "If enabled data collected while playing the example tutorial game will be sent to your game (using your game key and secret key). Otherwise data will be sent to a premade GA test game, to prevent it from polluting your data.");
	private GUIContent _runInEditor				= new GUIContent("Run In Editor Play Mode", "Submit data to the GameAnalytics server while playing your game in the Unity editor.");
	private GUIContent _customUserID			= new GUIContent("Custom User ID", "If enabled no data will be submitted until a custom user ID is provided. This is useful if you have your own log-in system, which ensures you have a unique user ID.");
	private GUIContent _interval				= new GUIContent("Data Submit Interval", "This option determines how often (in seconds) data is sent to GameAnalytics.");
	private GUIContent _allowRoaming			= new GUIContent("Submit While Roaming", "If enabled and using a mobile device (iOS or Android), data will be submitted to the GameAnalytics servers while the mobile device is roaming (internet connection via carrier data network).");
	private GUIContent _archiveData				= new GUIContent("Archive Data", "If enabled data will be archived when an internet connection is not available. When an internet connection is established again, any archived data will be sent. Not supported on: Webplayer, Google Native Client, Flash, Windows Store Apps.");
	private GUIContent _archiveMaxSize			= new GUIContent("Size<", "Indicates the maximum disk space used for archiving data in bytes.");
	private GUIContent _newSessionOnResume		= new GUIContent("New Session On Resume", "If enabled and using a mobile device (iOS or Android), a new play session ID will be generated whenever the game is resumed from background.");
	private GUIContent _account					= new GUIContent("Account", "This tab allows you to easily create a GameAnalytics account. You can also login to automatically retrieve your Game Key and Secret Key.");
	private GUIContent _basic					= new GUIContent("Setup", "This tab shows general options which are relevant for a wide variety of messages sent to GameAnalytics.");
	private GUIContent _debug					= new GUIContent("Debug", "This tab shows options which determine how the GameAnalytics SDK behaves in the Unity3D editor.");
	private GUIContent _preferences				= new GUIContent("Advanced", "This tab shows advanced and misc. options for the GameAnalytics SDK.");
	private GUIContent _ads						= new GUIContent("Ad Support", "This tab shows options for handling ads in your game.");
	private GUIContent _autoSubmitUserInfo		= new GUIContent("Auto Submit User Info", "If enabled information about platform, device, os, and os version is automatically submitted at the start of each session.");
	private GUIContent _delayQuitToSendData		= new GUIContent("Delay Quit", "If enabled the GameAnalytics SDK will cancel any Application.Quit events called, in order to send any remaining data. The application will automatically quit after the data is sent. This only works on PC/MAC.");

	private GUIStyle _orangeUpdateLabelStyle;
	private GUIStyle _orangeUpdateIconStyle;

	private GUIContent _startShowingAds			= new GUIContent("Ad Display Options", "When should your game start showing ads? If no conditions are enabled you must manually call the EnableAds() method to start showing ads.");
	private GUIContent _autoTriggers			= new GUIContent("Automatic Ad Triggers", "Automatic GA/Unity events which trigger ads.");
	private GUIContent _customTriggers			= new GUIContent("Custom Ad Triggers", "Custom GA events which trigger ads.");
	private GUIContent _startAlwaysShowAds		= new GUIContent("Always show ads", "Always show ads.");
	private GUIContent _startTimePlayed			= new GUIContent("Time Played", "Start showing ads after a period of time played (in seconds).");
	private GUIContent _startTime				= new GUIContent("Time:", "The number of seconds to wait until ads are enabled.");
	private GUIContent _startSessions			= new GUIContent("Sessions Played", "Start showing ads when the player returns after having played a number of sessions.");
	private GUIContent _startSes				= new GUIContent("Sessions:", "The number of sessions to wait until ads are enabled.");
	private GUIContent _triggerAdsEnabled		= new GUIContent("Ads Enabled", "Trigger an ad as soon as you want to start showing ads (enabled by the selected condition(s) under Start Showing Ads).");
	private GUIContent _triggerSceneChange		= new GUIContent("Level Change", "Trigger an ad when a new scene is loaded.");
	private GUIContent _triggerCustomCat		= new GUIContent("Category:", "The event category of the custom event which should trigger an ad.");
	private GUIContent _triggerCustomID			= new GUIContent("Event ID:", "The event ID (event name) of the custom event which should trigger an ad.");
	private GUIContent _triggerAdNetwork		= new GUIContent("Network:", "The ad network which should show an ad when the event is triggered. 'Any' will show an ad from one of the available ad networks.");
	private GUIContent _triggerAdNotEnabled		= new GUIContent("!", "You must enable the selected ad network, otherwise this event trigger will have no effect.");
	//private GUIContent _iAd					= new GUIContent("iAd:", "This fold out contains options for using iOS iAd banner ads.");
	private GUIContent _iAdenabled				= new GUIContent("iAd", "Enable/diable iOS iAd banner ads.");
	private GUIContent _iAdDuration				= new GUIContent("iAd Duration:", "The duration to show triggered iAds (in seconds).");
	private GUIContent _iAdtype					= new GUIContent("iAd Type:", "Type of the iOS iAd banner. Medium Rect only works on iPad with iOS 6+.");
	private GUIContent _iAdlayout				= new GUIContent("iAd Layout:", "Layout of the iOS iAd banner ads. Set layout to manual to customize position.");
	private GUIContent _iAdposition				= new GUIContent("iAd Position:", "Position of the iOS iAd banner ads using Unity GUI coords and conventions. Set layout to manual to customize position.");
	//private GUIContent _CB						= new GUIContent("Chartboost:", "This fold out contains options for using Chartboost ads.");
	private GUIContent _CBenabled				= new GUIContent("Chartboost", "Enable/diable Chartboost ads.");
	//#if !UNITY_ANDROID
	//private GUIContent _CBappID					= new GUIContent("App ID:", "Your App ID. You can find this in under your app in Chartboost.");
	//private GUIContent _CBappSig				= new GUIContent("App Signature:", "Your App Signature. You can find this in under your app in Chartboost.");
	//#endif

	//private static readonly Texture2D _triggerAdNotEnabledTexture = new Texture2D(1, 1);
	private static bool _checkedProjectNames = false;
	
	private const string _unityToken = "KKy7MQNc2TEUOeK0EMtR";
	
	void OnEnable()
	{
		GA_Settings ga = target as GA_Settings;
		
		if(ga.UpdateIcon == null)
		{
			ga.UpdateIcon = (Texture2D)Resources.LoadAssetAtPath("Assets/Gizmos/Images/update_orange.png", typeof(Texture2D));
		}

		if(ga.Logo == null)
		{
			ga.Logo = (Texture2D)Resources.LoadAssetAtPath("Assets/Gizmos/gaLogo.png", typeof(Texture2D));
			
			//http://www.base64-image.de
			/*String d = "";
			d += "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAABmJLR0QA/wD/AP+gvaeTAAAA";
			d += "CXBIWXMAAA7EAAAOxAGVKw4bAAAAB3RJTUUH3AcFDBE3zqglrQAACQdJREFUeNrtW2tIVGsXf";
			d += "mY7jcp4m+yoTdqYKWqFQtnlR2lFJGVxpEwIoThIHPDX15+v/oQdoiCC/DoQQSUxENSh7PJDHM";
			d += "wL6lEY00Og2dV7U15GHXHUue71/WjPsGf2zDR7nE7ZtGDjOPPenudda73vu961Jfh6IuEeBYC";
			d += "dADYDyAKQBiCJ+z6CK2sCMANgDMAAgNcA/gHwN/c9cc93L2EAwjmwFwG84A0+0OcF19Zmru2w";
			d += "7xG4DIAcQAWA524A2CWAd6/7nOtDzvX5zWUFgFgA/wUwHiTQ/pAxzvUZy43hm0g0gHIAg0Gcc";
			d += "bEaMciNIfrfBC4FsB1Azb8I/EtE1HBjkn5t8BEAfuO89bcA7ouIMW5sEV8LfAyASgBGXuffEj";
			d += "x5GIeRG2NMsMH/AuBPANbvYNa/pA1Wbqy/BBP8re9E5cWYxK1gkBDDsbkcwHsa459LMYcIzp6";
			d += "sywi8J3OoDMQxSjmPalyG4N1JMHJYRC2R23lL3XIE707CGIfJ7x1ezQ8A3p2EGn92jCu4rSX9";
			d += "gAQQh83n2SGWt7f/EcC7kzDIYXRxdvwj7e8AUnkBDdGiVCpx6NAh5OfnY+3atUhOTgYRQSKRY";
			d += "HZ2Fl1dXdDpdKirq4NWq3Wpu2bNGlRUVGDr1q1QqVSQSqVgGAY9PT0wGAy4desW2traAg3OgM";
			d += "P2O4D/AbC4F5LzjrSiZz8jI4PUajVNTk6S0Wgki8VCLMuSu5jNZlpcXKTZ2VkaGBigkpISZxt";
			d += "Hjx6l4eFhQR2bzUZWq5WqqqqCoQXjHFZBJKci0MYvX75M09PTZLVaBYN3kOCJDCKiM2fOONu5";
			d += "fv062Ww2l7r8eh8+fAiWSVS4R5bCeZEcUbN/9+5dMplMFIjMz8/TyZMnCQBFR0dTS0uLz/IWi";
			d += "4X27t0bDC14zmEGwxGwEUAe96Pftq9Wq1FaWorw8HAQiY9Z9vf3o7+/HwBQWFiItLQ0n+UZhs";
			d += "GePXuWGqglDutG/pcXxbJ57tw5MhqNAvXmq7zVaiWz2ezyWCwWstvtRERUXV1NERERBICqqqo";
			d += "8tuPeZm9vb7DM4CIAiZQjoEgMjenp6SgpKYFcLnd6eADOz3a7HUNDQ3j48CFqa2uxsLDweZmR";
			d += "ybBp0yYUFhYiPT0djY2NMJlMCA8PR0ZGBiQSibMNx2cAzvYlEgnWrVuHzMxMvHnzZqkHvSIA5";
			d += "wAgXqztX7t2zcVZuXvsuro6v9oJCwsjALR7927q6+sTzPjo6KigH7PZTGfPng3WviAeAH4VUz";
			d += "k9PZ06Ozu9qml9fb3oAZ0/f94joeXl5QIzs9vt1N7eHiwz+BUA/hBT6dSpU6TX673Ofmpqqqh";
			d += "ByOVyevTokYDQ1tZWSkhIoImJCQHJMzMzlJycHAwC/mC46yq/ZePGjYiPj/f4W11dHYaGhkQZ";
			d += "Yl5eHrKyspw27rB7jUaDiYkJ9Pb2gmVZF18gk8lw5MiRYES7shjurs6/CElEBFQqldPhuUttb";
			d += "a1zkP7KypUrYbPZoNfrwbKss357ezsAoLGx0UkAfxxFRUXBICBNyl1U+iWrV69GXFycy2zw5d";
			d += "WrVwJiJBIJFAoFEhMTBUCsVitYlsWTJ0+gUCiQnZ2NjIwMyGQyvH//HgDQ1NSEyspKZx0iAsM";
			d += "wyMnJQXx8PKamppZCQJKUu6X1r3RSEhQK78UHBgaEl4YyGUpLS1FWVgaz2Swgx2q1wmKxgGVZ";
			d += "6PV6rFq1Ct3d3VhcXAQAdHZ2Ym5uDgqFwrk8AkBkZCSKi4tRXV29FAIUAGDz12ns2rWLenp6v";
			d += "G5V5XK5oE5UVBTduXNH1Ba5vLycpFKps436+nrBWcJqtdK9e/eW6gRtjBi6GIbxaePetsMWi0";
			d += "XUtHR1dcFmszn/b2hoEAYtpVLk5OQgOnpp14IMl5zgl0xNTWFubs7r7ykpKZ434CIcY0dHh8C";
			d += "uGxoaXFYIx9+EhAQcPHhwKfhNDJeB4ZfMzMz4JCAzM1MAlr9V9qUx/OVvenrapdyLFy8wMzPj";
			d += "siUGgJiYGBQUFCyFgBmGi5j6JTqdDpOTk17VPS8vTwDWZDLh6tWrKC4uxuHDh1FUVITjx49Do";
			d += "9G4lHV81mq1zrODQ1iWRUdHh0cHm5ubC4ZhAiVgDAD+EuM4Ll265DW48fLlS7/aWL9+PTU1NQ";
			d += "nqv337lrKzsz3WOX36tMdAy+joKB04cCBQJ/gXg88JSX5LW1ubx+UOADZs2IBjx459sY3MzEz";
			d += "k5+cLvtdqtQL1d8izZ89cNM+hMfHx8di5c2egGvCawedsLL+ltbUVIyMjXm345s2b2L9/v/e4";
			d += "+4oV2LJlC8LCwgR1m5ubMT4+7rFeX18fBgcHBSYWGRmJvLy8QAn4h8HnVDTAzzS0+fl5PH36F";
			d += "AaDQXBmJyLExcXh/v37uHDhApKShJvM3Nxc7Nu3TzCbCwsLzuiQJ2FZFs3NzR6daEpKCrZt2y";
			d += "YGuAPr346lUFQ6W1RUFDU2Nnr0Aw7bNJvNNDs7S58+faIHDx6QRqOhjx8/ktFo9Bg87ejooKy";
			d += "sLJ/9lpWVeexzYWGBKisrA0m/YwIOieXn59PIyIjPiK9D7Ha7MwzmLVp85coVio6O9tlnYmKi";
			d += "1/5qamoCCok5VGJzIBHhgoIC0ul0HuN43gbqjSz+/YC3RyqVklar9dhed3c35eTkiIkGbeZHh";
			d += "V8C6OJFTf2SlpYWlJWV4fXr1y5HWV8bIL7fcMi7d++g0+m+2B/LsmhqavK4kVKpVNixY4c/ti";
			d += "/hsL4M2sVIZGQkqdVqMhgMZDabv2gS/KixIzqclJTkV1/bt2/3qkm3b98WfTEicbsaGwCQIPZ";
			d += "+gH+3d+LECRQXFyMtLQ1hYWHOBwDsdjvsdjsAYHx8HFqtFmq1WuDdfYlMJsPjx4+hVCqhVCpB";
			d += "RDCZTBgeHoZGo8GNGzdgMBh8zf4EFwSadydABuA/AC4HI9QSGxsLlUqF1NRUKJVK2Gw2jI2NY";
			d += "WBgAHq9HhMTE/hGcsbb5WhIXo//TJD4mSLzM0nKo4R8mhwQ4omSDgnpVFmHhHSyNJ+EkE2X55";
			d += "tDyL4wwXeMIfvKDH+JDNmXptx3jCH52pz72SFkX5yE21E6JF+ddZdl9/K05CuSsSxen/8/Sd3";
			d += "GJdqpTWgAAAAASUVORK5CYII=";
			ga.Logo = new Texture2D(1,1);
			ga.Logo.LoadImage(System.Convert.FromBase64String(d));*/
		}
		
		/*if (ga.UseBundleVersion)
		{
			ga.Build = PlayerSettings.bundleVersion;
		}*/
	}
	
	override public void OnInspectorGUI ()
	{
		GA_Settings ga = target as GA_Settings;
		
		EditorGUI.indentLevel = 1;
		EditorGUILayout.Space();
		
		if (ga.SignupButton == null)
		{
			GUIStyle signupButton = new GUIStyle(GUI.skin.button);
			signupButton.normal.background = (Texture2D)Resources.LoadAssetAtPath("Assets/Gizmos/Images/default.png", typeof(Texture2D));
			signupButton.active.background = (Texture2D)Resources.LoadAssetAtPath("Assets/Gizmos/Images/active.png", typeof(Texture2D));
			signupButton.normal.textColor = Color.white;
			signupButton.active.textColor = Color.white;
			signupButton.fontSize = 14;
			signupButton.fontStyle = FontStyle.Bold;
			ga.SignupButton = signupButton;
		}

		GUILayout.BeginHorizontal();

		GUILayout.Label(ga.Logo);
		
		GUILayout.BeginVertical();

		GUILayout.Label("GameAnalytics Unity SDK v." + GA_Settings.VERSION);
		
		GUILayout.BeginHorizontal();
		
		if (GUILayout.Button(_homeLink, GUILayout.MaxWidth(75)))
		{
			Application.OpenURL("http://easy.gameanalytics.com/LoginGA");
		}
		if (GUILayout.Button(_documentationLink, GUILayout.MaxWidth(75)))
		{
			Application.OpenURL("http://easy.gameanalytics.com/SupportDocu");
		}
		if (GUILayout.Button(_guideLink, GUILayout.MaxWidth(75)))
		{
			Application.OpenURL("http://easy.gameanalytics.com/guide_unity");
		}
		
		GUILayout.EndHorizontal();
		
		string updateStatus = GA_UpdateWindow.UpdateStatus(GA_Settings.VERSION);
		
		GUILayout.Space(5);
		GUILayout.BeginHorizontal();
		
		if (!updateStatus.Equals(string.Empty))
		{
			if (_orangeUpdateLabelStyle == null)
			{
				_orangeUpdateLabelStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
				_orangeUpdateLabelStyle.normal.textColor = new Color(0.875f, 0.309f, 0.094f);
			}
			if (_orangeUpdateIconStyle == null)
			{
				_orangeUpdateIconStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
			}
			
			if (GUILayout.Button(ga.UpdateIcon, _orangeUpdateIconStyle, GUILayout.MaxWidth(17)))
			{
				OpenUpdateWindow();
			}
			
			GUILayout.Label(updateStatus, _orangeUpdateLabelStyle);
		}
		else
			GUILayout.Label("");
		
		GUILayout.EndHorizontal();
		
		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal();
		
		
		//Hints
		/*ga.DisplayHints = EditorGUILayout.Foldout(ga.DisplayHints,"Show Hints");
		if (ga.DisplayHints)
		{
			ga.DisplayHintsScrollState = GUILayout.BeginScrollView(ga.DisplayHintsScrollState, GUILayout.Height (100));
		
			List<GA_Settings.HelpInfo> helpInfos = ga.GetHelpMessageList();
			foreach(GA_Settings.HelpInfo info in helpInfos)
			{
				MessageType msgType = ConvertMessageType(info.MsgType);
				EditorGUILayout.HelpBox(info.Message, msgType);
			}
		
			GUILayout.EndScrollView();
		}*/

		if (ga.IntroScreen)
		{
			if (GA.SettingsGA.GameKey.Length > 0 || GA.SettingsGA.SecretKey.Length > 0)
			{
				GA.SettingsGA.IntroScreen = false;
			}
			else
			{
				if (!_checkedProjectNames && !EditorPrefs.GetBool("GA_Installed"+"-"+Application.dataPath, false))
				{
					_checkedProjectNames = true;

					if (!PlayerSettings.companyName.Equals("DefaultCompany"))
					{
						GA.SettingsGA.StudioName = PlayerSettings.companyName;
					}
					if (!PlayerSettings.productName.StartsWith("New Unity Project"))
					{
						GA.SettingsGA.GameName = PlayerSettings.productName;
					}
					EditorPrefs.SetBool("GA_Installed"+"-"+Application.dataPath, true);
					Selection.activeObject = GA.SettingsGA;
				}

				GUILayout.Space(5);

				Splitter(new Color(0.35f, 0.35f, 0.35f));

				GUILayout.Space(10);

				GUIStyle largeWhiteStyle = new GUIStyle(EditorStyles.whiteLargeLabel);
				if (!Application.HasProLicense())
				{
					largeWhiteStyle = new GUIStyle(EditorStyles.largeLabel);
				}
				largeWhiteStyle.fontSize = 16;
				//largeWhiteStyle.fontStyle = FontStyle.Bold;
				
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label("Thank you for downloading!", largeWhiteStyle, new GUILayoutOption[] { GUILayout.Height(30) });
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(20);

				GUIStyle greyStyle = new GUIStyle(EditorStyles.label);
				greyStyle.fontSize = 12;
				
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label("Get started tracking your game by signing up to", greyStyle, new GUILayoutOption[] { GUILayout.Height(20) });
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(-5);

				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label("GameAnalytics for FREE.", greyStyle, new GUILayoutOption[] { GUILayout.Height(20) });
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(20);

				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Sign up", ga.SignupButton, new GUILayoutOption[] { GUILayout.Width(175), GUILayout.Height(40) } ))
				{
					ga.IntroScreen = false;
					ga.CurrentInspectorState = GA_Settings.InspectorStates.Account;
					ga.SignUpOpen = true;
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(15);

				Splitter(new Color(0.35f, 0.35f, 0.35f));

				GUILayout.Space(15);

				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label("Already have an account? Please login", greyStyle, new GUILayoutOption[] { GUILayout.Height(20) });
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(15);

				GUILayout.BeginHorizontal();
				//GUILayout.Label("", GUILayout.Width(3));
				GUILayout.Label(_emailLabel, GUILayout.Width(75));
				ga.EmailGA = EditorGUILayout.TextField("", ga.EmailGA);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				//GUILayout.Label("", GUILayout.Width(3));
				GUILayout.Label(_passwordLabel, GUILayout.Width(75));
				ga.PasswordGA = EditorGUILayout.PasswordField("", ga.PasswordGA);
				GUILayout.EndHorizontal();
				
				EditorGUILayout.Space();
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("", GUILayout.Width(90));
				if (GUILayout.Button("Login", new GUILayoutOption[] { GUILayout.Width(130), GUILayout.MaxHeight(30) }))
				{
					ga.IntroScreen = false;
					ga.SignUpOpen = false;
					ga.CurrentInspectorState = GA_Settings.InspectorStates.Account;

					ga.SelectedStudio = 0;
					ga.SelectedGame = 0;
					ga.Studios = null;
					SetLoginStatus ("Contacting Server..", ga);
					LoginUser(ga);
				}
				GUILayout.Label("", GUILayout.Width(10));
				GUILayout.BeginVertical();
				GUILayout.Space(8);
				if (GUILayout.Button("Forgot password?", EditorStyles.label, GUILayout.Width(105)))
				{
					Application.OpenURL("https://go.gameanalytics.com/login?showreset&email="+ga.EmailGA);
				}
				EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();

				GUILayout.Space(15);
				
				Splitter(new Color(0.35f, 0.35f, 0.35f));
				
				GUILayout.Space(15);

				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				/*GUILayout.Label("To manually input your Game Key and Secret Key click", GUILayout.Width(292));
				GUILayout.Space(-5);
				GUILayout.BeginVertical();
				GUILayout.Space(2);*/
				if (GUILayout.Button("Go to setup your game and secret keys manually", EditorStyles.label, GUILayout.Width(270)))
				{
					ga.IntroScreen = false;
					ga.CurrentInspectorState = GA_Settings.InspectorStates.Basic;
				}
				EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
				GUILayout.FlexibleSpace();
				//GUILayout.EndVertical();
				GUILayout.EndHorizontal();
			}
		}
		else
		{
			//Tabs
			GUILayout.BeginHorizontal();
			
			GUIStyle activeTabStyle = new GUIStyle(EditorStyles.miniButtonMid);
			GUIStyle activeTabStyleLeft = new GUIStyle(EditorStyles.miniButtonLeft);
			GUIStyle activeTabStyleRight = new GUIStyle(EditorStyles.miniButtonRight);
			
			activeTabStyle.normal = EditorStyles.miniButtonMid.active;
			activeTabStyleLeft.normal = EditorStyles.miniButtonLeft.active;
			activeTabStyleRight.normal = EditorStyles.miniButtonRight.active;
			
			GUIStyle inactiveTabStyle = new GUIStyle(EditorStyles.miniButtonMid);
			GUIStyle inactiveTabStyleLeft = new GUIStyle(EditorStyles.miniButtonLeft);
			GUIStyle inactiveTabStyleRight = new GUIStyle(EditorStyles.miniButtonRight);

			GUIStyle basicTabStyle = ga.CurrentInspectorState==GA_Settings.InspectorStates.Basic?activeTabStyleLeft:inactiveTabStyleLeft;

			if (ga.Studios == null)
			{
				if (GUILayout.Button(_account, ga.CurrentInspectorState==GA_Settings.InspectorStates.Account?activeTabStyleLeft:inactiveTabStyleLeft))
				{
					ga.CurrentInspectorState = GA_Settings.InspectorStates.Account;
				}

				basicTabStyle = ga.CurrentInspectorState==GA_Settings.InspectorStates.Basic?activeTabStyle:inactiveTabStyle;
			}

			if (GUILayout.Button(_basic, basicTabStyle))
			{
				ga.CurrentInspectorState = GA_Settings.InspectorStates.Basic;
			}
			
			if (GUILayout.Button(_debug, ga.CurrentInspectorState==GA_Settings.InspectorStates.Debugging?activeTabStyle:inactiveTabStyle))
			{
				ga.CurrentInspectorState = GA_Settings.InspectorStates.Debugging;
			}

			if (GUILayout.Button(_preferences,ga.CurrentInspectorState==GA_Settings.InspectorStates.Pref?activeTabStyle:inactiveTabStyle))
			{
				ga.CurrentInspectorState = GA_Settings.InspectorStates.Pref;
			}

			if (GUILayout.Button(_ads, ga.CurrentInspectorState==GA_Settings.InspectorStates.Ads?activeTabStyleRight:inactiveTabStyleRight))
			{
				ga.CurrentInspectorState = GA_Settings.InspectorStates.Ads;
			}

			GUILayout.EndHorizontal();

			if(ga.CurrentInspectorState == GA_Settings.InspectorStates.Account)
			{
				EditorGUILayout.Space();
				
				GUILayout.BeginHorizontal();
				
				if (ga.SignUpOpen)
				{
					GUILayout.Label("Sign up",EditorStyles.largeLabel);

					if (GUILayout.Button(_switchToLogin, GUILayout.MaxWidth(60)))
					{
						ga.SignUpOpen = false;
						//Application.OpenURL("http://easy.gameanalytics.com/LoginGA");
					}
				}
				else
				{
					GUILayout.Label("Login to GameAnalytics",EditorStyles.largeLabel);

					if (ga.Studios == null)
					{
						if (GUILayout.Button(_switchToSignUp, GUILayout.MaxWidth(60)))
						{
							ga.SignUpOpen = true;
							//Application.OpenURL("http://easy.gameanalytics.com/LoginGA");
						} 
					}
					else
					{
						//GUILayout.BeginHorizontal();
						float minW = 0;
						float maxW = 0;
						GUIContent email = new GUIContent(ga.EmailGA);
						EditorStyles.miniLabel.CalcMinMaxWidth(email, out minW, out maxW);
						GUILayout.Label(email, EditorStyles.miniLabel, GUILayout.MaxWidth(maxW));
						
						if (GUILayout.Button("Log out", GUILayout.MaxWidth(60)))
						{
							ga.SelectedStudio = 0;
							ga.SelectedGame = 0;
							ga.Studios = null;
							SetLoginStatus ("Not logged in.", ga);
						}
						//GUILayout.EndHorizontal();
					}
				}
				GUILayout.EndHorizontal();
				
				EditorGUILayout.Space();
				
				//Splitter(new Color(0.35f, 0.35f, 0.35f));
				
				if (ga.SignUpOpen)
				{
					GUILayout.Label("Sign up to GameAnalytics below, and start tracking data in no time!", EditorStyles.miniBoldLabel);
					
					EditorGUILayout.Space();
					
					GUILayout.BeginHorizontal();
					//GUILayout.Label("", GUILayout.Width(3));
					GUILayout.Label(_emailLabel, GUILayout.Width(75));
					ga.EmailGA = EditorGUILayout.TextField("", ga.EmailGA);
					GUILayout.EndHorizontal();
					
					GUILayout.BeginHorizontal();
					//GUILayout.Label("", GUILayout.Width(3));
					GUILayout.Label(_passwordLabel, GUILayout.Width(75));
					ga.PasswordGA = EditorGUILayout.PasswordField("", ga.PasswordGA);
					GUILayout.EndHorizontal();
					
					GUILayout.BeginHorizontal();
					//GUILayout.Label("", GUILayout.Width(3));
					GUILayout.Label(_passwordConfirmLabel, GUILayout.Width(75));
					ga.PasswordConfirm = EditorGUILayout.PasswordField("", ga.PasswordConfirm);
					GUILayout.EndHorizontal();
					
					EditorGUILayout.Space();
					
					GUILayout.BeginHorizontal();
					//GUILayout.Label("", GUILayout.Width(3));
					GUILayout.Label(_firstNameLabel, GUILayout.Width(75));
					ga.FirstName = EditorGUILayout.TextField("", ga.FirstName);
					GUILayout.EndHorizontal();
					
					GUILayout.BeginHorizontal();
					//GUILayout.Label("", GUILayout.Width(3));
					GUILayout.Label(_lastNameLabel, GUILayout.Width(75));
					ga.LastName = EditorGUILayout.TextField("", ga.LastName);
					GUILayout.EndHorizontal();
					
					EditorGUILayout.Space();
					
					GUILayout.BeginHorizontal();
					//GUILayout.Label("", GUILayout.Width(3));
					GUILayout.Label(_studioNameLabel, GUILayout.Width(75));
					ga.StudioName = EditorGUILayout.TextField("", ga.StudioName);
					GUILayout.EndHorizontal();
					
					GUILayout.BeginHorizontal();
					//GUILayout.Label("", GUILayout.Width(3));
					GUILayout.Label(_gameNameLabel, GUILayout.Width(75));
					ga.GameName = EditorGUILayout.TextField("", ga.GameName);
					GUILayout.EndHorizontal();
					EditorGUILayout.Space();
					
					GUILayout.BeginHorizontal();
					//GUILayout.Label("", GUILayout.Width(3));
					GUILayout.Label("", GUILayout.Width(-18));
					ga.EmailOptIn = EditorGUILayout.Toggle("", ga.EmailOptIn, GUILayout.Width(27));
					GUILayout.Label(_emailOptInLabel);
					GUILayout.EndHorizontal();
					
					EditorGUILayout.Space();
					
					GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					//GUILayout.Label("", GUILayout.Width(7));
					if (GUILayout.Button("Sign up", new GUILayoutOption[] { GUILayout.Width(130), GUILayout.MaxHeight(30) }))
					{
						SignupUser(ga);
					}
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
					
					EditorGUILayout.Space();
					
					GUILayout.BeginHorizontal();
					GUILayout.Label("By activating your account you agree with our", GUILayout.Width(248));
					GUILayout.Space(-5);
					GUILayout.BeginVertical();
					GUILayout.Space(2);
					if (GUILayout.Button("Terms of Service", EditorStyles.boldLabel, GUILayout.Width(105)))
					{
						Application.OpenURL("http://www.gameanalytics.com/terms.html");
					}
					EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
					GUILayout.EndVertical();
					GUILayout.EndHorizontal();
				}
				else
				{
					if (string.IsNullOrEmpty(ga.LoginStatus) || ga.LoginStatus.Equals("Not logged in."))
					{
						GUILayout.BeginHorizontal();
						//GUILayout.Label("", GUILayout.Width(3));
						GUILayout.Label("By logging in you will be able to retrieve your game's keys automatically.", EditorStyles.miniBoldLabel);
						GUILayout.EndHorizontal();
					}
					else if (!string.IsNullOrEmpty(ga.LoginStatus))
					{
						EditorGUILayout.Space();
						if (ga.JustSignedUp && !ga.HideSignupWarning)
						{
							GUILayout.BeginHorizontal();
							GUILayout.Label("", GUILayout.Width(-18));
							EditorGUILayout.HelpBox("Please be aware that our service might take a few minutes to get ready to receive events. Click here to open Integration Status to follow the progress as you start sending events.", MessageType.Warning);
							Rect r = GUILayoutUtility.GetLastRect();
							if (GUI.Button(r, "", EditorStyles.label))
							{
								Application.OpenURL("https://go.gameanalytics.com/login?token="+ga.TokenGA+"&exp="+ga.ExpireTime+"&goto=/game/"+ga.Studios[ga.SelectedStudio-1].GameIDs[ga.SelectedGame-1]+"/initialize");
							}
							EditorGUIUtility.AddCursorRect(r, MouseCursor.Link);
							if (GUILayout.Button("X"))
							{
								ga.HideSignupWarning = true;
							} 
							GUILayout.EndHorizontal();
							EditorGUILayout.Space();
						}
						GUILayout.BeginHorizontal();
						//GUILayout.Label("", GUILayout.Width(7));
						GUILayout.Label("Status", GUILayout.Width(88));
						GUILayout.Label(ga.LoginStatus);
						/*if (GUILayout.Button("Refresh", GUILayout.Width(60)))
						{
							ga.SelectedStudio = 0;
							ga.SelectedGame = 0;
							ga.Studios = null;
							SetLoginStatus ("Contacting Server..", ga);
							LoginUser(ga);
						}*/
						GUILayout.EndHorizontal();
					}
					
					EditorGUILayout.Space();
					
					if (ga.Studios == null)
					{
						GUILayout.BeginHorizontal();
						//GUILayout.Label("", GUILayout.Width(3));
						GUILayout.Label(_emailLabel, GUILayout.Width(75));
						ga.EmailGA = EditorGUILayout.TextField("", ga.EmailGA);
						GUILayout.EndHorizontal();
						
						GUILayout.BeginHorizontal();
						//GUILayout.Label("", GUILayout.Width(3));
						GUILayout.Label(_passwordLabel, GUILayout.Width(75));
						ga.PasswordGA = EditorGUILayout.PasswordField("", ga.PasswordGA);
						GUILayout.EndHorizontal();
						
						EditorGUILayout.Space();
						
						GUILayout.BeginHorizontal();
						GUILayout.Label("", GUILayout.Width(90));
						if (GUILayout.Button("Login", new GUILayoutOption[] { GUILayout.Width(130), GUILayout.MaxHeight(30) }))
						{
							ga.SelectedStudio = 0;
							ga.SelectedGame = 0;
							ga.Studios = null;
							SetLoginStatus ("Contacting Server..", ga);
							LoginUser(ga);
						}
						GUILayout.Label("", GUILayout.Width(10));
						GUILayout.BeginVertical();
						GUILayout.Space(8);
						if (GUILayout.Button("Forgot password?", EditorStyles.label, GUILayout.Width(105)))
						{
							Application.OpenURL("https://go.gameanalytics.com/login?showreset&email="+ga.EmailGA);
						}
						EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
						GUILayout.EndVertical();
						GUILayout.EndHorizontal();
					}
				}
			}
			else if(ga.CurrentInspectorState == GA_Settings.InspectorStates.Basic)
			{
				EditorGUILayout.Space();
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("GameAnalytics Setup",EditorStyles.largeLabel);

				if (ga.Studios != null)
				{
					float minW = 0;
					float maxW = 0;
					GUIContent email = new GUIContent(ga.EmailGA);
					EditorStyles.miniLabel.CalcMinMaxWidth(email, out minW, out maxW);
					GUILayout.Label(email, EditorStyles.miniLabel, GUILayout.MaxWidth(maxW));

					if (GUILayout.Button("Log out", GUILayout.MaxWidth(60)))
					{
						ga.CurrentInspectorState = GA_Settings.InspectorStates.Account;
						ga.SignUpOpen = false;
						ga.JustSignedUp = false;

						ga.SelectedStudio = 0;
						ga.SelectedGame = 0;
						ga.Studios = null;
						SetLoginStatus ("Not logged in.", ga);
					}
					GUILayout.EndHorizontal();
					EditorGUILayout.Space();
				}
				else
				{
					GUILayout.EndHorizontal();
				}

				EditorGUILayout.Space();

				if (!string.IsNullOrEmpty(ga.LoginStatus) && !ga.LoginStatus.Equals("Not logged in."))
				{
					if (ga.JustSignedUp && !ga.HideSignupWarning)
					{
						GUILayout.BeginHorizontal();
						GUILayout.Label("", GUILayout.Width(-18));
						EditorGUILayout.HelpBox("Please be aware that our service might take a few minutes to get ready to receive events. Click here to open Integration Status to follow the progress as you start sending events.", MessageType.Warning);
						Rect r = GUILayoutUtility.GetLastRect();
						if (GUI.Button(r, "", EditorStyles.label))
						{
							Application.OpenURL("https://go.gameanalytics.com/login?token="+ga.TokenGA+"&exp="+ga.ExpireTime+"&goto=/game/"+ga.Studios[ga.SelectedStudio-1].GameIDs[ga.SelectedGame-1]+"/initialize");
						}
						EditorGUIUtility.AddCursorRect(r, MouseCursor.Link);
						if (GUILayout.Button("X"))
						{
							ga.HideSignupWarning = true;
						}
						GUILayout.EndHorizontal();
						EditorGUILayout.Space();
					}
					GUILayout.BeginHorizontal();
					//GUILayout.Label("", GUILayout.Width(7));
					GUILayout.Label("Status", GUILayout.Width(88));
					GUILayout.Label(ga.LoginStatus);
					/*if (GUILayout.Button("Refresh", GUILayout.Width(60)))
						{
							ga.SelectedStudio = 0;
							ga.SelectedGame = 0;
							ga.Studios = null;
							SetLoginStatus ("Contacting Server..", ga);
							LoginUser(ga);
						}*/
					GUILayout.EndHorizontal();
				}
				
				if (ga.Studios != null && ga.Studios.Count > 0)
				{
					EditorGUILayout.Space();
					//Splitter(new Color(0.35f, 0.35f, 0.35f));

					GUILayout.BeginHorizontal();
					//GUILayout.Label("", GUILayout.Width(7));
					GUILayout.Label(_studiosLabel, GUILayout.Width(75));
					string[] studioNames = Studio.GetStudioNames(ga.Studios);
					if (ga.SelectedStudio >= studioNames.Length)
					{
						ga.SelectedStudio = 0;
					}
					int tmpSelectedStudio = ga.SelectedStudio;
					ga.SelectedStudio = EditorGUILayout.Popup("", ga.SelectedStudio, studioNames);
					if (tmpSelectedStudio != ga.SelectedStudio)
					{
						ga.SelectedGame = 0;
					}
					GUILayout.EndHorizontal();
					
					if (ga.SelectedStudio > 0)
					{
						if (tmpSelectedStudio != ga.SelectedStudio)
						{
							SelectStudio(ga.SelectedStudio, ga);
						}
						
						GUILayout.BeginHorizontal();
						//GUILayout.Label("", GUILayout.Width(7));
						GUILayout.Label(_gamesLabel, GUILayout.Width(75));
						string[] gameNames = Studio.GetGameNames(ga.SelectedStudio-1, ga.Studios);
						if (ga.SelectedGame >= gameNames.Length)
						{
							ga.SelectedGame = 0;
						}
						int tmpSelectedGame = ga.SelectedGame;
						ga.SelectedGame = EditorGUILayout.Popup("", ga.SelectedGame, gameNames);
						GUILayout.EndHorizontal();
						
						if (ga.SelectedStudio > 0 && tmpSelectedGame != ga.SelectedGame)
						{
							SelectGame(ga.SelectedGame, ga);
						}
					}
					else if (tmpSelectedStudio != ga.SelectedStudio)
					{
						SetLoginStatus ("Please select studio..", ga);
					}
				}
				
				EditorGUILayout.Space();
				
				GUILayout.BeginHorizontal();
			    //GUILayout.Label("", GUILayout.Width(7));
			    GUILayout.Label(_publicKeyLabel, GUILayout.Width(75));
				string tmpGameKey = ga.GameKey;
				ga.GameKey = EditorGUILayout.TextField("", ga.GameKey);
				if (ga.GameKey != tmpGameKey && ga.GameKey.Length > 0 && ga.SecretKey.Length > 0)
				{
					GA_Tracking.SendEvent("KeysInput");
				}
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
			    //GUILayout.Label("", GUILayout.Width(7));
			    GUILayout.Label(_privateKeyLabel, GUILayout.Width(75));
				string tmpSecretKey = ga.SecretKey;
				ga.SecretKey = EditorGUILayout.TextField("", ga.SecretKey);
				if (ga.SecretKey != tmpSecretKey && ga.GameKey.Length > 0 && ga.SecretKey.Length > 0)
				{
					GA_Tracking.SendEvent("KeysInput");
				}
				GUILayout.EndHorizontal();
				
				EditorGUILayout.Space();
			
				GUILayout.BeginHorizontal();
			    //GUILayout.Label("", GUILayout.Width(7));
			    GUILayout.Label(_build, GUILayout.Width(75));
				ga.Build = EditorGUILayout.TextField("", ga.Build);
				GUILayout.EndHorizontal();

				EditorGUILayout.Space();

				Splitter(new Color(0.35f, 0.35f, 0.35f));

				if (ga.Studios != null && ga.Studios.Count > 0 && ga.SelectedStudio > 0 && ga.SelectedGame > 0)
				{
					EditorGUILayout.Space();
					GUILayout.BeginHorizontal();
					GUILayout.Label("View", GUILayout.Width(90));
					if (GUILayout.Button("Integration Status"))
					{
						Application.OpenURL("https://go.gameanalytics.com/login?token="+ga.TokenGA+"&exp="+ga.ExpireTime+"&goto=/game/"+ga.Studios[ga.SelectedStudio-1].GameIDs[ga.SelectedGame-1]+"/initialize");
						//Application.OpenURL("https://go.gameanalytics.com/login?token="+ga.TokenGA+"&exp="+ga.ExpireTime+"&goto=/game/"+ga.Studios[ga.SelectedStudio-1].GameIDs[ga.SelectedGame-1]+"/dashboards/show/realtime");
					}
					if (GUILayout.Button("Game settings"))
					{
						Application.OpenURL("https://go.gameanalytics.com/game/"+ga.Studios[ga.SelectedStudio-1].GameIDs[ga.SelectedGame-1]+"/settings");
					}
					GUILayout.EndHorizontal();
				}

				#if UNITY_ANDROID

				EditorGUILayout.Space();
				
				if (ga.CB_enabled)
				{
					GUILayout.BeginHorizontal();
					GUILayout.Label("", GUILayout.Width(-18));
					EditorGUILayout.HelpBox("Ads are enabled (see the Ad Support tab).", MessageType.Info);
					GUILayout.EndHorizontal();
				}

				#endif

				#if UNITY_IPHONE

				EditorGUILayout.Space();

				GUILayout.BeginHorizontal();
				GUILayout.Label("", GUILayout.Width(-18));
				EditorGUILayout.HelpBox("Please refer to the iOS_Readme in the GameAnalytics/Plugins/iOS folder for information on how to setup the GA Unity SDK for iOS.", MessageType.Info);
				GUILayout.EndHorizontal();

				if (GA.SettingsGA.IAD_enabled || GA.SettingsGA.CB_enabled)
				{
					GUILayout.BeginHorizontal();
					GUILayout.Label("", GUILayout.Width(-18));
					EditorGUILayout.HelpBox("Ads are enabled (see the Ad Support tab).", MessageType.Info);
					GUILayout.EndHorizontal();
				}

				#endif

				/*GUILayout.BeginHorizontal();
			    GUILayout.Label("", GUILayout.Width(7));
			    GUILayout.Label(_useBundleVersion, GUILayout.Width(150));
				ga.UseBundleVersion = EditorGUILayout.Toggle("", ga.UseBundleVersion);
				GUILayout.EndHorizontal();*/
			}
			
			if(ga.CurrentInspectorState == GA_Settings.InspectorStates.Debugging)
			{
				EditorGUILayout.Space();
				
				GUILayout.BeginHorizontal();
			    //GUILayout.Label("", GUILayout.Width(7));
			    GUILayout.Label(_debugMode, GUILayout.Width(150));
				ga.DebugMode = EditorGUILayout.Toggle("", ga.DebugMode);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
			    //GUILayout.Label("", GUILayout.Width(7));
			    GUILayout.Label(_debugAddEvent, GUILayout.Width(150));
				ga.DebugAddEvent = EditorGUILayout.Toggle("", ga.DebugAddEvent);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
			    //GUILayout.Label("", GUILayout.Width(7));
			    GUILayout.Label(_runInEditor, GUILayout.Width(150));
			    ga.RunInEditorPlayMode = EditorGUILayout.Toggle("", ga.RunInEditorPlayMode);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
			    //GUILayout.Label("", GUILayout.Width(7));
			    GUILayout.Label(_sendExampleToMyGame, GUILayout.Width(150));
			    ga.SendExampleGameDataToMyGame = EditorGUILayout.Toggle("", ga.SendExampleGameDataToMyGame);
				GUILayout.EndHorizontal();
			}
			
			if(ga.CurrentInspectorState == GA_Settings.InspectorStates.Pref)
			{
				EditorGUILayout.Space();

				GUILayout.BeginHorizontal();
			    //GUILayout.Label("", GUILayout.Width(7));
			    GUILayout.Label(_interval, GUILayout.Width(150));
				
				int tmpTimer = 0;
				if (int.TryParse(EditorGUILayout.TextField(ga.SubmitInterval.ToString(), GUILayout.Width(38)), out tmpTimer))
				{
					ga.SubmitInterval = Mathf.Max(Mathf.Min(tmpTimer, 999), 1);
				}
				GUILayout.EndHorizontal();
				
				EditorGUILayout.Space();
				
				GUILayout.BeginHorizontal();
			    //GUILayout.Label("", GUILayout.Width(7));
			    GUILayout.Label(_customUserID, GUILayout.Width(150));
			    ga.CustomUserID = EditorGUILayout.Toggle("", ga.CustomUserID);
				GUILayout.EndHorizontal();
				
				EditorGUILayout.Space();
				
				GUILayout.BeginHorizontal();
			    //GUILayout.Label("", GUILayout.Width(7));
			    GUILayout.Label(_archiveData, GUILayout.Width(150));
			    ga.ArchiveData = EditorGUILayout.Toggle("", ga.ArchiveData, GUILayout.Width(36));
				GUI.enabled = ga.ArchiveData;
				GUILayout.Label(_archiveMaxSize, GUILayout.Width(40));
				
				int tmpMaxArchiveSize = 0;
				if (int.TryParse(EditorGUILayout.TextField(ga.ArchiveMaxFileSize.ToString(), GUILayout.Width(48)), out tmpMaxArchiveSize))
				{
					ga.ArchiveMaxFileSize = Mathf.Max(Mathf.Min(tmpMaxArchiveSize, 2000), 0);
				}
				
				GUI.enabled = true;
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
			    //GUILayout.Label("", GUILayout.Width(7));
			    GUILayout.Label(_allowRoaming, GUILayout.Width(150));
			    ga.AllowRoaming = EditorGUILayout.Toggle("", ga.AllowRoaming);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
			    //GUILayout.Label("", GUILayout.Width(7));
			    GUILayout.Label(_newSessionOnResume, GUILayout.Width(150));
			    ga.NewSessionOnResume = EditorGUILayout.Toggle("", ga.NewSessionOnResume);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
			    //GUILayout.Label("", GUILayout.Width(7));
			    GUILayout.Label(_autoSubmitUserInfo, GUILayout.Width(150));
			    ga.AutoSubmitUserInfo = EditorGUILayout.Toggle("", ga.AutoSubmitUserInfo);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				//GUILayout.Label("", GUILayout.Width(7));
				GUILayout.Label(_delayQuitToSendData, GUILayout.Width(150));
				ga.DelayQuitToSendData = EditorGUILayout.Toggle("", ga.DelayQuitToSendData);
				GUILayout.EndHorizontal();

				EditorGUILayout.Space();
				
			}

			if (ga.CurrentInspectorState == GA_Settings.InspectorStates.Ads)
			{
				EditorGUILayout.Space();

				GUILayout.BeginHorizontal();
				GUILayout.Label("GameAnalytics Ad Support",EditorStyles.largeLabel);
				if (GUILayout.Button(_documentationLink, GUILayout.MaxWidth(60)))
				{
					Application.OpenURL("http://support.gameanalytics.com/hc/en-us/articles/200841376-The-GA-Settings-object#ad");
				}
				GUILayout.EndHorizontal();

				GUILayout.Label("GameAnalytics Ad Support helps you show ads from different ad networks in your mobile games.", EditorStyles.miniLabel);

				EditorGUILayout.Space();
				Splitter(new Color(0.35f, 0.35f, 0.35f));

				//ga.IAD_foldout = EditorGUILayout.Foldout(ga.IAD_foldout, _iAd);

				GUILayout.BeginHorizontal();
				//GUILayout.Label("", GUILayout.Width(7));
				GUILayout.Label("", GUILayout.Width(-18));
				bool iad_enabled = EditorGUILayout.Toggle("", GA.SettingsGA.IAD_enabled, GUILayout.Width(27));
				if (iad_enabled != GA.SettingsGA.IAD_enabled)
				{
					EditorPrefs.SetBool("GA_IAD"+"-"+Application.dataPath, iad_enabled);
					GA.SettingsGA.IAD_enabled = iad_enabled;
				}
				GUILayout.Label(_iAdenabled, GUILayout.Width(150));
				GUILayout.EndHorizontal();
				
				if (GA.SettingsGA.IAD_enabled)
				{
					/*GUILayout.BeginHorizontal();
					GUILayout.Label("", GUILayout.Width(7));
					GUILayout.Label(_iAdenabled, GUILayout.Width(150));
					ga.IAD_enabled = EditorGUILayout.Toggle("", ga.IAD_enabled);
					GUILayout.EndHorizontal();
					
					GUI.enabled = ga.IAD_enabled;*/
					
					GUILayout.BeginHorizontal();
					GUILayout.Label("", GUILayout.Width(7));
					GUILayout.Label(_iAdDuration, GUILayout.Width(150));
					ga.IAD_Duration = EditorGUILayout.FloatField(ga.IAD_Duration, GUILayout.Width(60));
					ga.IAD_Duration = Mathf.Max(0, ga.IAD_Duration);
					GUILayout.EndHorizontal();
					
					GUILayout.BeginHorizontal();
					GUILayout.Label("", GUILayout.Width(7));
					GUILayout.Label(_iAdtype, GUILayout.Width(150));
					#if (UNITY_4_9 || UNITY_4_8 || UNITY_4_7 || UNITY_4_6 || UNITY_4_5 || UNITY_4_3 || UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0 || UNITY_3_5 || UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0_0 || UNITY_3_0 || UNITY_2_6_1 || UNITY_2_6)
					ga.IAD_type = (ADBannerView.Type)EditorGUILayout.EnumPopup(ga.IAD_type, GUILayout.Width(125));
					#else
					ga.IAD_type = (UnityEngine.iOS.ADBannerView.Type)EditorGUILayout.EnumPopup(ga.IAD_type, GUILayout.Width(125));
					#endif
					GUILayout.EndHorizontal();
					
					GUILayout.BeginHorizontal();
					GUILayout.Label("", GUILayout.Width(7));
					GUILayout.Label(_iAdlayout, GUILayout.Width(150));
					#if (UNITY_4_9 || UNITY_4_8 || UNITY_4_7 || UNITY_4_6 || UNITY_4_5 || UNITY_4_3 || UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0 || UNITY_3_5 || UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0_0 || UNITY_3_0 || UNITY_2_6_1 || UNITY_2_6)
					ga.IAD_layout = (ADBannerView.Layout)EditorGUILayout.EnumPopup(ga.IAD_layout, GUILayout.Width(125));
					#else
					ga.IAD_layout = (UnityEngine.iOS.ADBannerView.Layout)EditorGUILayout.EnumPopup(ga.IAD_layout, GUILayout.Width(125));
					#endif
					GUILayout.EndHorizontal();

					#if (UNITY_4_9 || UNITY_4_8 || UNITY_4_7 || UNITY_4_6 || UNITY_4_5 || UNITY_4_3 || UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0 || UNITY_3_5 || UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0_0 || UNITY_3_0 || UNITY_2_6_1 || UNITY_2_6)
					GUI.enabled = GA.SettingsGA.IAD_enabled && ga.IAD_layout == ADBannerView.Layout.Manual;
					#else
					GUI.enabled = GA.SettingsGA.IAD_enabled && ga.IAD_layout == UnityEngine.iOS.ADBannerView.Layout.Manual;
					#endif
					
					GUILayout.BeginHorizontal();
					GUILayout.Label("", GUILayout.Width(7));
					GUILayout.Label(_iAdposition, GUILayout.Width(150));
					ga.IAD_position = EditorGUILayout.Vector2Field("", ga.IAD_position, GUILayout.Width(125));
					GUILayout.EndHorizontal();
					
					GUI.enabled = true;
				}

				Splitter(new Color(0.35f, 0.35f, 0.35f));
				
				//ga.CB_foldout = EditorGUILayout.Foldout(ga.CB_foldout, _CB);

				GUILayout.BeginHorizontal();
				//GUILayout.Label("", GUILayout.Width(27));
				GUILayout.Label("", GUILayout.Width(-18));
				bool cb_enabled = EditorGUILayout.Toggle("", GA.SettingsGA.CB_enabled, GUILayout.Width(27));
				if (cb_enabled != GA.SettingsGA.CB_enabled && ToggleCB() != GA.SettingsGA.CB_enabled)
				{
					EditorPrefs.SetBool("GA_CB"+"-"+Application.dataPath, cb_enabled);
					GA.SettingsGA.CB_enabled = cb_enabled;
				}
				GUILayout.Label(_CBenabled, GUILayout.Width(150));
				GUILayout.EndHorizontal();
				
				if (GA.SettingsGA.CB_enabled)
				{
					//GUI.enabled = GA_Settings.CB_enabled;

					#if UNITY_ANDROID

					GUILayout.BeginHorizontal();
					GUILayout.Label("", GUILayout.Width(-5));
					EditorGUILayout.HelpBox("To setup Chartboost please see the Chartboost online documentation at https://help.chartboost.com/documentation/unity.", MessageType.Info);
					GUILayout.EndHorizontal();

					#else

					/*GUILayout.BeginHorizontal();
					GUILayout.Label("", GUILayout.Width(7));
					GUILayout.Label(_CBappID, GUILayout.Width(150));
					ga.CB_appID = EditorGUILayout.TextField(ga.CB_appID);
					GUILayout.EndHorizontal();
					
					GUILayout.BeginHorizontal();
					GUILayout.Label("", GUILayout.Width(7));
					GUILayout.Label(_CBappSig, GUILayout.Width(150));
					ga.CB_appSig = EditorGUILayout.TextField(ga.CB_appSig);
					GUILayout.EndHorizontal();*/

					GUILayout.BeginHorizontal();
					GUILayout.Label("", GUILayout.Width(-5));
					EditorGUILayout.HelpBox("To setup Chartboost please see the Chartboost online documentation at https://help.chartboost.com/documentation/unity.", MessageType.Info);
					GUILayout.EndHorizontal();

					#endif
					
					//GUI.enabled = true;
				}

				Splitter(new Color(0.35f, 0.35f, 0.35f));
				
				GUILayout.BeginHorizontal();
				GUILayout.Label(_startShowingAds, EditorStyles.largeLabel);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("", GUILayout.Width(7));
				GUILayout.Label(_startAlwaysShowAds, GUILayout.Width(150));
				ga.Start_AlwaysShowAds = EditorGUILayout.Toggle("", ga.Start_AlwaysShowAds);
				GUILayout.EndHorizontal();
				
				GUI.enabled = !ga.Start_AlwaysShowAds;
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("", GUILayout.Width(7));
				GUILayout.Label(_startTimePlayed, GUILayout.Width(150));
				ga.Start_TimePlayed = EditorGUILayout.Toggle("", ga.Start_TimePlayed, GUILayout.Width(27));
				GUILayout.Label("", GUILayout.Width(5));
				GUILayout.Label(_startTime, GUILayout.Width(55));
				ga.TimePlayed = EditorGUILayout.IntField(ga.TimePlayed, GUILayout.Width(60));
				ga.TimePlayed = Mathf.Max(0, ga.TimePlayed);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("", GUILayout.Width(7));
				GUILayout.Label(_startSessions, GUILayout.Width(150));
				ga.Start_Sessions = EditorGUILayout.Toggle("", ga.Start_Sessions, GUILayout.Width(27));
				GUILayout.Label("", GUILayout.Width(5));
				GUILayout.Label(_startSes, GUILayout.Width(55));
				ga.Sessions = EditorGUILayout.IntField(ga.Sessions, GUILayout.Width(60));
				ga.Sessions = Mathf.Max(0, ga.Sessions);
				GUILayout.EndHorizontal();
				
				GUI.enabled = true;
				
				EditorGUILayout.Space();
				
				GUILayout.BeginHorizontal();
				GUILayout.Label(_autoTriggers, EditorStyles.largeLabel);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("", GUILayout.Width(7));
				GUILayout.Label(_triggerAdsEnabled, GUILayout.Width(150));
				ga.Trigger_AdsEnabled = EditorGUILayout.Toggle("", ga.Trigger_AdsEnabled, GUILayout.Width(27));
				GUI.enabled = ga.Trigger_AdsEnabled;
				GUILayout.Label("", GUILayout.Width(1));
				GUILayout.Label(_triggerAdNetwork, GUILayout.Width(55));
				if (GA.SettingsGA.IAD_enabled && GA.SettingsGA.CB_enabled)
				{
					ga.Trigger_AdsEnabled_network = (GA_AdSupport.GAAdNetwork)EditorGUILayout.EnumPopup(ga.Trigger_AdsEnabled_network, GUILayout.Width(80));
				}
				else
				{
					if (GA.SettingsGA.IAD_enabled)
						ga.Trigger_AdsEnabled_network = GA_AdSupport.GAAdNetwork.iAd;
					else if (GA.SettingsGA.CB_enabled)
						ga.Trigger_AdsEnabled_network = GA_AdSupport.GAAdNetwork.Chartboost;
					else
						ga.Trigger_AdsEnabled_network = GA_AdSupport.GAAdNetwork.Any;
					
					GUILayout.Label("", GUILayout.Width(10));
					GUILayout.Label(ga.Trigger_AdsEnabled_network.ToString());
				}
				//GUILayout.Label("", GUILayout.Width(4));
				//CheckAdNetwork(ga, ga.Trigger_AdsEnabled_network);
				GUI.enabled = true;
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("", GUILayout.Width(7));
				GUILayout.Label(_triggerSceneChange, GUILayout.Width(150));
				ga.Trigger_SceneChange = EditorGUILayout.Toggle("", ga.Trigger_SceneChange, GUILayout.Width(27));
				GUI.enabled = ga.Trigger_SceneChange;
				GUILayout.Label("", GUILayout.Width(1));
				GUILayout.Label(_triggerAdNetwork, GUILayout.Width(55));
				if (GA.SettingsGA.IAD_enabled && GA.SettingsGA.CB_enabled)
				{
					ga.Trigger_SceneChange_network = (GA_AdSupport.GAAdNetwork)EditorGUILayout.EnumPopup(ga.Trigger_SceneChange_network, GUILayout.Width(80));
				}
				else
				{
					if (GA.SettingsGA.IAD_enabled)
						ga.Trigger_SceneChange_network = GA_AdSupport.GAAdNetwork.iAd;
					else if (GA.SettingsGA.CB_enabled)
						ga.Trigger_SceneChange_network = GA_AdSupport.GAAdNetwork.Chartboost;
					else
						ga.Trigger_SceneChange_network = GA_AdSupport.GAAdNetwork.Any;

					GUILayout.Label("", GUILayout.Width(10));
					GUILayout.Label(ga.Trigger_SceneChange_network.ToString());
				}
				//GUILayout.Label("", GUILayout.Width(4));
				//CheckAdNetwork(ga, ga.Trigger_SceneChange_network);
				GUI.enabled = true;
				GUILayout.EndHorizontal();
				
				EditorGUILayout.Space();
				
				GUILayout.BeginHorizontal();
				GUILayout.Label(_customTriggers, EditorStyles.largeLabel);
				GUILayout.EndHorizontal();
				
				List<GA_CustomAdTrigger> triggersToRemove = new List<GA_CustomAdTrigger>();

				foreach (GA_CustomAdTrigger trigger in ga.CustomAdTriggers)
				{
					if (trigger != null)
					{
						GUILayout.BeginHorizontal();
						GUILayout.Label("", GUILayout.Width(7));
						if (GUILayout.Button("x", GUILayout.Width(19), GUILayout.Height(15)))
						{
							triggersToRemove.Add(trigger);
						}
						//GUILayout.Label("", GUILayout.Width(7));
						GUILayout.Label(_triggerCustomID, GUILayout.Width(55));
						trigger.eventID = EditorGUILayout.TextField(trigger.eventID);//, GUILayout.Width(125));
						GUILayout.Label("", GUILayout.Width(1));
						GUILayout.Label(_triggerCustomCat, GUILayout.Width(60));
						trigger.eventCat = (GA_AdSupport.GAEventCat)EditorGUILayout.EnumPopup(trigger.eventCat, GUILayout.Width(80));
						GUILayout.Label("", GUILayout.Width(1));
						GUILayout.Label(_triggerAdNetwork, GUILayout.Width(55));
						if (GA.SettingsGA.IAD_enabled && GA.SettingsGA.CB_enabled)
						{
							trigger.AdNetwork = (GA_AdSupport.GAAdNetwork)EditorGUILayout.EnumPopup(trigger.AdNetwork, GUILayout.Width(80));
						}
						else
						{
							if (GA.SettingsGA.IAD_enabled)
								trigger.AdNetwork = GA_AdSupport.GAAdNetwork.iAd;
							else if (GA.SettingsGA.CB_enabled)
								trigger.AdNetwork = GA_AdSupport.GAAdNetwork.Chartboost;
							else
								trigger.AdNetwork = GA_AdSupport.GAAdNetwork.Any;
							
							GUILayout.Label("", GUILayout.Width(10));
							GUILayout.Label(trigger.AdNetwork.ToString());
						}
						//GUILayout.Label("", GUILayout.Width(4));
						//CheckAdNetwork(ga, trigger.AdNetwork);
						GUILayout.EndHorizontal();
					}
				}
				
				foreach (GA_CustomAdTrigger trigger in triggersToRemove)
				{
					ga.CustomAdTriggers.Remove(trigger);
					//ScriptableObject.DestroyImmediate(trigger);
				}
				
				GUILayout.Space(2);
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("", GUILayout.Width(7));
				if (GUILayout.Button("Add Trigger", GUILayout.Width(85)))
				{
					ga.CustomAdTriggers.Add( new GA_CustomAdTrigger() );
					//ga.CustomAdTriggers.Add(ScriptableObject.CreateInstance<GA_CustomAdTrigger>());
				}
				GUILayout.EndHorizontal();

				EditorGUILayout.Space();
			}
		}

		if (GUI.changed)
		{
            EditorUtility.SetDirty(ga);
        }
	}
	
	private MessageType ConvertMessageType(GA_Settings.MessageTypes msgType)
	{
		switch (msgType)
		{
			case GA_Settings.MessageTypes.Error:
				return MessageType.Error;
			case GA_Settings.MessageTypes.Info:
				return MessageType.Info;
			case GA_Settings.MessageTypes.Warning:
				return MessageType.Warning;
			default:
				return MessageType.None;
		}
	}

	private static void GetGameInfo (GA_Settings ga)
	{
		#if UNITY_4_3 || UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0
		Hashtable headers = new Hashtable();
		#else
		Dictionary<string, string> headers = new Dictionary<string, string>();
		#endif
		headers.Add("X-Authorization", ga.TokenGA);

		WWW www = new WWW("https://go.gameanalytics.com/v1/games/" + ga.Studios[ga.SelectedStudio-1].GameIDs[ga.SelectedGame-1], null, headers);
		GA_ContinuationManager.StartCoroutine(GetKeysFrontend(www, ga), () => www.isDone);
	}

	private static IEnumerator<WWW> GetKeysFrontend (WWW www, GA_Settings ga)
	{
		yield return www;
		
		try {
			if (string.IsNullOrEmpty(www.error))
			{
				Hashtable returnParam = (Hashtable)GA_MiniJSON.JsonDecode(www.text);
				string error = "";
				if (returnParam.ContainsKey("errors"))
				{
					ArrayList errorList = (ArrayList)returnParam["errors"];
					if (errorList != null && errorList.Count > 0)
					{
						Hashtable errors = (Hashtable)errorList[0];
						if (errors.ContainsKey("msg"))
						{
							error = errors["msg"].ToString();
						}
					}
				}
				
				if (!String.IsNullOrEmpty(error))
				{
					Debug.LogError(error);
				}
				else
				{
					ArrayList resultList = (ArrayList)returnParam["results"];
					Hashtable results = (Hashtable)resultList[0];
					ga.GameKey = results["key"].ToString();
					ga.SecretKey = results["secret_key"].ToString();

					SetLoginStatus ("Received Game Key and Secret Key. Ready to send events!", ga);

					GA_Tracking.SendEvent("KeysInput");
				}
			}
			else
			{
				Debug.LogError("Failed to get Game Key and Secret Key: " + www.error);
				SetLoginStatus ("Failed to get Game Key and Secret Key.", ga);
			}
		}
		catch {
			Debug.LogError("Failed to get Game Key and Secret Key");
			SetLoginStatus ("Failed to get Game Key and Secret Key.", ga);
		}
	}

	private static void SignupUser (GA_Settings ga)
	{
		string info = "{\"unityToken\": \"" + _unityToken + "\", \"email\": \"" + ga.EmailGA + "\", \"password\": \"" + ga.PasswordGA + "\", \"passwordConfirm\": \"" + ga.PasswordConfirm + "\", \"firstName\": \"" + ga.FirstName + "\", \"lastName\": \"" + ga.LastName + "\", \"studioName\": \"" + ga.StudioName + "\", \"gameTitle\": \"" + ga.GameName + "\", \"emailOptOut\": " + Convert.ToInt32(ga.EmailOptIn) + "}";
		byte[] data = System.Text.Encoding.UTF8.GetBytes(info);
		
		WWW www = new WWW("https://go.gameanalytics.com/v1/public/signup/basic/unity_editor", data);
		GA_ContinuationManager.StartCoroutine(SignupUserFrontend(www, ga), () => www.isDone);
	}

	private static IEnumerator<WWW> SignupUserFrontend (WWW www, GA_Settings ga)
	{
		yield return www;

		try {
			if (string.IsNullOrEmpty(www.error))
			{
				Hashtable returnParam = (Hashtable)GA_MiniJSON.JsonDecode(www.text);
				string error = "";
				if (returnParam.ContainsKey("errors"))
				{
					ArrayList errorList = (ArrayList)returnParam["errors"];
					if (errorList != null && errorList.Count > 0)
					{
						Hashtable errors = (Hashtable)errorList[0];
						if (errors.ContainsKey("msg"))
						{
							error = errors["msg"].ToString();
						}
					}
				}

				if (!String.IsNullOrEmpty(error))
				{
					Debug.LogError(error);
				}
				else
				{
					ArrayList resultList = (ArrayList)returnParam["results"];
					Hashtable results = (Hashtable)resultList[0];
					ga.TokenGA = results["token"].ToString();
					ga.ExpireTime = results["exp"].ToString();

					ga.JustSignedUp = true;

					//ga.SignUpOpen = false;
					ga.SelectedStudio = 0;
					ga.SelectedGame = 0;
					ga.Studios = null;
					SetLoginStatus ("Signed up. Getting user data..", ga);
					
					GetUserData(ga);
				}
			}
			else
			{
				Debug.LogError("Failed to sign up: " + www.error);
				SetLoginStatus ("Failed to sign up.", ga);
			}
		}
		catch {
			Debug.LogError("Failed to sign up");
			SetLoginStatus ("Failed to sign up.", ga);
		}
	}
	
	private static void LoginUser (GA_Settings ga)
	{
		string info = "{\"email\": \"" + ga.EmailGA + "\", \"password\": \"" + ga.PasswordGA + "\", \"remember\": false}";

		byte[] data = System.Text.Encoding.UTF8.GetBytes(info);

		WWW www = new WWW("https://go.gameanalytics.com/v1/public/login/basic", data);
		GA_ContinuationManager.StartCoroutine(LoginUserFrontend(www, ga), () => www.isDone);
	}
	
	private static IEnumerator<WWW> LoginUserFrontend (WWW www, GA_Settings ga)
	{
		yield return www;
		
		try {
			if (string.IsNullOrEmpty(www.error))
			{
				Hashtable returnParam = (Hashtable)GA_MiniJSON.JsonDecode(www.text);
				string error = "";
				if (returnParam.ContainsKey("errors"))
				{
					ArrayList errorList = (ArrayList)returnParam["errors"];
					if (errorList != null && errorList.Count > 0)
					{
						Hashtable errors = (Hashtable)errorList[0];
						if (errors.ContainsKey("msg"))
						{
							error = errors["msg"].ToString();
						}
					}
				}
				
				if (!String.IsNullOrEmpty(error))
				{
					Debug.LogError(error);
					SetLoginStatus ("Failed to login.", ga);
				}
				else
				{
					ArrayList resultList = (ArrayList)returnParam["results"];
					Hashtable results = (Hashtable)resultList[0];
					ga.TokenGA = results["token"].ToString();
					ga.ExpireTime = results["exp"].ToString();
					
					SetLoginStatus ("Logged in. Getting user data..", ga);

					GetUserData(ga);
				}
			}
			else
			{
				Debug.LogError("Failed to login: " + www.error);
				SetLoginStatus ("Failed to login.", ga);
			}
		}
		catch {
			Debug.LogError("Failed to login");
			SetLoginStatus ("Failed to login.", ga);
		}
	}

	private static void GetUserData (GA_Settings ga)
	{
		#if UNITY_4_3 || UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0
		Hashtable headers = new Hashtable();
		#else
		Dictionary<string, string> headers = new Dictionary<string, string>();
		#endif
		headers.Add("X-Authorization", ga.TokenGA);
		
		WWW www = new WWW("https://go.gameanalytics.com/v1/user/data", null, headers);
		GA_ContinuationManager.StartCoroutine(GetUserDataFrontend(www, ga), () => www.isDone);
	}

	private static IEnumerator<WWW> GetUserDataFrontend (WWW www, GA_Settings ga)
	{
		yield return www;
		
		try {
			if (string.IsNullOrEmpty(www.error))
			{
				Hashtable returnParam = (Hashtable)GA_MiniJSON.JsonDecode(www.text);
				string error = "";
				if (returnParam.ContainsKey("errors"))
				{
					ArrayList errorList = (ArrayList)returnParam["errors"];
					if (errorList != null && errorList.Count > 0)
					{
						Hashtable errors = (Hashtable)errorList[0];
						if (errors.ContainsKey("msg"))
						{
							error = errors["msg"].ToString();
						}
					}
				}
				
				if (!String.IsNullOrEmpty(error))
				{
					Debug.LogError(error);
					SetLoginStatus ("Failed to get user data.", ga);
				}
				else
				{
					ArrayList resultList = (ArrayList)returnParam["results"];
					Hashtable results = (Hashtable)resultList[0];
					ArrayList studioList = (ArrayList)results["studiosGames"];
					
					List<Studio> returnStudios = new List<Studio>();
					
					for (int s = 0; s < studioList.Count; s++)
					{
						Hashtable studio = (Hashtable)studioList[s];
						if (!studio.ContainsKey("demo") || !((bool)studio["demo"]))
						{
							List<string> returnGames = new List<string>();
							List<string> returnTokens = new List<string>();
							List<int> returnIDs = new List<int>();
							
							ArrayList gamesList = (ArrayList)studio["games"];
							for (int g = 0; g < gamesList.Count; g++)
							{
								Hashtable games = (Hashtable)gamesList[g];
								returnGames.Add(games["title"].ToString());
								returnIDs.Add(int.Parse(games["id"].ToString()));
								Hashtable token = (Hashtable)games["dataApiToken"];
								returnTokens.Add(token["token"].ToString());
							}
							
							returnStudios.Add(new Studio(studio["name"].ToString(), returnGames, returnTokens, returnIDs));
						}
					}
					ga.Studios = returnStudios;

					if (ga.Studios.Count == 1)
					{
						SelectStudio (1, ga);
					}
					else
					{
						SetLoginStatus ("Received user data. Please select studio..", ga);
					}

					ga.CurrentInspectorState = GA_Settings.InspectorStates.Basic;
				}
			}
			else
			{
				Debug.LogError("Failed to get user data: " + www.error);
				SetLoginStatus ("Failed to get user data.", ga);
			}
		}
		catch {
			Debug.LogError("Failed to get user data");
			SetLoginStatus ("Failed to get user data.", ga);
		}
	}

	private static void SelectStudio (int index, GA_Settings ga)
	{
		ga.SelectedStudio = index;
		if (ga.Studios[index - 1].Games.Count == 1)
		{
			SelectGame (1, ga);
		}
		else
		{
			SetLoginStatus ("Please select game..", ga);
		}
	}

	private static void SelectGame (int index, GA_Settings ga)
	{
		ga.SelectedGame = index;
		SetLoginStatus ("Getting Game Key and Secret Key..", ga);
		GetGameInfo(ga);
	}

	private static void SetLoginStatus (string status, GA_Settings ga)
	{
		ga.LoginStatus = status;
		EditorUtility.SetDirty(ga);
	}

	public static void CheckForUpdates ()
	{
		WWW www = new WWW("https://s3.amazonaws.com/public.gameanalytics.com/sdk_status/current.json");
		GA_ContinuationManager.StartCoroutine(CheckWebUpdate(www), () => www.isDone);
	}
	
	private static void GetUpdateChanges ()
	{
		WWW www = new WWW("https://s3.amazonaws.com/public.gameanalytics.com/sdk_status/change_logs.json");
		GA_ContinuationManager.StartCoroutine(CheckWebChanges(www), () => www.isDone);
	}
	
	private static IEnumerator<WWW> CheckWebUpdate (WWW www)
	{
		yield return www;
		
		try {
			if (string.IsNullOrEmpty(www.error))
			{
				Hashtable returnParam = (Hashtable)GA_MiniJSON.JsonDecode(www.text);
				string newVersion = ((Hashtable)returnParam["unity"])["version"].ToString();
				
				GA_UpdateWindow.SetNewVersion(newVersion);

				int newV = int.Parse(newVersion.Replace(".",""));
				int oldV = int.Parse(GA_Settings.VERSION.Replace(".",""));

				if (newV > oldV)
				{
					GetUpdateChanges();
				}
			}
		}
		catch {}
	}
	
	private static IEnumerator<WWW> CheckWebChanges (WWW www)
	{
		yield return www;
		
		try {
			if (string.IsNullOrEmpty(www.error))
			{
				Hashtable returnParam = (Hashtable)GA_MiniJSON.JsonDecode(www.text);
				
				ArrayList unity = ((ArrayList)returnParam["unity"]);
				for (int i = 0; i < unity.Count; i++)
				{
					Hashtable unityHash = (Hashtable)unity[i];
					if (unityHash["version"].ToString() == GA_UpdateWindow.GetNewVersion())
					{
						i = unity.Count;
						ArrayList changes = ((ArrayList)unityHash["changes"]);
						string newChanges = "";
						for (int u = 0; u < changes.Count; u++)
						{
							if (string.IsNullOrEmpty(newChanges))
								newChanges = "- " + changes[u].ToString();
							else
								newChanges += "\n- " + changes[u].ToString();
						}
						
						GA_UpdateWindow.SetChanges(newChanges);
						string skippedVersion = EditorPrefs.GetString("ga_skip_version"+"-"+Application.dataPath, "");
						
						if (!skippedVersion.Equals(GA_UpdateWindow.GetNewVersion()))
						{
							OpenUpdateWindow();
						}
					}
				}
			}
		}
		catch {}
	}
	
	private static void OpenUpdateWindow ()
	{
		GA_UpdateWindow updateWindow = ScriptableObject.CreateInstance<GA_UpdateWindow> ();
		updateWindow.ShowUtility ();
		updateWindow.position = new Rect (150, 150, 420, 340);
	}

	private void CheckAdNetwork (GA_Settings ga, GA_AdSupport.GAAdNetwork AdNetwork)
	{
		if (AdNetwork == GA_AdSupport.GAAdNetwork.iAd && !GA.SettingsGA.IAD_enabled ||
		    AdNetwork == GA_AdSupport.GAAdNetwork.Chartboost && !GA.SettingsGA.CB_enabled)
		{
			Rect lastrect = GUILayoutUtility.GetLastRect();
			Color tmpColor = GUI.color;
			int tmpSize = GUI.skin.label.fontSize;
			//GUI.color = Color.red;
			//GUI.DrawTexture(new Rect(lastrect.x - 2, lastrect.y - 1, 5, 17), _triggerAdNotEnabledTexture);
			GUI.color = Color.white;
			GUI.skin.label.fontSize = 20;
			GUI.Label(new Rect(lastrect.x - 5, lastrect.y - 7, 20, 30), _triggerAdNotEnabled);
			GUI.skin.label.fontSize = tmpSize;
			GUI.color = tmpColor;
		}
	}

	static bool ToggleCB ()
	{
		bool enabled = false;
		bool fail = false;
		
		string searchText = "//#define CB_ON";
		string replaceText = "#define CB_ON";
		
		string filePath = Application.dataPath + "/GameAnalytics/Plugins/Framework/Scripts/GA_AdSupport.cs";
		string filePathJS = Application.dataPath + "/Plugins/GameAnalytics/Framework/Scripts/GA_AdSupport.cs";
		try {
			enabled = GA_Menu.ReplaceInFile (filePath, searchText, replaceText);
		} catch {
			try {
				enabled = GA_Menu.ReplaceInFile (filePathJS, searchText, replaceText);
			} catch {
				fail = true;
			}
		}
		
		AssetDatabase.Refresh();
		
		if (fail)
			Debug.Log("Failed to toggle CB.");
		
		return enabled;
	}

	static void Splitter(Color rgb, float thickness = 1)
	{
		GUIStyle splitter = new GUIStyle();
		splitter.normal.background = EditorGUIUtility.whiteTexture;
		splitter.stretchWidth = true;
		splitter.margin = new RectOffset(0, 0, 7, 7);

		Rect position = GUILayoutUtility.GetRect(GUIContent.none, splitter, GUILayout.Height(thickness));
		
		if (Event.current.type == EventType.Repaint) {
			Color restoreColor = GUI.color;
			GUI.color = rgb;
			splitter.Draw(position, false, false, false, false);
			GUI.color = restoreColor;
		}
	}
}