using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using TeamBrookvale;

namespace TeamBrookvale.Game
{
	public class PanicModeCommand : Command {

		[Inject]
		public SingletonSoundFxSignal singletonSoundFxSignal {get;set;}

		[Inject]
		public ISpotlightDictionary spotlightDictionary {get;set;}

		[Inject]
		public StatusBarMessageSignal statusBarMessageSignal {get;set;}

		[Inject]
		public PanicModeStartedOrEndedSignal panicModeStartedOrEndedSignal {get;set;}

		[Inject]
		public IGameModel gameModel {get;set;}

		// IRoutineRunner allows use to run Coroutines without having to directly implement the MonoBehavior interface.
		[Inject]
		public IRoutineRunner routineRunner {get;set;}

		public override void Execute ()
		{
			routineRunner.StartCoroutine (DelayedCommands());
		}

		IEnumerator DelayedCommands ()
		{
			// measure the time of the panic mode
			float thisCommandStartedAtTime = Time.time;

			// count the number of current panic mode commands as only the last one ceases the panic mode
			gameModel.numberOfCurrentPanicModeCommand++;

			// panic mode sound starts delayed but only one at a time
			if (gameModel.numberOfCurrentPanicModeCommand == 1)
			{
				// dispatch a status bar message signal
				statusBarMessageSignal.Dispatch (Const.StatusBarPanicMode);

				// count number of panic modes for game statistics
				gameModel.numberOfSpotPanicModeCommands++;
				//Debug.Log ("numberOfSpotPanicModeCommands " + gameModel.numberOfSpotPanicModeCommands);

				yield return new WaitForSeconds (Random.Range(1f,3f));
				singletonSoundFxSignal.Dispatch (SoundFx.PanicMode, SingletonSoundFxCmd.Play);
				panicModeStartedOrEndedSignal.Dispatch (true);
			}

			// spotlight cones extend delayed
			yield return new WaitForSeconds (Random.Range(1f,3f));
			foreach(ISpotlightModel s in spotlightDictionary.spotlights.Values)
			{
				// Double the field of viewrange
				s.targetFieldOfViewRange = 2 * s.originalFieldOfViewRange;

				// Speed up seeking
				s.angularVelocity *= 1.5f;

				// wait under 1 sec for the next spotlight
				yield return new WaitForSeconds (Random.Range(.5f,2f));
			}

			// count the number of current panic mode commands as only the last one ceases the panic mode
			gameModel.numberOfCurrentPanicModeCommand--;

			// wait up to panic mode length
			yield return new WaitForSeconds (Const.PanicModeLenght - (Time.time - thisCommandStartedAtTime));

			// cease panic mode if this is the last panic command
			if (gameModel.numberOfCurrentPanicModeCommand == 0)
			{
				singletonSoundFxSignal.Dispatch (SoundFx.PanicMode, SingletonSoundFxCmd.FadeOut);

				foreach (ISpotlightModel s in spotlightDictionary.spotlights.Values)
				{
					s.targetFieldOfViewRange = s.originalFieldOfViewRange;
					s.angularVelocity = s.originalAngularVelocity;
				}

				panicModeStartedOrEndedSignal.Dispatch (false);
			}
		}
	}
}