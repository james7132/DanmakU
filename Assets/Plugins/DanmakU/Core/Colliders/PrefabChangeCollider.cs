// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Collider {

    [AddComponentMenu("DanmakU/Colliders/Prefab Change Collider")]
    public class PrefabChangeCollider : DanmakuCollider {

        private DanmakuGroup affected;

        //TODO Document

        [SerializeField, Show]
        private DanmakuPrefab prefab;

        public DanmakuPrefab Prefab {
            get { return prefab; }
            set { prefab = value; }
        }

        protected override void Awake() {
            base.Awake();
            affected = new DanmakuSet();
        }

        #region implemented abstract members of DanmakuCollider

        /// <summary>
        /// Handles a Danmaku collision. Only ever called with Danmaku that pass the filter.
        /// </summary>
        /// <param name="danmaku">the danmaku that hit the collider.</param>
        /// <param name="info">additional information about the collision</param>
        protected override void DanmakuCollision(Danmaku danmaku,
                                                 RaycastHit2D info) {
            if (affected.Contains(danmaku))
                return;

            if (prefab != null)
                danmaku.MatchPrefab(prefab);

            affected.Add(danmaku);
        }

        #endregion
    }

}