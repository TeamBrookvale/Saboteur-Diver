using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.UI
{
	public class InGameMenuBackgroundView : View
	{
		internal void init ()
		{
			// play fadein on startup
			GetComponent<Animator> ().SetTrigger ("FadeOut");
		}

		internal void OnInGameFadeSignal (bool inOrOut)
		{
			GetComponent<Animator> ().SetTrigger (inOrOut ? "FadeIn" : "FadeOut");
		}
	}
}