using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;

namespace TeamBrookvale.SelectLevel
{
	public class SelectLevelBootstrap : ContextView
	{
		void Awake()
		{
			context = new SelectLevelContext(this);
		}
	}
}