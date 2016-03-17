using UnityEngine;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TeamBrookvale.UI
{
	public class TutorialView : View
	{
		internal GUIElemRectRegisterSignal _guiElemRectRegisterSignal = new GUIElemRectRegisterSignal ();

		List<TeamBrookvale.Game.TutorialModel.TutorialEntry> tutorialEntries;

		int prevCounterValue = -1;
		int counter;
		Rect currentRect;
		Rect wholeScreenRect;
		bool isNextDelayedInProgress;
		float currentRectShownAt;

		internal void init (List<TeamBrookvale.Game.TutorialModel.TutorialEntry> tutorialEntries)
		{
			this.tutorialEntries = tutorialEntries;

			wholeScreenRect = new Rect (0,0, Screen.width, Screen.height);

			currentRectShownAt = Time.time;
		}

		void OnGUI ()
		{
			if (Time.timeSinceLevelLoad > 3f && counter < tutorialEntries.Count)
			{
				// dispatch gui elem rect register signal once
				if (prevCounterValue != counter)
				{
					prevCounterValue = counter;
					currentRect = TeamBrookvale.Game.TBUtil.RectWithAnchorAndTexture (tutorialEntries[counter].anchor, tutorialEntries[counter].texture);
					_guiElemRectRegisterSignal.Dispatch (wholeScreenRect, true);
					currentRectShownAt = Time.time;
				}

				// show the button, if pushed then deregister it as gui item and increase the counter
				if (GUI.RepeatButton (
						currentRect,
						tutorialEntries[counter].texture,
						GUI.skin.label))
					StartCoroutine (NextDelayed ());
			}
		}
	
		void Update ()
		{
			if (currentRectShownAt + 10 < Time.time && counter < tutorialEntries.Count)
				StartCoroutine (NextDelayed ());
		}

		// if button push then deregister it as gui item and increase the counter
		IEnumerator NextDelayed ()
		{
			if (!isNextDelayedInProgress)
			{
				isNextDelayedInProgress = true;
				yield return new WaitForSeconds (.2f);
				_guiElemRectRegisterSignal.Dispatch (wholeScreenRect, false);
				counter++;
				isNextDelayedInProgress = false;
			}
		}
	}
}