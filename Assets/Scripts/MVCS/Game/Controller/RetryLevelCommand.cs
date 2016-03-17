using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

namespace TeamBrookvale.Game
{
	public class RetryLevelCommand : Command {

		[Inject]
		public IGameModel gameModel {get;set;}

		public override void Execute ()
		{
			Application.LoadLevel ("GameScene");
		}
	}
}