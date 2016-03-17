using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class MuzzleFlashView : View
	{
		internal void init (Vector2 playerPosition)
		{
			Vector2 dir = playerPosition - (Vector2) transform.position;
			transform.eulerAngles = Vector3.forward * Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		}
	}
}