#if false

using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sets a custom area Identifier. By default, the GameAnalytics SDK for Unity uses the Application.loadedLevelName static Unity parameter as area identifier. If you need to set a custom area identifier, use this.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1179")]
	public class SetCustomArea : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The custom area")]
		public FsmString CustomArea;
		
		public override void Reset()
		{
			CustomArea = null;
		}
		
		public override void OnEnter()
		{
			GA.SettingsGA.SetCustomArea(CustomArea.Value);
			
			Finish();
		}
	}
}

#endif