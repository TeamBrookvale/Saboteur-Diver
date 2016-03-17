using UnityEngine;

namespace TeamBrookvale.SelectLevel
{
	public interface ISelectLevelModel
	{
		Font	  AtwriterFont					{get;}
		Texture2D LevelIconBackground			{get;}
		Texture2D LevelIconBackgroundCompleted	{get;}
		Texture2D LevelIconLocked				{get;}
		Texture2D LevelIconReturnMainMenu		{get;}
	}
}