// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

/// <summary>
/// A set of pre-created Danmaku Colliders that can be used
/// </summary>
namespace DanmakU.Collider {

	/// <summary>
	/// A DanmakuCollider implementation that adds controllers to valid bullets that contact it.
	/// </summary>
	[AddComponentMenu("DanmakU/Colliders/Add Controller Collider")]
	public class AddControllerCollider : DanmakuCollider {
		
		[SerializeField, Show]
		private IDanmakuController[] controllers;
		
		private DanmakuController controllerAggregate;

		private DanmakuGroup affected;

		/// <summary>
		/// Called on Component instantiation
		/// </summary>
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
		/// Adds a Danmaku Controller to the list. 
		/// </summary>
		/// 
		/// <remarks>
		/// The Danmaku Controller will be added to all bullets that contact the collider until it is removed from the list.
		/// If the controller is already on the list, it will still be added. More than one copy of the controller will be applied 
		/// to bullets.
		/// </remarks>
		/// <param name="controller">The controller to be added.</param>
		public void AddController(IDanmakuController controller) {
			controllerAggregate += controller.Update;
		}

		/// <summary>
		/// Removes a controller. 
		/// </summary>
		/// 
		/// <remarks>
		/// The controller is no longer added to bullets that contact this collider.
		/// If the list does not contain the controller, this method does nothing.
		/// If the list contains more than one copy of the controller, this method only removes one copy.
		/// </remarks>
		/// <param name="controller">The controller to be removed.</param>
		public void RemoveController(IDanmakuController controller) {
			controllerAggregate -= controller.Update;
		}
		
		/// <summary>
		/// Adds a Danmaku Controller to the list. 
		/// </summary>
		/// 
		/// <remarks>
		/// The Danmaku Controller will be added to all bullets that contact the collider until it is removed from the list.
		/// If the controller is already on the list, it will still be added. More than one copy of the controller will be applied 
		/// to bullets.
		/// </remarks>
		/// <param name="controller">The controller(s) to be added.</param>
		public void AddController(DanmakuController controller) {
			controllerAggregate += controller;
		}
		
		/// <summary>
		/// Removes a controller. 
		/// </summary>
		/// 
		/// <remarks>
		/// The controller is no longer added to bullets that contact this collider.
		/// If the list does not contain the controller, this method does nothing.
		/// If the list contains more than one copy of the controller, this method only removes one copy.
		/// If the supplied controller is multicast and contains multiple controllers, all of the contained controllers will be removed.
		/// </remarks>
		/// <param name="controller">Controller.</param>
		public void RemoveController(DanmakuController controller) {
			controllerAggregate -= controller;
		}

		/// <summary>
		/// Clears the controllers.
		/// All of the currently included contrrollers are removed.
		/// </summary>
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
