using UnityEngine;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class LevelBoundsView : View
	{
		internal void init (ILevelBoundsModel model)
		{
			foreach (Transform child in transform)
			{
				model.x_min = Mathf.Min (model.x_min, child.position.x);
				model.y_min = Mathf.Min (model.y_min, child.position.y);
				model.x_max = Mathf.Max (model.x_max, child.position.x);
				model.y_max = Mathf.Max (model.y_max, child.position.y);
			}
		}
	}
}