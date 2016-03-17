using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class UBoatView : View
	{
		internal UBoatPlayerCloseSignal uBoatPlayerCloseSignal = new UBoatPlayerCloseSignal ();

		static string[] spriteNames = {"Right", "RightUp", "Up", "LeftUp", "Left", "LeftDown", "Down", "RightDown"};
		static Sprite[] sprites;

		// set by mediator through init
		IUBoatModel model;
		Func<Vector2> playerModelGetCachedPosition;

		// local state variables
		Vector3 prevPosition;
		float dirAngle;
		bool isPlayerClose;

		internal void init (IUBoatModel model, Func<Vector2> playerModelgetCachedPosition)
		{
			this.model = model;
			this.playerModelGetCachedPosition = playerModelgetCachedPosition;

			// UBoat sprite betöltés prototípus
			if (sprites == null)
			{
				sprites = new Sprite[spriteNames.Length];
				for (int i=0; i < spriteNames.Length; i++)
				{
					sprites[i] = (Sprite) Resources.Load("Sprites/UBoat/UBoat" + spriteNames[i], typeof (Sprite));
					if (sprites[i] == null) Debug.LogError("Sprites/UBoat/UBoat" + spriteNames[i] + "  is missing");
				}
			}
		}
		
		void Update ()
		{
			float playerDistance = ((Vector2) transform.position - playerModelGetCachedPosition()).magnitude;
			
			// schmitt trigger, if player gets close or far then dispatch signal
			if (!isPlayerClose && playerDistance < 1 ||
			    isPlayerClose && playerDistance > 1.1f)
			{
				isPlayerClose = !isPlayerClose;
				uBoatPlayerCloseSignal.Dispatch (isPlayerClose);
			}

			if (model.isPlayerEmbarked)
			{
				// UBoat forgás prototípus
				transform.position = playerModelGetCachedPosition ();
				if (transform.position != prevPosition)
				{
					Vector3 dirVector = (transform.position - prevPosition).normalized;
					dirAngle = Mathf.Atan2 (dirVector.y, dirVector.x) * 180 / Mathf.PI;
					dirAngle = Mathf.LerpAngle(transform.eulerAngles.z, dirAngle, 0.1f);
					transform.localEulerAngles = new Vector3 (0, 0, dirAngle);
					dirAngle += 22.5f;
					dirAngle %= 360;
					GetComponent<SpriteRenderer>().sprite = (Sprite) sprites[(int)(dirAngle / 45)];
					prevPosition = transform.position;
				}
			}
		}
	}
}