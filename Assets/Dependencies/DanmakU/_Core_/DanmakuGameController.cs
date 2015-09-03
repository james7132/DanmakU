// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Hourai;

namespace Hourai.DanmakU {

    /// <summary>
    /// A Game implementation for 2D Danmaku games.
    /// </summary>
    [AddComponentMenu("Hourai.DanmakU/Danmaku Game")]
    public sealed class DanmakuGame : Game {

        [SerializeField]
        private float angleResolution = 0.1f;

        [SerializeField]
        private int danmakuInitialCount = Danmaku.standardStart;

        [SerializeField]
        private int danmakuSpawnOnEmpty = Danmaku.standardSpawn;

        public bool FrameRateIndependent = true;

        protected override void Awake() {
            base.Awake();
            Danmaku.Setup(danmakuInitialCount,
                          danmakuSpawnOnEmpty,
                          angleResolution);
        }

        private void OnLevelWasLoaded(int level) {
            Danmaku.DeactivateAll();
        }

    }

}