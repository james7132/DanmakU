// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityUtilLib;

namespace DanmakU {

	
	public class ConstantForceCollider : DanmakuCollider {
		
		[SerializeField]
		private Vector2 force;

		private Vector2 actual;
		
		public Vector2 Force {
			get {
				return force;
			}
			set {
				force = value;
			}
		}

		private void Update() {
			actual = force * Util.DeltaTime;
		}

		#region implemented abstract members of DanmakuCollider
		protected override void DanmakuCollision (Danmaku danmaku, RaycastHit2D info) {
			danmaku.Position += actual;
		}
		#endregion
	}

}
