using UnityEngine;

namespace TeamBrookvale.Game
{
	public interface ISmokeBombCircleModel
	{
		GameObject smokeBombCircleGameObject {get;set;}
		bool isTouchWithinEllipse (TouchScreenPosition t);
	}
}