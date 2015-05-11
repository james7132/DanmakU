// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityUtilLib;

/// <summary>
/// A set of pre-created Danmaku Colliders that can be used
/// </summary>
namespace DanmakU.Collider {

	/// <summary>
	/// A Danmaku Controller that speeds up or slows down danmaku so long as bullets are contacting it.
	/// </summary>
	[AddComponentMenu("DanmakU/Colliders/Acceleration Collider")]
	public class AccelerationCollider : DanmakuCollider {

		[SerializeField]
		private float acceleration;
	
		private float actual;

		public float Acceleration {
			get {
				return acceleration;
			}
			set {
				acceleration = value;
			}
		}

		private void Update () {
			actual = acceleration * Util.DeltaTime;
		}

		#region implemented abstract members of DanmakuCollider

		protected override void DanmakuCollision (Danmaku danmaku, RaycastHit2D info) {
			danmaku.Speed += actual;
		}

		#endregion

	}

}
