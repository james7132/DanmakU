// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

/// <summary>
/// A set of pre-created Danmaku Colliders that can be used
/// </summary>
namespace DanmakU.Collider {

	/// <summary>
	/// A DanmakuCollider implementation that uses vector reflection to redirect bullets.
	/// </summary>
	/// 
	/// <remarks>
	/// The resultant direction that valid bullets face after coming into contact with this collider is based
	/// on the vector reflection calculated from the incoming direction of the bullet and the normal to the collider at the point of collision.
	/// </remarks>
	[AddComponentMenu("DanmakU/Colliders/Reflection Collider")]
	public class ReflectionCollider : DanmakuCollider {
		
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
