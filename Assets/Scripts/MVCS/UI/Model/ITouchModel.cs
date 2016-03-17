using System;
using UnityEngine;

namespace TeamBrookvale.UI
{
	public interface ITouchModel
	{
		void fireTouchEvent (TouchFSM.Events e, TouchScreenPosition p);
		TouchFSM.States getCurrentState();
		void guiItemRectRegister (Rect r, bool register);
		bool isGuiItem (TouchScreenPosition t);
	}
}