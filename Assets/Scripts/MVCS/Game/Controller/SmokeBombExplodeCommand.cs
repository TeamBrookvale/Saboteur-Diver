using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using strange.extensions.pool.api;

namespace TeamBrookvale.Game
{
	public class SmokeBombExplodeCommand : Command
	{
		[Inject]
		public TouchScreenPosition touchScreenPosition { get; set; }

		[Inject]
		public SpawnPrefabSignal spawnPrefabSignal {get;set;}

		[Inject]
		public PlaySoundFxSignal playSoundFxSignal {get;set;}

		[Inject]
		public SmokeBombExplodedSignal smokeBombExplodedSignal {get;set;}

		[Inject]
		public IInventoryModel inventoryModel {get;set;}

		public override void Execute()
		{
			// one less smoke bomb is available
			inventoryModel.smokeBombsLeft--;

			// spawn sprites and audio FX
			spawnPrefabSignal.Dispatch 	(SpawnPrefabModel.Prefab.SmokeBomb, 		touchScreenPosition.world + Vector2.up * 1.3f, Const.SpawnAnimationLifeSpan);
			spawnPrefabSignal.Dispatch 	(SpawnPrefabModel.Prefab.SmokeBombSplash,	touchScreenPosition.world, Const.SpawnAnimationLifeSpan);
			playSoundFxSignal.Dispatch 	(SoundFx.SmokeBombSplash, 					touchScreenPosition);
			playSoundFxSignal.Dispatch 	(SoundFx.SmokeBombHissing, 					touchScreenPosition);

			// dispatch singleton signal so spotlights will know where to look
			smokeBombExplodedSignal.Dispatch (touchScreenPosition);
		}
	}
}