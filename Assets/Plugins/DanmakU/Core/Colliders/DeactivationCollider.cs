// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.
using UnityEngine;

namespace DanmakU {

	[AddComponentMenu("DanmakU/Colliders/Deactivation Collider")]
	public class DeactivationCollider : DanmakuCollider {

		#region implemented abstract members of DanmakuCollider
		protected override void ProcessDanmaku (Danmaku danmaku, RaycastHit2D info) {
			danmaku.Deactivate ();
		}
		#endregion
		
	}
}