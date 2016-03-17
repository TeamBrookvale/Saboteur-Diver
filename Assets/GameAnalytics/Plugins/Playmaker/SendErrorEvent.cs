#if false

using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends a error event message to the GameAnalytics server.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1171")]
	public class SendErrorEvent : FsmStateAction
	{

		[Tooltip("The severity of this event: critical, error, warning, info, debug")]
		public GA_Error.SeverityType severityType ;
		
		[Tooltip("The message")]
		[RequiredField]
		public FsmString Message;
		
		public override void Reset()
		{
			severityType = GA_Error.SeverityType.error;
			Message = new FsmString();
		}
		
		public override void OnEnter()
		{
			GA.API.Error.NewEvent(severityType,Message.Value);

			Finish();
		}
	}
}

#endif