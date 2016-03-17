#if false

using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Force data to be sent to Game Analytics server.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1168")]
	public class QueueForceSubmit : FsmStateAction
	{
		[Tooltip("If True, will force all data to be sent to the Game Analytics server.")]
		FsmBool forceSubmit;
		
		public override void Reset()
		{
			forceSubmit = true;
		}
		
		public override void OnEnter()
		{
			if (forceSubmit.Value)
			{
				GA_Queue.ForceSubmit();
			}
			
			Finish();
		}
	}
}

#endif