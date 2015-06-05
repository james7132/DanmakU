// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Controllers {

	[System.Serializable]
	public class DelayedAngleChange : IDanmakuController {
		
		//TODO Document
		//TODO Find a better solution to than this
		
		[SerializeField, Show]
		private RotationMode rotationMode;
		public RotationMode RotationMode {
			get {
				return rotationMode;
			}
			set {
				rotationMode = value;
			}
		}
		
		[SerializeField, Show]
		private float delay;
		public float Delay {
			get {
				return delay;
			}
			set {
				delay = value;
			}
		}
		
		[SerializeField, Show]
		private DynamicFloat angle;
		public DynamicFloat Angle {
			get {
				return angle;
			}
			set {
				angle = value;
			}
		}
		
		[SerializeField, Show]
		private Transform target;
		public Transform Target {
			get {
				return target;
			}
			set {
				target = value;
			}
		}

		#region implemented abstract members of IDanmakuController

		/// <summary>
		/// Updates the Danmaku controlled by the controller instance.
		/// </summary>
		/// <param name="danmaku">the bullet to update.</param>
		/// <param name="dt">the change in time since the last update</param>
		public void Update (Danmaku danmaku, float dt) {
			float time = danmaku.Time;
			if(time >= Delay && time - dt <= Delay) {
				float baseAngle = Angle.Value;
				switch(RotationMode) {
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
			}
		}

		#endregion
	}

}