using UnityEngine;

namespace TeamBrookvale.Game
{
	public class SmokeBombCircleModel : ISmokeBombCircleModel
	{
		public GameObject smokeBombCircleGameObject {get;set;}

		public bool isTouchWithinEllipse (TouchScreenPosition t)
		{
			// if the touch is within the smoke bomb ellipse
			GameObject e = smokeBombCircleGameObject;
			
			float rx = e.GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
			float ry = e.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
			
			float h = e.transform.position.x;
			float k = e.transform.position.y;
			
			float x = t.world.x;
			float y = t.world.y;
			
			// check if the touch is within the ellipse
			return (x-h)*(x-h)/rx/rx + (y-k)*(y-k)/ry/ry < 1;
		}
	}
}