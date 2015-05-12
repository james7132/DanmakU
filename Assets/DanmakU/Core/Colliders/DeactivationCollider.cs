// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.
using UnityEngine;

namespace DanmakU.Collider {

	/// <summary>
	/// A DanmakuCollider that deactivates all valid danmaku that come in contact with it.
	/// </summary>
	[AddComponentMenu("DanmakU/Colliders/Deactivation Collider")]
	public class DeactivationCollider : DanmakuCollider {

		#region implemented abstract members of DanmakuCollider

		protected override void DanmakuCollision (Danmaku danmaku, RaycastHit2D info) {
			danmaku.Deactivate ();
		}

		#endregion
		
	}
}