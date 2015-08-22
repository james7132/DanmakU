using UnityEngine;
using Vexe.Runtime.Types;

namespace Hourai {

    public class BGMGroup : ScriptableObject {

        [SerializeField, Tooltip("The name of the BGM group")]
        private string _name;

        [SerializeField]
        private BGMData[] backgroundMusicData;

        private WeightedRNG<Resource<AudioClip>> selection;

        public string Name {
            get { return _name; }
        }

        private void OnEnable() {
            selection = new WeightedRNG<Resource<AudioClip>>();
            foreach (BGMData bgmData in backgroundMusicData) {
                bgmData.Initialize(Name);
                selection[bgmData.BGM] = bgmData.Weight;
            }
        }

        public void PlayRandom() {
            BGM.Play(selection.Select().Load());
            Resources.UnloadUnusedAssets();
        }

        [System.Serializable]
        private class BGMData {

            private const string delimiter = "/";
            private const string suffix = "weight";

            [SerializeField, Range(0f, 1f)]
            private float _baseWeight = 1f;

            [SerializeField, ResourcePath]
            private string _bgm;

            private Resource<AudioClip> _bgmResource;
            private float _weight;
            private string playerPrefsKey;

            public Resource<AudioClip> BGM {
                get { return _bgmResource; }
            }

            public float Weight {
                get { return _weight; }
            }

            public void Initialize(string stageName) {
                _bgmResource = new Resource<AudioClip>(_bgm);
                playerPrefsKey = stageName + delimiter + _bgm + "_" + suffix;

                if (PlayerPrefs.HasKey(playerPrefsKey))
                    _weight = PlayerPrefs.GetFloat(playerPrefsKey);
                else {
                    PlayerPrefs.SetFloat(playerPrefsKey, _baseWeight);
                    _weight = _baseWeight;
                }
            }

            public override string ToString() {
                return _bgm + " - (" + _baseWeight + ")";
            }

        }

    }

}