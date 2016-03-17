#if false

using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends a business event message to the GameAnalytics server")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1163")]
	public class SendBusinessEvent : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The event ID")]
		public FsmString EventID;
		
		[RequiredField]
		[Tooltip("Abbreviation of the currency used for the transaction. F.x. USD (U.S. Dollars)")]
		public FsmString Currency;
		
		[RequiredField]
		[Tooltip("The value of the transaction in the lowest currency unit. F.x. if currency is USD then amount should be in cent. Use action 'ConvertFloatToLowestCurrencyUnit' to convert if required")]
		public FsmInt Amount;

		
		public override void Reset()
		{
			EventID = new FsmString() { UseVariable = false };
			Currency = new FsmString() { UseVariable = false };
			Amount = new FsmInt() { UseVariable = false };
		}
		
		public override void OnEnter()
		{
			GA.API.Business.NewEvent(EventID.Value, Currency.Value, Amount.Value);
			
			Finish();
		}
	}
}

#endif