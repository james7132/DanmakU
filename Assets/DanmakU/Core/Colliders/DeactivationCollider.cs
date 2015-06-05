// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.
using UnityEngine;

/// <summary>
/// A set of pre-created Danmaku Colliders that can be used
/// </summary>
namespace DanmakU.Collider {

	/// <summary>
	/// A DanmakuCollider that deactivates all valid danmaku that come in contact with it.
	/// </summary>
	[AddComponentMenu("DanmakU/Colliders/Deactivation Collider")]
	public class DeactivationCollider : DanmakuCollider {

		#region implemented abstract members of DanmakuCollider

		/// <summary>
		/// Handles a Danmaku collision. Only ever called with Danmaku that pass the filter.
		/// </summary>
		/// <param name="danmaku">the danmaku that hit the collider.</param>
		/// <param name="info">additional information about the collision</param>
		protected override void DanmakuCollision (Danmaku danmaku, RaycastHit2D info) {
			danmaku.Deactivate ();
		}

		#endregion
		
	}
}