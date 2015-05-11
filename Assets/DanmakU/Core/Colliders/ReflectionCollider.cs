// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU {
	
	public class ReflectionCollider : DanmakuCollider {
		
		private DanmakuGroup affected;

		public override void Awake () {
			base.Awake ();
			affected = new DanmakuGroup ();
		}

		#region implemented abstract members of DanmakuCollider
		protected override void DanmakuCollision (Danmaku danmaku, RaycastHit2D info) {
			if (affected.Contains (danmaku))
				return;
			Vector2 normal = info.normal;
			Vector2 direction = danmaku.direction;
			danmaku.Direction = direction - 2 * Vector2.Dot (normal, direction) * normal;
			danmaku.position = info.point;
			affected.Add (danmaku);
		}
		#endregion

	}

}
