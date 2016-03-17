using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace TeamBrookvale.Game
{
	public class DeserializedLevelsLoaderModel : IDeserializedLevelsLoaderModel
	{
		// Cache all items with locations
		List<GOProperties> sceneItemsList;

		public class GOProperties
		{
			public GameObject	prefab;
			public float		x;
			public float		y;
			public float		rot;
			public float		scale_x;
			public float		scale_y;
			public float		speed;
			public float		spotmedianangle;
		}
		
		// Cache prefabs in prefabDict
		Dictionary<string,GameObject> prefabPool;

		public Transform parentOfXmlItems {get; private set;}

		// Levels deserialized
		DeserializedLevels deserializedLevels;
		
		const string prefabsFolder = "Prefabs/";
		
		public const string xmlItemsGameObjectName = "XMLItems";

		public void generateItems (int startLevel = -1)
		{
			prefabPool = new Dictionary<string, GameObject>();
			sceneItemsList = new List<GOProperties>();

			// if the XmlItems gameobject folder remained in the Hierarcy, then delete it
			while (GameObject.Find (xmlItemsGameObjectName) != null)
				MonoBehaviour.DestroyImmediate(GameObject.Find (xmlItemsGameObjectName));
			
			parentOfXmlItems = new GameObject(xmlItemsGameObjectName).transform;

			parentOfXmlItems.parent = GameObject.Find ("GameContext").transform;

			deserializedLevels = XmlIO.LoadXml<DeserializedLevels>("Levels");

			// if startlevel is in the XML i.e. <Developer StartLevel="3" /> then get level from there
			// otherwise start with level 1
			if (startLevel == -1) startLevel = int.Parse (deserializedLevels.developer.startLevel);

			DeserializedLevels.Level currentLevel = deserializedLevels.levels[startLevel-1];

			// spagetti coding set player, camera position and level bounds
			setPos2D (
				GameObject.Find ("PlayerView"),
				new Vector2 (toFloatZeroIfNull (currentLevel.playerx), toFloatZeroIfNull (currentLevel.playery)));
			setPos2D (
				GameObject.Find ("CameraView"),
				new Vector2 (toFloatZeroIfNull (currentLevel.playerx), toFloatZeroIfNull (currentLevel.playery)));

			// set level's bounds
	#if UNITY_EDITOR
			try {
	#endif
			setLevelBounds (
				parentOfXmlItems,
				float.Parse (currentLevel.bound_x_min),
				float.Parse (currentLevel.bound_y_min),
				float.Parse (currentLevel.bound_x_max),
				float.Parse (currentLevel.bound_y_max));

	#if UNITY_EDITOR
			} catch { Debug.LogError ("Level Bounds must be set in levels.xml"); }
	#endif
			// <Item prefab="Chair" x="1" y="10" rot="90" />
			foreach (DeserializedLevels.Item deserializedItem in currentLevel.items)
			{
				// caching prefabString i.e. "phone"
				string prefabString = deserializedItem.prefab;
				
				// if the prefab in the item XmlNode has not been loaded then add it to the prefabsDict dictionary,
				if (!prefabPool.ContainsKey(prefabString))
				{
					// load prefab
					GameObject prefabObject = Resources.Load (prefabsFolder + prefabString, typeof(GameObject)) as GameObject;
					
					// if unsuccesful, error message and jump to next in the foreach loop
					if (prefabObject == null)
					{
						Debug.LogError ("Prefab \"" + prefabString + "\" does not exists.");
						continue;
					}
					
					// otherwise add to dictionary
					prefabPool.Add (prefabString, prefabObject);
				}
				
				GOProperties item = new GOProperties();
				item.prefab = prefabPool[prefabString];
				item.x 		= toFloatZeroIfNull(deserializedItem.x);
				item.y 		= toFloatZeroIfNull(deserializedItem.y);
				item.rot 	= toFloatZeroIfNull(deserializedItem.rot);
				item.scale_x= toFloatOneIfNull(deserializedItem.scale_x);
				item.scale_y= toFloatOneIfNull(deserializedItem.scale_y);
				item.speed  = toFloatOneIfNull(deserializedItem.speed);
				item.spotmedianangle = toFloatZeroIfNull(deserializedItem.spotmedianangle);

				sceneItemsList.Add (item);
			}
			
			// Finally instantiate all items
			foreach (GOProperties item in sceneItemsList)
			{

				// TODO load height coordinate from a directory
				GameObject newGameObject = MonoBehaviour.Instantiate(item.prefab) as GameObject;

				// set position
				setPos2D (newGameObject, new Vector2(item.x, item.y));

				// set rotation
				setRot2D (newGameObject, item.rot);

				// set scale
				newGameObject.transform.localScale = new Vector3 (item.scale_x, item.scale_y, 1);

				// set speed
				if (newGameObject.GetComponent<ShipPatrolView>() != null)
					newGameObject.GetComponent<ShipPatrolView>().speed = item.speed;
				if (newGameObject.GetComponent<SoldierWalkingView>() != null)
					newGameObject.GetComponent<SoldierWalkingView>().speed = item.speed;

				// set spotlight median angle
				if (newGameObject.GetComponentInChildren<SpotLightView>() != null)
					newGameObject.GetComponentInChildren<SpotLightView>().spotMedianAngle = item.spotmedianangle;

				// set parent
				newGameObject.transform.parent = parentOfXmlItems;
			}
		}

		// DONE, these are only helper functions below

		// if no value then return zero or one, otherwise convert to float
		float toFloatZeroIfNull (string value) { return value == null ? 0 : float.Parse(value);	}
		float toFloatOneIfNull  (string value) { return value == null ? 1 : float.Parse(value);	}

		void setPos2D(GameObject g, Vector2 pos)
		{
			g.transform.position = new Vector3 (
				pos.x,
				pos.y,
				g.transform.position.z
			);
		}

		void setRot2D(GameObject g, float rot)
		{
			Quaternion rotation = Quaternion.identity;
			rotation.eulerAngles = new Vector3(0, 0, rot);
			g.transform.localRotation = rotation;
		}

		void setLevelBounds (Transform parent, float x_min, float y_min, float x_max, float y_max)
		{
			// create a folder for the four levelbounds
			GameObject subParent = new GameObject ("LevelBounds");

			// parent should be the XMLitems folder
			subParent.transform.parent = parent;

			// initialize the four levelbounds
			GameObject[] levelBounds = new GameObject[4];
			
			for (int i = 0; i < 4; i++)
			{
				levelBounds[i] = new GameObject ("LevelBound" + i.ToString());
				levelBounds[i].transform.parent = subParent.transform;
				levelBounds[i].AddComponent<BoxCollider2D>();
			}
			
			levelBounds[0].transform.position = new Vector3 (x_min, 0);
			levelBounds[1].transform.position = new Vector3 (x_max, 0);
			levelBounds[2].transform.position = new Vector3 (0, y_min);
			levelBounds[3].transform.position = new Vector3 (0, y_max);
			
			levelBounds[0].transform.localScale = new Vector3 (1, 1000, 0);
			levelBounds[1].transform.localScale = new Vector3 (1, 1000, 0);
			levelBounds[2].transform.localScale = new Vector3 (1000, 1, 0);
			levelBounds[3].transform.localScale = new Vector3 (1000, 1, 0);

			// Add the view to the sub parent levelbounds
			subParent.AddComponent<LevelBoundsView> ();
		}

	}
}