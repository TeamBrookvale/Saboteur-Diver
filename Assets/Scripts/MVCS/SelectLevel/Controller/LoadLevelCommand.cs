using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

namespace TeamBrookvale.SelectLevel
{
	public class LoadLevelCommand : Command
	{
		// Signals parameter
		[Inject]
		public int levelToLoad { get; set; }

		public override void Execute ()
		{
			PlayerPrefs.SetInt ("LevelToLoad", levelToLoad);

			if (levelToLoad == 1)
				Application.LoadLevel ("IntroScene01");
			else
				Application.LoadLevel ("GameScene");
		}
	}
}