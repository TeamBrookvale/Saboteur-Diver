//#define CB_ON

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if CB_ON
using ChartboostSDK;
#endif
public class GA_AdSupport : MonoBehaviour
{
	public enum GAEventCat { Design, Business } //, Quality }
	public enum GAEventType { AdsEnabled, LevelChange, Custom }
	public enum GAAdNetwork { Any, iAd, Chartboost }
	
	#region public values
	
	public static GA_AdSupport GA_ADSUPPORT;
	
	public bool AdsEnabled = false;
	
	#endregion
	
	//private bool _showAdOnLoad = false;
	private bool _adShowing = false;
	
	#if UNITY_IPHONE
	private ADBannerView _iAdBanner = null;
	#endif
	
	private string _eventTriggerID = "";
	
	private float _timePlayed = 0;
	private int _sessionsPlayed = 0;
	private static bool _sessionRecorded = false;
	
	#region unity derived methods
	
	void Awake()
	{
		if (GA_ADSUPPORT != null)
		{
			// only one ad support allowed per scene
			GA.LogWarning("Destroying dublicate GA_ADSUPPORT - only one is allowed per scene!");
			Destroy(gameObject);
			return;
		}
		GA_ADSUPPORT = this;
		
		DontDestroyOnLoad(gameObject);
		
		SaveConditions ();
		
		// iAd
		if (GA.SettingsGA.IAD_enabled)
		{
			#if UNITY_IPHONE
			if (ADBannerView.IsAvailable(ADBannerView.Type.MediumRect) && iPhone.generation.ToString().StartsWith("iPad"))
				_iAdBanner = new ADBannerView(GA.SettingsGA.IAD_type, GA.SettingsGA.IAD_layout);
			else
				_iAdBanner = new ADBannerView(ADBannerView.Type.Banner, GA.SettingsGA.IAD_layout);
			
			if (GA.SettingsGA.IAD_layout == ADBannerView.Layout.Manual)
			{
				_iAdBanner.position = GA.SettingsGA.IAD_position;
			}
			
			ADBannerView.onBannerWasClicked += OnBannerClicked;
			ADBannerView.onBannerWasLoaded  += OnBannerLoaded;
			#endif
		}
		
		// Charboost
		#if CB_ON
		if (GA.SettingsGA.CB_enabled)
		{
			if (FindObjectOfType(typeof(Chartboost)) == null)
			{
				GameObject go = new GameObject("ChartboostManager");
				go.AddComponent<Chartboost>();

				Chartboost.didDismissInterstitial += OnDismissInterstitialEvent;
				Chartboost.didCloseInterstitial += OnCloseInterstitialEvent;
				Chartboost.didClickInterstitial += OnClickInterstitialEvent;
				Chartboost.didDisplayInterstitial += OnShowInterstitialEvent;
			}
		}
		#endif
	}
	
	void Start ()
	{
		if (GA.SettingsGA.Start_AlwaysShowAds)
		{
			EnableAds();
		}
	}
	
	void Update ()
	{
		if (GA.SettingsGA.Start_TimePlayed)
		{
			_timePlayed += Time.deltaTime;
			
			if (_timePlayed >= GA.SettingsGA.TimePlayed)
			{
				EnableAds();
			}
		}
		
		#if UNITY_ANDROID && CB_ON
		// Uncomment the following code if you are not handling the Android back button.
		// Note: This will cause the Android back button to quit your application.
		/*
		if (GA.SettingsGA.CB_enabled)
		{
			// Handle the Android back button
			if (Input.GetKeyUp(KeyCode.Escape)) {
				// Check if Chartboost wants to respond to it
				if (CBBinding.onBackPressed()) {
					// If so, return and ignore it
					return;
				} else {
					// Otherwise, handle it ourselves -- let's close the app
					Application.Quit();
				}
			}
		}
		*/
		#endif
	}
	
	void OnDestroy()
	{
		if (GA_ADSUPPORT == this)
			GA_ADSUPPORT = null;
	}
	
