using UnityEngine;

namespace TeamBrookvale.Game
{
	public class InvItem
	{
		// types of items the player may have, important that SmokeBombInactive is the first one as it is the default icon
		public enum IDType {
			SmokeBombInactive,
			SmokeBombActive,
			DrillActive,
			DrillInactive,
			TimeBombActive,
			TimeBombInactive,
			TimeBombInactiveTick,
			UBoat
		};

		// current item for this instance i.e. SmokeBomb
		public IDType id {get; private set;} 

		// refers to the file name of the icon
		public Texture2D icon {get; private set;}

		// prefab's opinionated path
		public GameObject prefab {get; private set;}

		// i.e. number of smokebombs available
		int quantity;

		public InvItem (IDType id)
		{
			this.id = id;

			// generate path i.e. SmokeBomb becomes UI/SmokeBombIcon
			string iconPath = "UI/" + id.ToString() + "Icon";
			string prefabPath = "Prefabs/" + id.ToString();

			// load icon
			icon = Resources.Load<Texture2D> (iconPath);
			prefab = Resources.Load<GameObject> (prefabPath);

			// error if icon cannot be loaded
			if (icon == null)
				Debug.LogError (iconPath + " icon cannot be loaded");	
		}
	}
}