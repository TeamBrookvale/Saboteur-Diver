using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TeamBrookvale.UI;

namespace TeamBrookvale.Game
{
	public static class TBUtil
	{
		// http://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect

		public static bool LineSegmentsIntersect(Vector2 p, Vector2 q, Vector2 r, Vector2 s)
		{
			return _LineSegmentsIntersect (p.x, p.y, q.x, q.y, r.x, r.y, s.x, s.y);
		}

		private static bool _LineSegmentsIntersect (float p0_x, float p0_y, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y)
		{
			float s1_x, s1_y, s2_x, s2_y;
			s1_x = p1_x - p0_x;
			s1_y = p1_y - p0_y;
			s2_x = p3_x - p2_x;
			s2_y = p3_y - p2_y;

			float s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / (-s2_x * s1_y + s1_x * s2_y);
			float t = ( s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / (-s2_x * s1_y + s1_x * s2_y); 

			//Debug.Log (s + "  " + t + "   " + (s >= 0 && s <= 1 && t >= 0 && t <= 1).ToString());
			//Debug.DrawLine (new Vector3(p0_x,p0_y), new Vector3(p1_x,p1_y), Color.red, 1f, false);
			//Debug.DrawLine (new Vector3(p2_x,p2_y), new Vector3(p3_x,p3_y), Color.red, 1f, false);

			return (s >= 0 && s <= 1 && t >= 0 && t <= 1);
		}

		public static bool IsTouchOnWater (TouchScreenPosition touchScreenPosition)
		{
			return 
				Physics2D.GetRayIntersection(
					new Ray (
						(Vector3) touchScreenPosition.world - Vector3.forward,
						Vector3.forward),
					Mathf.Infinity
				).transform == null;
		}

		public static void TryAddDict<K,V> (Dictionary<K,V> d, K k, V v)
		{
			if (!d.ContainsKey (k))
				d.Add (k, v);
		}

		public static Rect RectWithAnchorAndTexture (TutorialModel.Anchor anchor, Texture2D texture)
		{
			const int margin = 20;
			switch (anchor)
			{
			case TutorialModel.Anchor.Center :
				return new Rect ((Screen.width - texture.width) / 2,
				                 (Screen.height- texture.height) / 2,
				                 texture.width,
				                 texture.height);
			
			case TutorialModel.Anchor.PauseButton :
				return new Rect (PauseIconView.getIconRect.xMin - texture.width - margin,
				                 PauseIconView.getIconRect.yMin,
				                 texture.width,
				                 texture.height);

			case TutorialModel.Anchor.ProgressCounter :
				return new Rect (ProgressCounterIconView.getIconRect.xMin - texture.width - margin,
				                 ProgressCounterIconView.getIconRect.center.y - texture.height / 2,
				                 texture.width,
				                 texture.height);
			
			case TutorialModel.Anchor.ToolButton :
				return new Rect (InventoryIconView.getIconRect.xMin - texture.width - margin,
				                 InventoryIconView.getIconRect.yMax - texture.height,
				                 texture.width,
				                 texture.height);

			default: break;
			}

			return new Rect (0,0,0,0);
		}

		public static string LevelNumberToKey (int i)
		{
			return "Level" + i.ToString("00");
		}

		public static float ScreenDPI; // set by init script
		public static float ScreenHeight; // set by init script

		public static int CalcFontSize (int fontSize)
		{
			//Debug.Log (Screen.height);
			return (int) (Screen.height * fontSize * .00175f); // ppi iPad-132; iPhone-163; iPad3-264; iPhone4-326
		}
	}
}