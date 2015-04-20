// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU {

	public interface IDanmakuCollider {

		/// <summary>
		/// Raises the danmaku collision event.
		/// </summary>
		/// <param name="danmaku">Danmaku.</param>
		void OnDanmakuCollision(Danmaku danmaku);
	
	}
}