	void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			SaveConditions ();
		}
	}
	
	void OnLevelWasLoaded ()
	{
		SaveConditions ();
		ShowAd(GAEventType.LevelChange, GAEventCat.Design, null);
	}
	
	#endregion
	
	public static void ShowAdStatic (GAEventType eventType, GAEventCat eventCategory, string eventID)
	{
		if (GA_ADSUPPORT != null)
		{
			GA_ADSUPPORT.ShowAd (eventType, eventCategory, eventID);
		}
	}
	
	private void ShowAd (GAEventType eventType, GAEventCat eventCategory, string eventID)
	{
		if (AdsEnabled)
		{
			switch (eventType)
			{
			case GAEventType.AdsEnabled:
				if (GA.SettingsGA.Trigger_AdsEnabled)
				{
					ShowAdNow("AdsEnabled");
				}
				break;
			case GAEventType.LevelChange:
				if (GA.SettingsGA.Trigger_SceneChange)
				{
					ShowAdNow("LevelChange");
				}
				break;
			case GAEventType.Custom:
				foreach (GA_CustomAdTrigger trigger in GA.SettingsGA.CustomAdTriggers)
				{
					if (eventCategory == trigger.eventCat && eventID.Equals(trigger.eventID))
					{
						ShowAdNow("Custom:"+trigger.eventID);
					}
				}
				break;
			}
		}
	}
	
	private void ShowAdNow(string eventID)
	{
		if (_adShowing)
			return;
		
		GA.Log("GA Show Ad Now");
		
		bool iAd = false;
		bool cb = false;
		
		#if UNITY_IPHONE
		if (GA.SettingsGA.IAD_enabled && _iAdBanner != null && _iAdBanner.loaded)
		{
			iAd = true;
		}
		#endif
		
		#if CB_ON
		if (GA.SettingsGA.CB_enabled)
		{
			cb = true;
		}
		#endif
		
		_eventTriggerID = eventID;
		
		if (iAd && cb)
		{
			int r = Random.Range(0, 2);
			
			if (r == 0)
			{
				ShowIad();
			}
			else if (r == 1)
			{
				ShowCB();
			}
		}
		else if (iAd)
		{
			ShowIad();
		}
		else if (cb)
		{
			ShowCB();
		}
	}
	
	private void ShowIad ()
	{
		#if UNITY_IPHONE
		_iAdBanner.visible = true;
		_adShowing = true;
		
		GA.API.Design.NewEvent("Impressions:iAD:" + _eventTriggerID);
		
		StartCoroutine(CloseAd(GA.SettingsGA.IAD_Duration));
		#endif
	}
	
	private void ShowCB ()
	{
		#if CB_ON
		CBExternal.showInterstitial( CBLocation.Default );
		_adShowing = true;
		#endif
	}
	
	IEnumerator CloseAd (float duration)
	{
		yield return new WaitForSeconds(duration);
		
		#if UNITY_IPHONE
		if (GA.SettingsGA.IAD_enabled)
		{
			_iAdBanner.visible = false;
		}
		#endif
		_adShowing = false;
		//_showAdOnLoad = false;
	}
	
	void OnBannerLoaded()
	{
		//GA.Log("GA iAd Loaded");
	}
	
	void OnBannerClicked()
	{
		//GA.Log("GA iAd Clicked");
		GA.API.Design.NewEvent("Clicks:iAD:" + _eventTriggerID);
	}

	#if CB_ON
	void OnShowInterstitialEvent(CBLocation location)
	{
		//Debug.Log( "didShowInterstitialEvent: " + location );
		GA.API.Design.NewEvent("Impressions:Chartboost:" + _eventTriggerID);
	}
	
	void OnClickInterstitialEvent(CBLocation location)
	{
		//Debug.Log( "didClickInterstitialEvent: " + location );
		GA.API.Design.NewEvent("Clicks:Chartboost:" + _eventTriggerID);
	}
	
	void OnDismissInterstitialEvent(CBLocation location)
	{
		//Debug.Log( "didDismissInterstitialEvent: " + location );
		_adShowing = false;
	}
	
	void OnCloseInterstitialEvent(CBLocation location)
	{
		//Debug.Log( "didCloseInterstitialEvent: " + location );
		_adShowing = false;
	}
	#endif
	
	private void SaveConditions ()
	{
		bool save = false;
		
		if (GA.SettingsGA.Start_TimePlayed)
		{
			float tmpTime = PlayerPrefs.GetFloat("GA_TimePlayed");
			
			if (tmpTime > _timePlayed)
			{
				_timePlayed = tmpTime;
			}
			
			PlayerPrefs.SetFloat("GA_TimePlayed", _timePlayed);
			
			save = true;
		}
		
		if (GA.SettingsGA.Start_Sessions && !_sessionRecorded)
		{
			int tmpSession = PlayerPrefs.GetInt("GA_Sessions");
			
			if (tmpSession > _sessionsPlayed)
			{
				_sessionsPlayed = tmpSession;
			}
			
			_sessionRecorded = true;
			_sessionsPlayed = _sessionsPlayed + 1;
			
			PlayerPrefs.SetInt("GA_Sessions", _sessionsPlayed);
			
			if (_sessionsPlayed > GA.SettingsGA.Sessions)
			{
				EnableAds();
			}
			
			save = true;
		}
		
		if (save)
		{
			PlayerPrefs.Save();
		}
	}
	
	/// <summary>
	/// Start Showing Ads
	/// </summary>
	public static void EnableAds()
	{
		if (GA_ADSUPPORT != null)
			GA_ADSUPPORT.EnableShowingAds();
	}
	
	public void EnableShowingAds()
	{
		if (!AdsEnabled)
		{
			GA.Log("GA Ads Enabled");
			AdsEnabled = true;
			
			ShowAd(GAEventType.AdsEnabled, GAEventCat.Design, null);
		}
	}
}
