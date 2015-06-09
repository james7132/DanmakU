// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU {

    /// <summary>
    /// A GameController implementation for 2D Danmaku games.
    /// </summary>
    [AddComponentMenu("DanmakU/Danmaku Game Controller")]
    public sealed class DanmakuGameController : MonoBehaviour {

        private static DanmakuGameController instance;

        [SerializeField]
        private float angleResolution = 0.1f;

        [SerializeField]
        private int danmakuInitialCount = Danmaku.standardStart;

        [SerializeField]
        private int danmakuSpawnOnEmpty = Danmaku.standardSpawn;

        public bool FrameRateIndependent = true;

        public static DanmakuGameController Instance {
            get {
                if (instance != null)
                    return instance;
                instance = FindObjectOfType<DanmakuGameController>() ??
                           new GameObject("Danmaku Game Controller")
                               .AddComponent<DanmakuGameController>();
                return instance;
            }
        }

        private void Awake() {
            DontDestroyOnLoad(gameObject);
            if (instance != null) {
                Destroy(this);
                return;
            }
            instance = this;
            Danmaku.Setup(danmakuInitialCount,
                          danmakuSpawnOnEmpty,
                          angleResolution);
        }

        private void Update() {
            Danmaku.UpdateAll();
        }

        private void OnLevelWasLoaded(int level) {
            Danmaku.DeactivateAll();
        }

    }

}