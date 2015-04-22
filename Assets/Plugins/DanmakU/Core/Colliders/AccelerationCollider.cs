// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityUtilLib;

namespace DanmakU {

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
		protected override void ProcessDanmaku (Danmaku danmaku, RaycastHit2D info) {
			danmaku.Speed += actual;
		}
		#endregion




	}

}
