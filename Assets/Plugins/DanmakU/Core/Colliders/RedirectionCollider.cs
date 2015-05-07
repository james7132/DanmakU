// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityUtilLib;
using Vexe.Runtime.Types;

namespace DanmakU {

	/// <summary>
	/// A DanmakuCollider that changes the direction of motion for all valid bullets that come into contact with it.
	/// </summary>
	[AddComponentMenu("DanmakU/Colliders/Redirection Collider")]
	public class RedirectionCollider : DanmakuCollider {

		[SerializeField]
		private RotationMode rotationMode;

		[SerializeField]
		private DynamicFloat angle;

		private DanmakuGroup affected;

		public Transform Target {
			get;
			set;
		}

		/// <summary>
		/// Called on Component instantiation
		/// </summary>
		public override void Awake () {
			base.Awake ();
			affected = new DanmakuGroup ();
		}

		#region implemented abstract members of DanmakuCollider
		protected override void DanmakuCollision (Danmaku danmaku, RaycastHit2D info) {
			if (affected.Contains (danmaku))
				return;
			float baseAngle = angle.Value;
			switch(rotationMode) {
			case RotationMode.Relative:
				baseAngle += danmaku.Rotation;
				break;
			case RotationMode.Object:
				baseAngle += DanmakuUtil.AngleBetween2D (danmaku.Position, Target.position);
				break;
			case RotationMode.Absolute:
				break;
			}
			danmaku.Rotation = baseAngle;
			affected.Add (danmaku);
		}
		#endregion

	}

}
