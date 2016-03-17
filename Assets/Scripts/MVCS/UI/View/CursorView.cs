using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.UI
{
	public class CursorView : View
	{
		internal void playClickAnimationAt (Vector2 clickWorldPosition, bool isRunning)
		{
			if (isRunning)
			{
				transform.position = clickWorldPosition;
				GetComponent<Animator>().SetTrigger("Click");
			}
		}
	}
}