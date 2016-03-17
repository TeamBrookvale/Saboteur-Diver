using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;

namespace TeamBrookvale.Game
{
	public enum SingletonSoundFxCmd { Play, Stop, FadeOut }

	public class SingletonSoundFxCommand : Command {

		// Keep reference to each possible audio clip, to avoid frequent calls to Resources.Load.
		private static Dictionary<SoundFx,GameObject> _singletonSoundFxs = new Dictionary<SoundFx,GameObject>();

		// This is one of our event parameters, which tells the command which sound effect to play.
		[Inject]
		public SoundFx soundFx { get; set; }
		
		// This is our second event parameter, which tells where the audio source whet to do
		[Inject]
		public SingletonSoundFxCmd cmd {get;set;}

		// IRoutineRunner allows use to run Coroutines without having to directly implement the MonoBehavior interface.
		[Inject]
		public IRoutineRunner routineRunner { get; set; }

		// Have a parent GameObject for more structure in the scene view
		static GameObject _parent;

		// if currently fading out but play event received then play
		static bool isFadingOut;

		public override void Execute ()
		{
			if (_parent == null)
			{
				_parent = new GameObject("SingletonSoundFxs");
				_singletonSoundFxs.Clear ();
			}

			// Load audio game object if not exists in the directory
			if (!_singletonSoundFxs.ContainsKey(soundFx))
			{
				// if SoundToPlay enum value is SoundFx.Alarm then LoadAudioClipResource("Fx/Alarm", SoundFx.Alarm);
				string path = "Fx/" + soundFx.ToString();
				AudioClip audClip = Resources.Load(path) as AudioClip;

				// Raise exception in console if could not load the AudioClip
				if (audClip == null)
					Debug.LogError("Resources/Fx/" + soundFx.ToString() + " could not be loaded");

				GameObject sFxGO = new GameObject(soundFx.ToString());
				sFxGO.transform.parent = _parent.transform;

				AudioSource audsrc = sFxGO.AddComponent<AudioSource>();
				audsrc.clip = audClip;
				audsrc.loop = true;
				_singletonSoundFxs.Add(soundFx, sFxGO);
			}

			if (cmd == SingletonSoundFxCmd.Play) play();
			if (cmd == SingletonSoundFxCmd.Stop) stop();
			if (cmd == SingletonSoundFxCmd.FadeOut) fadeOut();
		}

		private void play()
		{
			// if currently fading out but play event received then play
			isFadingOut = false;

			_singletonSoundFxs[soundFx].GetComponent<AudioSource>().Play();
		}

		private void stop()
		{
			_singletonSoundFxs[soundFx].GetComponent<AudioSource>().Stop();
		}

		private void fadeOut()
		{
			isFadingOut = true;

			routineRunner.StartCoroutine(
				UpdateFadeOut(
					_singletonSoundFxs[soundFx].GetComponent<AudioSource>()
				)
			);
		}

		private IEnumerator UpdateFadeOut(AudioSource audsrc)
		{
			// if currently fading out but play event received then play
			if (!isFadingOut)
			{	
				audsrc.volume = 1;
				yield break;
			}

			// Don't return the until it's completely faded out
			while (0 < audsrc.volume)
			{
				// progressively decrease volume
				audsrc.volume -= Time.deltaTime;

				// do not return completely yet
				yield return null;
			}

			// if volume is zero than do not fade out anymore and initialize
			audsrc.Stop ();
			audsrc.volume = 1;
		}
	}
}