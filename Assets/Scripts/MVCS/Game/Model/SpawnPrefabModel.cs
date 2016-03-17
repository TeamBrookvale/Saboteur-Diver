using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TeamBrookvale.Game
{
	public class SpawnPrefabModel : ISpawnPrefabModel
	{
		public enum Prefab {
			SmokeBomb,
			SmokeBombSplash,
			TimeBombMounted,
			GaugeEllipse,
			MuzzleFlash
		}

		public IDictionary<Prefab, GameObject> prefabDictionary {get;set;}

		// Parent of spawned prefabs
		public Transform parent {get;set;}

		public SpawnPrefabModel ()
		{
			// Instantiate dictionary
			prefabDictionary = new Dictionary<Prefab, GameObject>();

			// Fill up the dictionary
			foreach (Prefab prefabType in (Prefab[]) Enum.GetValues(typeof(Prefab)))
			{
				// load prefab
				GameObject prefab = Resources.Load<GameObject> ("Prefabs/" + prefabType.ToString());
				
				// if could not be loaded
				if (prefab == null)
					Debug.LogError ("Prefabs/" + prefabType.ToString() + " could not be loaded.");
				
				// add it to the dictionary
				prefabDictionary.Add(prefabType, prefab);
			}
		}
	}
}