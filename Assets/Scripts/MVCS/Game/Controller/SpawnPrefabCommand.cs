using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using strange.extensions.pool.api;

namespace TeamBrookvale.Game
{
	public class SpawnPrefabCommand : Command
	{
		// Keep reference to each possible GameObject, to avoid frequent calls to Resources.Load.
		[Inject]
		public ISpawnPrefabModel model {get;set;}

		// This is one of our event parameters, which tells the command which prefab to spawn.
		[Inject]
		public SpawnPrefabModel.Prefab prefabType { get; set; }

		// This is our second event parameter, which tells where the prefab should be positioned in the scene.
		[Inject]
		public Vector2 position { get; set; }

		// How long this prefab should live for
		[Inject]
		public float lifeSpan {get;set;}

		// IRoutineRunner allows use to run Coroutines without having to directly implement the MonoBehavior interface.
		[Inject]
		public IRoutineRunner routineRunner { get; set; }

		public static GameObject lastSpawnedGameObject;

		public override void Execute()
		{
			// parent of spawned prefabs
			model.parent = model.parent ?? new GameObject ("SpawnedPrefabs").transform;

			// this is the reference to the prefab that we will use 
			GameObject prefab = model.prefabDictionary [prefabType];

			// we want the bottom center point to be at the touch so the sprite has to be moved up by half of it's vertical size
			float adjustY = prefab.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
			
			// calculate new position
			position = position + new Vector2 (0, adjustY);

			// instantiate item
			GameObject instance = GameObject.Instantiate (prefab, position, Quaternion.identity) as GameObject;

			// define parent
			instance.transform.parent = model.parent;

			// Destroy delayed if specified by lifespan
			if (lifeSpan > 0)
				routineRunner.StartCoroutine (DestroyDelayed (instance));

			// assign static variable
			lastSpawnedGameObject = instance;
		}

		private IEnumerator DestroyDelayed (GameObject instance)
		{
			// Destroy after a certain amount of time
			yield return new WaitForSeconds (lifeSpan);
			GameObject.Destroy (instance);
		}
	}
}