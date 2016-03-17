#if false

using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sets the User ID manually. This is useful if you have your own log-in system, which ensures you have a unique user ID.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1180")]
	public class SetCustomUserID : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The custom User ID")]
		public FsmString UserCustomID;
		
		public override void Reset()
		{
			UserCustomID = null;
		}
		
		public override void OnEnter()
		{
			GA.SettingsGA.SetCustomUserID(UserCustomID.Value);

			Finish();
		}
	}
}

#endif