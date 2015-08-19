using UnityEngine;
using UnityEngine.Audio;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Hourai {

    public sealed class BGM : Singleton<BGM> {

        private static AudioSource bgmSource;

        [Serialize, Show]
        private AudioMixerGroup mixerGroup;

        public static AudioClip CurrentlyPlaying {
            get { return bgmSource.clip; }
        }

        public static void Play(AudioClip bgm) {
            bgmSource.Stop();
            bgmSource.clip = bgm;
            bgmSource.Play();
        }

        public static void Stop() {
            bgmSource.Stop();
        }

        protected override void Awake() {
            base.Awake();
            bgmSource = gameObject.GetOrAddComponent<AudioSource>();
            bgmSource.outputAudioMixerGroup = mixerGroup;
            bgmSource.hideFlags = HideFlags.HideInInspector;
            bgmSource.volume = 1f;
            bgmSource.loop = true;
            bgmSource.spatialBlend = 0f;
        }

        private void OnDestroy() {
            Destroy(bgmSource);
        }

    }

}