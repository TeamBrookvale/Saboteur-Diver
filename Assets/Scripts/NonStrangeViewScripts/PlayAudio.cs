using UnityEngine;
using System.Collections;

public class PlayAudio : MonoBehaviour {

	public bool isPlaying;
	AudioSource audioSource;

	void Start ()
	{
		audioSource = GetComponent<AudioSource> ();

		if (audioSource == null)
			Debug.LogError ("No audio source attached to " + gameObject.name);
	}
	
	void Update ()
	{
		if (isPlaying && !audioSource.enabled)
		{
			audioSource.enabled = true;
			audio.Play ();
		}

		if (!isPlaying && audioSource.enabled)
		{
			audio.Stop ();
			audioSource.enabled = false;
		}
	}
}
