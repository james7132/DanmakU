// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU {
	
	public class AddControllerCollider : DanmakuCollider {
		
		[Serialize]
		private IDanmakuController[] controllers;
		
		private DanmakuController controllerAggregate;

		private DanmakuGroup affected;

		public override void Awake () {
			base.Awake ();
			affected = new DanmakuGroup ();
			if (controllers == null)
				return;
			for (int i = 0; i < controllers.Length; i++) {
				controllerAggregate += controllers[i].UpdateDanmaku;
			}
		}

		public void AddController(IDanmakuController controller) {
			controllerAggregate += controller.UpdateDanmaku;
		}
		
		public void RemoveController(IDanmakuController controller) {
			controllerAggregate -= controller.UpdateDanmaku;
		}
		
		public void ClearControllers() {
			controllerAggregate = null;
		}

		#region implemented abstract members of DanmakuCollider

		protected override void DanmakuCollision (Danmaku danmaku, RaycastHit2D info) {
			if(affected.Contains(danmaku))
			   return;

			danmaku.AddController (controllerAggregate);

			affected.Add (danmaku);
		}

		#endregion

	}

}
