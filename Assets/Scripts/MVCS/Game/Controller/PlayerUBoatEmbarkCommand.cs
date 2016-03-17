using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.context.api;

namespace TeamBrookvale.Game
{
	public class PlayerUBoatEmbarkCommand : Command {

		[Inject]
		public bool isEmbarkedUBoat {get;set;}

		[Inject]
		public IInventoryModel inventoryModel {get;set;}

		[Inject]
		public IPlayerModel playerModel {get;set;}

		[Inject]
		public IUBoatModel uboatModel {get;set;}

		[Inject]
		public PlaySoundFxSignal playSoundFxSignal {get;set;}

		[Inject]
		public SingletonSoundFxSignal singletonSoundFxSignal {get;set;}

		public override void Execute ()
		{
			//Debug.Log ("PlayerUBoatEmbarkCommand isEmbarkedUBoat " + isEmbarkedUBoat);

			playerModel.isEmbarkedUBoat = uboatModel.isPlayerEmbarked = isEmbarkedUBoat;

			if (isEmbarkedUBoat)
			{
				// play embark sound
				playSoundFxSignal.Dispatch (SoundFx.SubMarineEnter, TouchScreenPosition.zero);

				// play uboat sound in loop
				singletonSoundFxSignal.Dispatch (SoundFx.UBoat, SingletonSoundFxCmd.Play);
			}
			else
			{
				singletonSoundFxSignal.Dispatch (SoundFx.UBoat, SingletonSoundFxCmd.Stop);
			}
		}
	}
}