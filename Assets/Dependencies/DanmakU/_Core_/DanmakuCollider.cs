// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections.Generic;
using UnityEngine;
using Vexe.Runtime.Types;

namespace Hourai.DanmakU {

    /// <summary>
    /// An interface for any behaviour that would like to recieve messages of when danmaku collides
    /// with an attached collider.
    /// </summary>
    public interface IDanmakuCollider
    {

        /// <summary>
        /// Raises a danmaku collision event.
        /// </summary>
        /// <param name="danmaku">The danmaku collided with.</param>
        /// <param name="info">The relevant information about collision.</param>
        void OnDanmakuCollision(Danmaku danmaku, RaycastHit2D info);

    }

    [RequireComponent(typeof (Collider2D))]
    public abstract class DanmakuCollider : DanmakuBehaviour, IDanmakuCollider {

        /// <summary>
        /// A filter for a set of tags, delimited by "|" for selecting which bullets to affect
        /// Leaving this blank will affect all bullets
        /// </summary>
        [Serialize, PerItem, Tags]
        private string[] validTags;

        private HashSet<string> tags; 

        #region IDanmakuCollider implementation

        /// <summary>
        /// Called on collision with any Danmaku
        /// </summary>
        /// <param name="proj">Proj.</param>
        public void OnDanmakuCollision(Danmaku danmaku, RaycastHit2D info) {
            if (tags == null ||tags.Contains(danmaku.Tag))
                DanmakuCollision(danmaku, info);
        }

        #endregion

        /// <summary>
        /// Called on Component instantiation
        /// </summary>
        protected virtual void Awake() {
            if(validTags != null && validTags.Length > 0)
                tags = new HashSet<string>(validTags);
        }

        /// <summary>
        /// Handles a Danmaku collision. Only ever called with Danmaku that pass the filter.
        /// </summary>
        /// <param name="danmaku">the danmaku that hit the collider.</param>
        /// <param name="info"> additional information about the collision</param>
        protected abstract void DanmakuCollision(Danmaku danmaku,
                                                 RaycastHit2D info);

    }

}