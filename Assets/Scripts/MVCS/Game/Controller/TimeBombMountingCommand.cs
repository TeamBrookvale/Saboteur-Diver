using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

namespace TeamBrookvale.Game
{
	public class TimeBombMountingCommand : Command
	{
		[Inject]
		public bool startOrStop {get;set;}

		[Inject]
		public SpawnPrefabSignal spawnPrefabSignal {get;set;}

		[Inject]
		public IDamBridgeModel damBridgeModel {get;set;}

		[Inject]
		public InventoryEventFireSignal inventoryEventFireSignal {get;set;}

		[Inject]
		public AllShipsSunkOrBombsMountedSignal allShipsSunkOrBombsMountedSignal {get;set;}

		[Inject]
		public IRoutineRunner routineRunner {get;set;}

		[Inject]
		public SingletonSoundFxSignal singletonSoundFxSignal {get;set;}

		static float lastMountingStartTime;
		static GameObject gaugeEllipse;

		public override void Execute ()
		{
			// if mounting attempt started
			if (startOrStop)
			{
				// spawn gauge prefab
				spawnPrefabSignal.Dispatch (
					SpawnPrefabModel.Prefab.GaugeEllipse,
					damBridgeModel.lastCollisionTouchScreenPosition.world + new Vector2 (0, .5f),
					0);
				
				// assign the prefab to a static variable
				gaugeEllipse = SpawnPrefabCommand.lastSpawnedGameObject;

				// enable the sprite
				gaugeEllipse.GetComponent<SpriteRenderer> ().enabled = true;

				// count time
				lastMountingStartTime = Time.time;

				// play audio
				singletonSoundFxSignal.Dispatch (SoundFx.BombMounting, SingletonSoundFxCmd.Play);

				// update gauge meter on every frame
				GameObject gaugeMeter = gaugeEllipse.transform.Find("GaugeMeter").gameObject;
				if (gaugeMeter == null) Debug.LogError("GaugeMeter not found");
				routineRunner.StartCoroutine (GaugeMeterUpdate (gaugeMeter));
			}

			// if mounting attempt finished
			else
			{
				// if finished with the attempt then destroy
				GameObject.Destroy (gaugeEllipse);

				// decide if enough time elapsed to mount the bomb
				if (lastMountingStartTime + Const.timeNeededToMountTimeBomb < Time.time)
				{
					// add bomb to the list of bomb positions
					damBridgeModel.bombsMounted.Add (damBridgeModel.lastCollisionTouchScreenPosition.world);

					// if all bombs are mounted then the diver should swim back to the getaway boat
					if (damBridgeModel.areAllBombsMounted ())
						allShipsSunkOrBombsMountedSignal.Dispatch ();

					// spawn bomb sprite
					spawnPrefabSignal.Dispatch (
						SpawnPrefabModel.Prefab.TimeBombMounted,
						damBridgeModel.lastCollisionTouchScreenPosition.world,
						0);
					
					// dispatch inventory signal so no new bombs can be mounted straightaway
					inventoryEventFireSignal.Dispatch (
						InventoryModel.Events.AbandonShipBridgeOrDam,
						damBridgeModel.lastCollisionTouchScreenPosition);
				}

				// stop audio
				singletonSoundFxSignal.Dispatch (SoundFx.BombMounting, SingletonSoundFxCmd.Stop);
			}
		}

		IEnumerator GaugeMeterUpdate (GameObject gaugeMeter)
		{
			// once gaugeEllipse is destroyed then do not continue anymore
			while (gaugeEllipse != null)
			{
				// update gaugemeter's size
				gaugeMeter.transform.localScale = new Vector3 (
					1,
					(Time.time - lastMountingStartTime) / Const.timeNeededToMountTimeBomb * 4,
					1);

				// if enough time lapsed then change the meter to the tick icon
				if (lastMountingStartTime + Const.timeNeededToMountTimeBomb < Time.time)
				{
					gaugeMeter.GetComponent<SpriteRenderer>().enabled = false;
					Sprite gaugeSuccessSprite = Resources.Load<Sprite> ("Sprites/GaugeSuccess");
					if (gaugeSuccessSprite == null) Debug.LogError("Sprites/GaugeSuccess does not exist");
					gaugeEllipse.GetComponent<SpriteRenderer>().sprite = gaugeSuccessSprite;
				}

				// wait for the next frame
				yield return new WaitForEndOfFrame ();
			}
		}
	}
}