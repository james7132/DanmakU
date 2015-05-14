// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

/// <summary>
/// A set of pre-created Danmaku Colliders that can be used
/// </summary>
namespace DanmakU.Collider {
	
	[AddComponentMenu("DanmakU/Colliders/Add Controller Collider")]
	public class AddControllerCollider : DanmakuCollider {
		
		[SerializeField, Show]
		private IDanmakuController[] controllers;
		
		private DanmakuController controllerAggregate;

		private DanmakuGroup affected;

		public override void Awake () {
			base.Awake ();
			affected = new DanmakuSet ();
			if (controllers == null)
				return;
			for (int i = 0; i < controllers.Length; i++) {
				controllerAggregate += controllers[i].Update;
			}
		}

		/// <summary>
		/// Adds a Danmaku Controller to the list. This Danmaku Controller will be added to all bullets that touch
		/// the collider until it is removed from the list.
		/// </summary>
		/// <param name="controller">The IDanmakuController implementation of a danmaku controller.</param>
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

			danmaku.ControllerUpdate += controllerAggregate;

			affected.Add (danmaku);
		}

		#endregion

	}

}
