using UnityEngine;
using System.Collections;

using strange.extensions.command.impl;

namespace TeamBrookvale.Game
{
	public class SpotlightPlayerNoticedCommand : Command
	{
		// model injections
		[Inject]
		public ISpotlightDictionary spotlightDict { get;set; }


		// mediator injections
		[Inject]
		public int instanceID { get; set;}

		// signal injections
		[Inject]
		public SingletonSoundFxSignal singletonSoundFxSignal {get;set;}


		// fire this signal when the player is caught
		[Inject]
		public GameLevelFailedSignal gameLevelFailedSignal {get;set;}

		[Inject]
		public IRoutineRunner routineRunner { get; set; }

		// spawn muzzle flash
		[Inject]
		public SpawnPrefabSignal spawnPrefabSignal {get;set;}

		// count player noticed events
		[Inject]
		public IGameModel gameModel {get;set;}

		public override void Execute ()
		{
			// player visible
			if (spotlightDict.spotlights[instanceID].isPlayerNoticed)
			{
				// count player noticed events
				gameModel.numberOfSpotLightPlayerNoticedCommands++;

				// play danger sound but only for the first spotlight noticing the player
				if (spotlightDict.numberOfSpotlightsPlayerNoticed == 0)
					singletonSoundFxSignal.Dispatch(SoundFx.DangerZone, SingletonSoundFxCmd.Play);

				// count number of spotlights currently seeing the player
				spotlightDict.numberOfSpotlightsPlayerNoticed++;

				// If still visible later then game over
				routineRunner.StartCoroutine (CheckLaterIfStillVisible (instanceID));

				Retain();
			}

			// player not visible anymore
			else
			{
				// count number of spotlights currently seeing the player
				spotlightDict.numberOfSpotlightsPlayerNoticed--;

				// if no spotlights can see the diver then dipatch and the alert audio can fade
				if (!spotlightDict.anySpotlightsCurrentlyNoticedPlayer() && spotlightDict.numberOfSpotlightsPlayerNoticed == 0)
				{
					singletonSoundFxSignal.Dispatch(SoundFx.DangerZone, SingletonSoundFxCmd.FadeOut);
				}
			}
		}

		private IEnumerator CheckLaterIfStillVisible (int instanceID)
		{
			yield return new WaitForSeconds (Const.SpotlightCatchTimeThreshold);

			// only check if the diver is still visible even if disappeared and reappeared during
			// the SpotLightView.catchTimeThreshold time limit
			if (spotlightDict.spotlights[instanceID].isPlayerNoticed)
			{
				spawnPrefabSignal.Dispatch (
					SpawnPrefabModel.Prefab.MuzzleFlash,
					spotlightDict.spotlights[instanceID].cachedPosition,
					0);

				gameLevelFailedSignal.Dispatch();
			}

			Release();
		}

	}
}