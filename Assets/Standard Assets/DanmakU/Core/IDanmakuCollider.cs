// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU {

    /// <summary>
    /// An interface for any behaviour that would like to recieve messages of when danmaku collides
    /// with an attached collider.
    /// </summary>
    public interface IDanmakuCollider {

        /// <summary>
        /// Raises a danmaku collision event.
        /// </summary>
        /// <param name="danmaku">The danmaku collided with.</param>
        /// <param name="info">The relevant information about collision.</param>
        void OnDanmakuCollision(Danmaku danmaku, RaycastHit2D info);

    }

}