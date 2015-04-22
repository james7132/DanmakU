// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;

/// <summary>
/// A utilty library of random useful and portable scripts for Unity
/// </summary>
namespace UnityUtilLib {

	/// <summary>
	/// A static Singleton music manager for playing global background music.
	/// Best used with the Scene's AudioListener attached on the same object.
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class AudioManager : Singleton<AudioManager> {

		public AudioSource musicSource;
		public AudioSource sfxSource;

		/// <summary>
		/// Gets the currently playing BGM.
		/// </summary>
		/// <value>The currently playing.</value>
		public static AudioClip CurrentlyPlaying  {
			get {
				return Instance.musicSource.clip;
			}
		}

		public static void PlaySFX(AudioClip audioClip, float volume = 1) {
			Instance.musicSource.PlayOneShot (audioClip, volume);
		}

		/// <summary>
		/// Plays the given music clip at the given volume.
		/// </summary>
		/// <param name="musicClip">the music clip to play</param>
		/// <param name="volume">the volume to play it at. If set to a negative number, the current volume will be used.</param>
		public static void PlayMusic(AudioClip musicClip, float volume = -1) {
			Instance.musicSource.Stop ();
			Instance.musicSource.clip = musicClip;
			SetVolume (volume);
			Instance.musicSource.Play ();
		}

		/// <summary>
		/// Pauses this the current music clip.
		/// </summary>
		public static void Pause() {
			Instance.musicSource.Pause ();
		}

		/// <summary>
		/// Stops the current music clip.
		/// </summary>
		public static void Stop() {
			Instance.musicSource.Pause ();
		}

		/// <summary>
		/// Sets the volume.
		/// If the given value is not between 0.0 and 1.0, no volume change will occur.
		/// </summary>
		/// <param name="volume">the new volume.</param>
		public static void SetVolume(float volume) {
			if(volume >= 0f && volume <= 1f)
				Instance.musicSource.volume = volume;
		}

		public override void Awake () {
			base.Awake ();
			musicSource = GetComponent<AudioSource>();
		}
	}
}