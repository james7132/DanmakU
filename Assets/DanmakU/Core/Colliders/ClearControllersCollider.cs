// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU.Collider {

	[AddComponentMenu("DanmakU/Colliders/Clear Controllers Collider")]
	public class ClearControllersCollider : DanmakuCollider {
		
		private DanmakuGroup affected;

		public override void Awake () {
			base.Awake ();
			affected = new DanmakuGroup ();
		}

		#region implemented abstract members of DanmakuCollider
		
		protected override void DanmakuCollision (Danmaku danmaku, RaycastHit2D info) {
			if(affected.Contains(danmaku))
				return;

			danmaku.ClearControllers ();

			affected.Add (danmaku);
		}
		
		#endregion
	}

}
