// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

/// <summary>
/// A set of pre-created Danmaku Colliders that can be used
/// </summary>
namespace DanmakU.Collider {

	/// <summary>
	/// A DanmakuCollider implementation that removes all DanmakuControllers from the bullets that come into contact with it.
	/// </summary>
	[AddComponentMenu("DanmakU/Colliders/Clear Controllers Collider")]
	public class ClearControllersCollider : DanmakuCollider {
		
		private DanmakuGroup affected;

		/// <summary>
		/// Called on Component instantiation
		/// </summary>
		public override void Awake () {
			base.Awake ();
			affected = new DanmakuSet ();
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

			danmaku.ClearControllers ();

			affected.Add (danmaku);
		}
		
		#endregion
	}

}
