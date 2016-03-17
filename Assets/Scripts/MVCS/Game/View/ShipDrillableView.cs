using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.Game
{
	public class ShipDrillableView : View {

		internal ShipSunkSignal _shipSunkSignal = new ShipSunkSignal ();
		internal ShipsHealthReachedZeroSignal _shipsHealthReachedZeroSignal = new ShipsHealthReachedZeroSignal ();

		IShipDrillableModel shipDrillableModel; // all drillableships
		DrillableShipProperties thisShipsProperties;
		bool drilledBefore = false;
		public bool isSunk {get; private set;}
		Transform gaugeMeter, airBubblesSink;
		Sprite gaugeSuccessSprite;
		bool isLargeShip; // if the ship is large then drill slower
		AudioClip bodyHittingWoodClip;

		public void init (IShipDrillableModel shipDrillableModel, DrillableShipProperties thisShipsProperties)
		{
			this.shipDrillableModel = shipDrillableModel;
			this.thisShipsProperties = thisShipsProperties;

			ShipOrZeppelinRocking shipOrZeppelinRocking = gameObject.AddComponent<ShipOrZeppelinRocking> () as ShipOrZeppelinRocking;
			shipOrZeppelinRocking.rockingSpeed = Const.ShipDrillableRockingSpeed;
			shipOrZeppelinRocking.rockingAmplitude = Const.ShipDrillableRockingAmplitude;

			gaugeMeter 		= transform.Find("GaugeEllipse/GaugeMeter");
			airBubblesSink 	= transform.Find("AirBubblesSink");
			gaugeSuccessSprite  = Resources.Load<Sprite> ("Sprites/GaugeSuccess");
			bodyHittingWoodClip = Resources.Load<AudioClip> ("Fx/BodyHittingWood");

			airBubblesSink.GetComponent<SpriteRenderer>().enabled = false;
			airBubblesSink.GetComponent<Animator>().enabled = false;

			if (gaugeMeter 		== null) Debug.LogError("GaugeMeter not found");
			if (airBubblesSink	== null) Debug.LogError("AirBubblesSink not found");
			if (gaugeSuccessSprite == null) Debug.LogError("Sprites/GaugeSuccess does not found");
			if (bodyHittingWoodClip== null) Debug.LogError("Fx/BodyHittingWoodClip not found");

			// is the ship large then drill slower
			isLargeShip = gameObject.name.Contains ("Large");

			// add body hitting view sound
			AudioSource a = gameObject.AddComponent<AudioSource> ();
			a.playOnAwake = a.loop = false;
			a.clip = bodyHittingWoodClip;
			gameObject.AddComponent<PlayAudioOnCollision> ();
		}

		void Update ()
		{
			// only run this once when ship is sunk
			if (thisShipsProperties.health < 0)
			{
				thisShipsProperties.health = 0;
				_shipsHealthReachedZeroSignal.Dispatch ();
			}
		}

		void OnCollisionEnter2D (Collision2D collision)
		{
			shipDrillableModel.lastDrilledShipProperties = thisShipsProperties;
		}

		// return true if drillable i.e. not sink already
		public bool drill (Vector2 drillPosition)
		{
			// show gauge on first drill
			if (!drilledBefore)
			{
				drilledBefore = true;
				gaugeMeter.transform.parent.GetComponent<SpriteRenderer>().enabled = true;
			}

			// if not sunk yet increase gauge while drilling
			thisShipsProperties.health -= .25f * Time.deltaTime * 
				(isLargeShip ? Const.ShipDrillableLargeDrillSpeedFactor : 1);

			// gauge meter
			gaugeMeter.transform.localScale = new Vector3 (1, (1 - thisShipsProperties.health) * 4, 1);

			if (thisShipsProperties.health < 0)
			{
				isSunk = true;

				// report to mediator
				_shipSunkSignal.Dispatch (GetInstanceID(), drillPosition);

				// change gauge meter to complete
				gaugeMeter.transform.GetComponent<SpriteRenderer>().enabled = false;
				gaugeMeter.transform.parent.GetComponent<SpriteRenderer>().sprite = gaugeSuccessSprite;

				// play AirBubblesSink
				airBubblesSink.GetComponent<SpriteRenderer>().enabled = true;
				airBubblesSink.GetComponent<Animator>().enabled = true;

				airBubblesSink.transform.position = drillPosition - Vector2.up * .3f;
			}

			// returns true if currently being drilled i.e. not sink yet
			return !isSunk;
		}
	}
}