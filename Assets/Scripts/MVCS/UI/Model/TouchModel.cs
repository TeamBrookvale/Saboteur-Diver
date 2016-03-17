using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBrookvale.UI
{
	public class TouchModel : ITouchModel
	{
		[Inject]
		public ITouchFSM touchFSM {get;set;}

		// Register all GUI item's boundaries. List of GUI item's boundaries. If they are clicked than the player should not move
		IList<Rect> guiRectList = new List<Rect>();

		// State machine's current state
		TouchFSM.States currentState;

		public void guiItemRectRegister (Rect guiRect, bool register)
		{
			if (register)
				guiRectList.Add (guiRect);
			else
				guiRectList.Remove (guiRect);
		}

		public void fireTouchEvent (TouchFSM.Events e, TouchScreenPosition t)
		{
			currentState = touchFSM.fireTouchEvent (e, t);
		}

		public TouchFSM.States getCurrentState ()
		{
			return currentState;
		}

		// checks if the touch is within any GUI item or not
		public bool isGuiItem (TouchScreenPosition tsp)
		{
			// invert y as screen is bottom-up, GUI is top-down
			tsp.pixel.y = Screen.height - tsp.pixel.y;

			foreach (Rect guiRect in guiRectList)
			{
				// check if the touch is within a GUI item
				if (guiRect.Contains (tsp.pixel))
					return true;
			}

			return false;
		}
	}
}