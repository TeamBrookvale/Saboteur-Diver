using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;

namespace TeamBrookvale.UI
{
	public class UIBootstrap : ContextView
	{
		void Awake()
		{

#if UNITY_EDITOR
			if (GameObject.Find ("GameContext") == null)
				Application.LoadLevel ("GameScene");
			else
#endif

			context = new UIContext(this);
		}
	}
}