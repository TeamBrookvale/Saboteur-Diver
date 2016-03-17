using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBrookvale.Game
{
	public interface ISpawnPrefabModel
	{
		// Keep reference to each possible GameObject to avoid frequent calls to Resources.Load
		IDictionary<SpawnPrefabModel.Prefab, GameObject> prefabDictionary {get;set;}

		// Parent of spawned prefabs
		Transform parent {get;set;}
	}
}