// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Collider {
	
	[AddComponentMenu("DanmakU/Colliders/Add Controller Collider")]
	public class AddControllerCollider : DanmakuCollider {
		
		[SerializeField, Show]
		private IDanmakuController[] controllers;
		
		private DanmakuController controllerAggregate;

		private DanmakuGroup affected;

		public override void Awake () {
			base.Awake ();
			affected = new DanmakuGroup ();
			if (controllers == null)
				return;
			for (int i = 0; i < controllers.Length; i++) {
				controllerAggregate += controllers[i].Update;
			}
		}

		public void AddController(IDanmakuController controller) {
			controllerAggregate += controller.Update;
		}
		
		public void RemoveController(IDanmakuController controller) {
			controllerAggregate -= controller.Update;
		}

		public void AddController(DanmakuController controller) {
			controllerAggregate += controller;
		}

		public void RemoveController(DanmakuController controller) {
			controllerAggregate -= controller;
		}
		
		public void ClearControllers() {
			controllerAggregate = null;
		}

		#region implemented abstract members of DanmakuCollider

		/// <summary>
		/// Handles a Danmaku collision. Only ever called with Danmaku that pass the filter.
		/// </summary>
		/// <param name="danmaku">the danmaku that hit the collider.</param>
		/// <param name="info">additional information about the collision</param>
		protected override void DanmakuCollision (Danmaku danmaku, RaycastHit2D info) {
			if(affected.Contains(danmaku))
			   return;

			danmaku.AddController (controllerAggregate);

			affected.Add (danmaku);
		}

		#endregion

	}

}
