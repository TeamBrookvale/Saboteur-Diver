using UnityEngine;

namespace TeamBrookvale
{
	public static class Const
	{
		public const float PlayerSwimSpeed = 1; // player's swim speed
		public const float PlayerRotationTrim = 0; // THIS SHOULD BE ZERO - player is rotated by this at all times

		public const float SpotlightDetail = 6; // detail of the cone in degrees  666
		public const float SpotlightDistortion = .6f; 	// y distance has to be squeezed due to fake isometric view
	
		public const float SpotlightLookAtTargetLag = 1; // takes 2 seconds plus random until the spotlight realises the target
		public const float SpotlightLookAtRotateSpeed = .5f; // how long it takes to rotate to the target
		public const float SpotlightReturnToNormalTime = 6; // if the spotlight is looking at something then return after 6 seconds
		public const float SpotlightMaxTargetDistance = 8; // if the target is further than this then do not look at it
		public const float SpotlightCatchTimeThreshold = 1f; // player caught if staying in the spotlight for over 3 seconds

		public const float ShipPatrolSlowSpeed = 0.001f; // speed if alert or almost crashing .003
		public const float ShipPatrolAlertSlowLength = 10; // how long the patrol ship is slowing down for in seconds
		public const float ShipPatrolDoNotNoticeDrillAirBubbleTemporarilyTime = 10; // do not notice sunk drilled ships again for 30 seconds
		public const float ShipPatrolPutBackOnceFurtherThan = 25; // if the patrolship is further than 25 in any direction then put back
		public const float ShipPatrolDirectionRightAngle = 36; // 0 to 90 direction angle (was 38 - Tomi)
		public const float ShipPatrolDirectionLeftAngle = 20; // 0 to 90 direction angle (was 25 - Tomi)

		public const float ShipDrillableRockingSpeed = 1.5f;
		public const float ShipDrillableRockingAmplitude = .02f;
		public const float ShipDrillableLargeDrillSpeedFactor = .3f; // drill speed of the large boat compared to the small one

		public static readonly Color SpotLightColorRed 	= new Color (1, .4f, .4f, .5f);
		public static readonly Color SpotLightColorGreen= new Color (.4f, 1, .9f, .5f);

		public static int ScreenHeight;

		public static int ToolIconPixelSizeX					{ get { return 128 * ScreenHeight / 640; }}
		public static int ToolIconPixelSizeY					{ get { return 128 * ScreenHeight / 640; }}
		public static int CounterIconPixelSizeX					{ get { return  89 * ScreenHeight / 640; }}
		public static int CounterIconPixelSizeY					{ get { return  89 * ScreenHeight / 640; }}
		public static int InventoryCounterIconPositionOffsetY	{ get { return  40 * ScreenHeight / 640; }}// distance of the centres of InventoryCounterIcon and Tool icon
		public static int ProgressCounterIconPositionOffsetY	{ get { return 100 * ScreenHeight / 640; }}// distance of the centres of ProgressCounterIcon and InventoryCounterIcon

		public const int SelectLevelButtonSizeXinScreenPercentage = 10;

		public const int IconScreenBorderMargin = 5;
		public const int SelectLevelButtonMarginXinScreenPercentage = 5;

		public const float CameraOrthographicZoomSpeed = .03f;  // The rate of change of the orthographic size in orthographic mode.
		public const float CameraOrthographicMinSize = 2.5f;
		public const float CameraOrthographicMaxSize = 20f;
		public const float CameraPanSpeed = .005f;
		public const float CameraBoundsExtraMargin = 3; // if spotlight is out of over 5 world coordinate units of the camera then turn off

		public const int FirstFingerMovePixelThreshold = 20;
		public const float LongPressTimeThreshold = .5f; // Touch and hold for at least .5 seconds and then it becomes a longpress

		public const float SmokeBombDistanceThreshold = 3; // Everybody looks towards the smokebomb within this distance

		public const float SpawnAnimationLifeSpan = 10; // spawned animation items will be destroyed after these many seconds

		public const float PanicModeLenght = 30; // panic mode length in seconds

		public const bool isStatusBarAlwaysVisible = true; // does the status bar hide between messages
		public const float StatusBarMessageTimeLenght = 5; // time after the status bar disappears

		public const string StatusBarPanicMode =		"A sinking ship has been noticed";
		public const string StatusBarBackToGetawayBoat= "Swim back to the getaway boat";
		public const string StatusBarLocateDock = 		"Locate the military dock";
		public const string StatusBarLocateBuoy = 		"Locate the buoys";
		public const string StatusBarLocateDam = 		"Locate the dam";
		public const string StatusBarLocateBridge = 	"Locate the bridge";
		public const string StatusBarLocateTutorial =	"Tutorial level";
		public const string StatusBarInfiltrateBoat = 	"Infiltrate boats";
		//public const string StatusBarSunkBoat = 		"You are finished with this boat";
		public const string StatusBarPlantBomb = 		"Plant three timebombs";
		//public const string StatusBarPlantedBomb = 		"You have already planted a bomb here";
		public const string StatusBarUBoat = 			"Use the mini U-boat to move unnoticed";	

		public const float minimumTimeBombDistance = 1; // minimum distance between timebombs
		public const int numberOfTimeBombsToMount = 3; // number of timebombs to mount in a scene that has bridge or dam
		public const float timeNeededToMountTimeBomb = 2; // seconds needed to mount timebomb

		public const string PlayerPrefsMusicOn			= "MusicOn";
		public const string PlayerPrefsLevelToLoad		= "LevelToLoad";
		public const string PlayerPrefsMaxLevelPassed	= "MaxLevelPassed";

		public const int MaxLevelNumber = 19; // this is the number of the last level
	}

}