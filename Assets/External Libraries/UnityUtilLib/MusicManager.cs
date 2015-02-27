using UnityEngine;
using System.Collections;

namespace UnityUtilLib {

	[RequireComponent(typeof(AudioSource))]
	public class MusicManager : SingletonBehavior<MusicManager> {

		private AudioSource audioSource;

		public static AudioClip CurrentlyPlaying  {
			get {
				return Instance.audioSource.clip;
			}
		}

		public static void PlayMusic(AudioClip musicClip, float volume = -1) {
			Instance.audioSource.Stop ();
			Instance.audioSource.clip = musicClip;
			SetVolume (volume);
			Instance.audioSource.Play ();
		}

		public static void Pause() {
			Instance.audioSource.Pause ();
		}

		public static void Stop() {
			Instance.audioSource.Pause ();
		}

		public static void SetVolume(float volume) {
			if(volume >= 0f && volume <= 1f)
				Instance.audioSource.volume = volume;
		}

		public override void Awake () {
			base.Awake ();
			audioSource = audio;
		}

	}
}