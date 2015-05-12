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
	/// A Danmaku Controller that speeds up or slows down danmaku so long as bullets are contacting it.
	/// </summary>
	[AddComponentMenu("DanmakU/Colliders/Acceleration Collider")]
	public class AccelerationCollider : DanmakuCollider {

		[SerializeField, Show]
		private float acceleration;
		public float Acceleration {
			get {
				return acceleration;
			}
			set {
				acceleration = value;
			}
		}

		private float actual;

		private void Update () {
			actual = acceleration * TimeUtil.DeltaTime;
		}

		#region implemented abstract members of DanmakuCollider

		protected override void DanmakuCollision (Danmaku danmaku, RaycastHit2D info) {
			danmaku.Speed += actual;
		}

		#endregion

	}

}
