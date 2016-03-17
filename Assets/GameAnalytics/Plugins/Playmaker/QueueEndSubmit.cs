#if false

using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Stop sending data to Game Analytics server.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1167")]
	public class QueueEndSubmit : FsmStateAction
	{
		[Tooltip("If True, will stop sending data to Game Analytics server with next time interval.")]
		FsmBool endSubmit;
		
		public override void Reset()
		{
			endSubmit = true;
		}
		
		public override void OnEnter()
		{
			if (endSubmit.Value)
			{
				GA_Queue.EndSubmit();
			}
			
			Finish();
		}
	}
}

#endif