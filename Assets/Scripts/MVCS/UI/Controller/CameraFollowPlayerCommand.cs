using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.context.api;

namespace TeamBrookvale.UI
{
	public class CameraFollowPlayerCommand : Command
	{
		[Inject (CrossContextElements.GAME_CAMERA)]
		public Camera cam {get;set;}

		[Inject]
		public TeamBrookvale.Game.ICameraModel cameraModel {get;set;}

		public override void Execute ()
		{
			cameraModel.setIsFollowingPlayer (true);
		}
	}
}