using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class GameModelView : View
	{
		IGameModel model;

		internal void init (IGameModel model)
		{
			this.model = model;
		}

		void Update ()
		{
			Time.timeScale = 
				(model.pauseStatus == PauseStatus.PAUSE ? 0 : 1);
		}
	}
}