// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU {

	public interface IDanmakuCollider {

		/// <summary>
		/// Raises the danmaku collision event.
		/// </summary>
		/// <param name="danmaku">The danmaku collided with.</param>
		/// <param name="danmaku">The relevant information about collision.</param>
		void OnDanmakuCollision(Danmaku danmaku, RaycastHit2D info);
	
	}
}