// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
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
		/// <param name="danmaku">The relevant information about collision.</param>
		void OnDanmakuCollision(Danmaku danmaku, RaycastHit2D info);
	
	}
}