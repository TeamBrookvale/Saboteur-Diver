using UnityEngine;

namespace TeamBrookvale.Game
{
	public interface ILevelBoundsModel
	{
		float x_min {get;set;}
		float x_max {get;set;}

		float y_min {get;set;}
		float y_max {get;set;}

	}
}