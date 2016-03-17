using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBrookvale.UI
{
	public class StatusBarModel : IStatusBarModel
	{
		[Inject]
		public IRoutineRunner routineRunner {get;set;}

		List<string> messages = new List<string>();

		public float statusBarToggledTime {get;set;}

		public StatusBarModel ()
		{
			statusBarToggledTime = -999;
		}

		public void addNewMessage (string message)
		{
			if (messages.Count == 0)
				statusBarToggledTime = Time.time;

			if (!messages.Contains (message))
			{
				messages.Add (message);

				if (!Const.isStatusBarAlwaysVisible)
					#pragma warning disable 162
					routineRunner.StartCoroutine (RemoveThisMessageDelayed ());
					#pragma warning restore 162
			}
		}

		public void removeMessage (string message)
		{
			if (messages.Contains (message))
				messages.Remove (message);
		}

		public string getCurrentMessage ()
		{
			#pragma warning disable 429
			return Const.isStatusBarAlwaysVisible
				? ((messages.Count == 0) ? "" : messages[messages.Count - 1])
				: ((messages.Count == 0) ? "" : messages[0]);
			#pragma warning restore 429
		}

		public bool isStatusBarVisible ()
		{
			return messages.Count > 0;
		}

		IEnumerator RemoveThisMessageDelayed ()
		{
			yield return new WaitForSeconds (Const.StatusBarMessageTimeLenght);
			messages.RemoveAt (0);

			if (messages.Count == 0)
				statusBarToggledTime = Time.time;
		}
	}
}