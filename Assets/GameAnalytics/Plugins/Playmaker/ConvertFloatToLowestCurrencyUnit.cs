#if false

using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
    [Tooltip("Game Analytics Business events expects currency in the lowest currency unit. F.x. if currency is USD then amount should be in cent, 3.50 would become 350")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1169")]
	public class ConvertFloatToLowestCurrencyUnit : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The float variable to convert.")]
		public FsmFloat floatVariable;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The currency express in cents")]
		public FsmInt currencyResult;
       

		public override void Reset()
		{
			floatVariable = null;
			currencyResult = null;
		}

		public override void OnEnter()
		{
			DoConvert();
			Finish();
		}

		void DoConvert()
		{

			string[] _split = floatVariable.Value.ToString().Split("."[0]);
			
			string final = _split[0];
			
			if (_split.Length == 1)
			{
				final += "00";
			}else{
				final += _split[1].PadRight(2, '0').Substring(0,2);
			}
			
			currencyResult.Value = int.Parse(final);

		}
	}
}

#endif