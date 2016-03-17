using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;

namespace TeamBrookvale.Game
{
	public class GameBootstrap : ContextView
	{
		void Awake()
		{
			context = new GameContext(this);
		}
	}
}