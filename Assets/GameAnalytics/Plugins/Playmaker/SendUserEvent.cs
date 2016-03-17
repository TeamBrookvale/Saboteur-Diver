#if false

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends a user event message to the GameAnalytics server")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1166")]
	public class SendUserEvent : FsmStateAction
	{
		[Tooltip("The player's gender as string: 'male', 'female' case insensitive, any other values will be interpreted as unknown")]
		public FsmString GenderAsString;
		
		[Tooltip("The player's gender")]
		public GA_User.Gender Gender;
		
		[Tooltip("The year the player was born")]
		public FsmInt BirthYear;
		
		[Tooltip("The number of friends in the player's network.")]
		public FsmInt FriendCount;

		public FsmString iosId;
		public FsmString androidId;
		public FsmString platform;
		public FsmString device;
		public FsmString os;
		public FsmString osVersion;
		public FsmString sdk;
		public FsmString installPublisher;
		public FsmString installSite;
		public FsmString installCampaign;
		public FsmString installAdGroup;
		public FsmString installAd;
		public FsmString installKeyword;
		public FsmString facebookId;
		public FsmString android_adID;


		public bool advanced;


		public override void Reset()
		{
			Gender				= GA_User.Gender.Unknown;
			BirthYear 			= new FsmInt()    { UseVariable = true};
			FriendCount 		= new FsmInt()    { UseVariable = true};
			iosId 				= new FsmString() { UseVariable = true};
			androidId			= new FsmString() { UseVariable = true};
			platform			= new FsmString() { UseVariable = true};
			device				= new FsmString() { UseVariable = true};
			os					= new FsmString() { UseVariable = true};
			osVersion			= new FsmString() { UseVariable = true};
			sdk					= new FsmString() { UseVariable = true};
			installPublisher	= new FsmString() { UseVariable = true};
			installSite			= new FsmString() { UseVariable = true};
			installCampaign		= new FsmString() { UseVariable = true};
			installAdGroup		= new FsmString() { UseVariable = true};
			installAd			= new FsmString() { UseVariable = true};
			installKeyword		= new FsmString() { UseVariable = true};
			facebookId			= new FsmString() { UseVariable = true};
			android_adID		= new FsmString() { UseVariable = true};
		}
		
		public override void OnEnter()
		{
			if (GenderAsString.Value.Equals("male",StringComparison.InvariantCultureIgnoreCase) )
			{
				Gender = GA_User.Gender.Male;
			}else if (GenderAsString.Value.Equals("female",StringComparison.InvariantCultureIgnoreCase) )
			{
				Gender = GA_User.Gender.Female;
			}

			GA.API.User.NewUser(
				Gender,
				BirthYear.IsNone		? null : (int?)BirthYear.Value,
				FriendCount.IsNone		? null : (int?)FriendCount.Value,
				iosId.IsNone			? null : iosId.Value,
				androidId.IsNone		? null : androidId.Value, 
				platform.IsNone			? null : platform.Value, 
				device.IsNone			? null : device.Value,  
				os.IsNone				? null : os.Value,  
				osVersion.IsNone		? null : osVersion.Value,  
				sdk.IsNone				? null : sdk.Value,  
				installPublisher.IsNone	? null : installPublisher.Value,  
				installSite.IsNone		? null : installSite.Value,  
				installCampaign.IsNone	? null : installCampaign.Value,  
				installAdGroup.IsNone	? null : installAdGroup.Value,  
				installAd.IsNone		? null : installAd.Value,  
				installKeyword.IsNone	? null : installKeyword.Value,
				facebookId.IsNone		? null : facebookId.Value,
				android_adID.IsNone		? null : android_adID.Value
				);

		
			Finish();
		}
	}
}

#endif