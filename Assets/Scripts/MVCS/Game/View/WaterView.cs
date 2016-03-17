using UnityEngine;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class WaterView : View
	{
		internal void OnPanicModeStartedOrEndedSignal (bool startOrStop)
		{
			GetComponent<Animator> ().SetTrigger ((startOrStop ? "PanicStart" : "PanicEnd"));
		}
	}
}