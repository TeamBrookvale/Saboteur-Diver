using UnityEngine;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class InGameMusicView : View
	{
		static AudioClip[] audioClips;

		internal void init (bool isInGameMusicEnabledOnThisLevel)
		{
			if (audioClips == null)
				audioClips = Resources.LoadAll<AudioClip> ("InGameMusic");

			if (audioClips == null || audioClips.Length == 0)
				Debug.LogError ("Could not load audioclips in the Resources/InGameMusic folder");

			if (PlayerPrefs.GetInt (Const.PlayerPrefsMusicOn) == 1 && isInGameMusicEnabledOnThisLevel)
			{
				GetComponent<AudioSource>().clip = audioClips [Random.Range (0, audioClips.Length)];
				GetComponent<AudioSource>().Play ();
			}
			else
			{
				GetComponent<AudioSource>().Stop ();
			}
		}
	}
}