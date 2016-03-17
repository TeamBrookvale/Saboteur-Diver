using UnityEngine;
using TeamBrookvale.Game;

namespace TeamBrookvale.SelectLevel
{
	public class SelectLevelModel : ISelectLevelModel
	{
		public Texture2D LevelIconBackground			{get; private set;}
		public Texture2D LevelIconBackgroundCompleted	{get; private set;}
		public Texture2D LevelIconLocked				{get; private set;}
		public Texture2D LevelIconReturnMainMenu		{get; private set;}
		public Font		 AtwriterFont					{get; private set;}
		
		public SelectLevelModel ()
		{
			AtwriterFont					= Resources.Load<Font> 		("Fonts/Atwriter");
			LevelIconBackground 			= Resources.Load<Texture2D> ("UI/LevelIcon/LevelIconBackground");
			LevelIconBackgroundCompleted	= Resources.Load<Texture2D> ("UI/LevelIcon/LevelIconBackgroundCompleted");
			LevelIconLocked					= Resources.Load<Texture2D> ("UI/LevelIcon/LevelIconLocked");
			LevelIconReturnMainMenu			= Resources.Load<Texture2D> ("UI/LevelIcon/LevelIconReturnMainMenu");

			if (AtwriterFont == null) 					Debug.LogError ("Fonts/Atwriter does not exist");
			if (LevelIconBackground == null) 			Debug.LogError ("UI/LevelIcon/LevelIconBackground cannot be found");
			if (LevelIconBackgroundCompleted == null) 	Debug.LogError ("UI/LevelIcon/LevelIconBackgroundCompleted cannot be found");
			if (LevelIconLocked == null) 				Debug.LogError ("UI/LevelIcon/LevelIconLocked cannot be found");
			if (LevelIconReturnMainMenu == null) 		Debug.LogError ("UI/LevelIcon/LevelIconReturnMainMenu");

			//////////////////////////////////////
			/// PlayerPrefs format GetInt, SetInt 
			/// 
			/// key:
			/// 	Level01 
			/// 
			/// value:
			/// 	0 - normal (open)
			///		1 - completed (open)
			/// 	2 - locked (closed)
			/// 
			//////////////////////////////////////


			// Check if playerprefs exist for the levels. If not then populate
			for (int i = 1; i <= Const.MaxLevelNumber; i++)
			{
				if (!PlayerPrefs.HasKey (TBUtil.LevelNumberToKey (i)))
				    
					// First level is open
					if (i == 1)
						PlayerPrefs.SetInt (TBUtil.LevelNumberToKey (i), (int) PlayerPrefsLevelKey.Open);
					
					// Remaining are closed
					else
						PlayerPrefs.SetInt (TBUtil.LevelNumberToKey (i), (int) PlayerPrefsLevelKey.Locked);
			}
		}
	}
}