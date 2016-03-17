using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

namespace TeamBrookvale.Game
{
	public class DrillingCommand : Command
	{
		[Inject]
		public bool startOrStop {get;set;}

		[Inject]
		public IShipDrillableModel shipDrillableModel {get;set;}
		
		[Inject]
		public SingletonSoundFxSignal singletonSoundFxSignal {get;set;}

		public override void Execute ()
		{
			shipDrillableModel.lastDrilledShipProperties.isBeingDrilled = startOrStop;

			singletonSoundFxSignal.Dispatch (
					SoundFx.Drill,
					(startOrStop ? SingletonSoundFxCmd.Play : SingletonSoundFxCmd.Stop));
		}
	}
}