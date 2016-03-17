using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using strange.extensions.pool.api;

namespace TeamBrookvale.Game
{
	public enum SoundFx { PanicMode, DangerZone, Drill, DrillSuccess, LevelPassed, SmokeBombSplash, SmokeBombHissing, SubMarineEnter, UBoat, BombMounting }

	public class PlaySoundFxCommand : Command {

		// Keep reference to each possible audio clip, to avoid frequent calls to Resources.Load.
		private static Dictionary<SoundFx, AudioClip> _soundFxs;

		// This is one of our event parameters, which tells the command which sound effect to play.
		[Inject]
		public SoundFx soundFxToPlay { get; set; }

		// This is our second event parameter, which tells where the audio source should be positioned in the scene.
		[Inject]
		public TouchScreenPosition touchScreenPosition { get; set; }

		// IRoutineRunner allows use to run Coroutines without having to directly implement the MonoBehavior interface.
		//[Inject(RoutineRunnerTypes.SoundFxAudioSource)]
		[Inject]
		public IRoutineRunner routineRunner { get; set; }

		// An object pool of audio source objects used in the scene. 
		[Inject(GameElement.SOUND_FX_AUDIO_SOURCE)]
		public IPool<GameObject> soundFxAudioSourcePool { get; set; }
		
		public override void Execute()
		{
			if (_soundFxs == null)
				_soundFxs = new Dictionary<SoundFx, AudioClip>();

			GameObject audioSourceGo = soundFxAudioSourcePool.GetInstance();

			if (audioSourceGo)
			{
				// Lazily load the audio clip and store reference to it in our
				// sound fxs dictionary.
				if (!_soundFxs.ContainsKey(soundFxToPlay))
					LoadAudioClip();

				audioSourceGo.SetActive(true);
				audioSourceGo.audio.clip = _soundFxs[soundFxToPlay];
				audioSourceGo.transform.position = touchScreenPosition.world;
				audioSourceGo.audio.Play();
				routineRunner.StartCoroutine(Update(audioSourceGo));
				Retain();
			}
		}
		
		private IEnumerator Update(GameObject audioSourceGo)
		{
			// Don't return the audio source to the pool until it's stopped playing.
			// The obvious danger here is that we'll create an infinite loop if
			// the audio source is set to loop the audio clip.
			// This may, or may not, be a desirable effect, depending
			// on the sound effect being played (music? ambient?)
			while (audioSourceGo.audio.isPlaying)
				yield return null;

			audioSourceGo.SetActive(false);
			soundFxAudioSourcePool.ReturnInstance(audioSourceGo);
			Release();
		}
		
		private void LoadAudioClip()
		{

			// if SoundToPlay enum value is SoundFx.Alarm then LoadAudioClipResource("Fx/Alarm", SoundFx.Alarm);
			LoadAudioClipResource("Fx/" + soundFxToPlay.ToString(), soundFxToPlay);
		}
		
		private void LoadAudioClipResource(string path, SoundFx soundFx)
		{
			AudioClip audioClip = Resources.Load(path) as AudioClip;
			_soundFxs.Add(soundFx, audioClip);
		}
	}
}