using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace TeamBrookvale.Game
{
	public class CameraMediator : Mediator
	{
		[Inject]
		public CameraView view {get;set;}

		[Inject]
		public ICameraModel cameraModel {get;set;}

		[Inject]
		public IPlayerModel playerModel {get;set;} // player is followed by default

		[Inject]
		public ILevelBoundsModel levelBounds {get;set;}

		public override void OnRegister ()
		{
			// assign camera to model
			if (cameraModel.camera == null)
				cameraModel.camera = view.GetComponent<Camera>();
			else
				Debug.LogError ("More than one camera in the scene");

			view.init (cameraModel, playerModel.getCachedPosition, levelBounds);
		}

		public override void OnRemove ()
		{
		}
	}
}