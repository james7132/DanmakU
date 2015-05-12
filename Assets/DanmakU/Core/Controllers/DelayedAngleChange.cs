// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Controllers {

	[System.Serializable]
	public class DelayedAngleChange : IDanmakuController {

		//TODO Find a better solution to than this

		[Serialize, Show]
		public RotationMode RotationMode {
			get;
			set;
		}

		[Serialize, Show]
		public float Delay {
			get;
			set;
		}

		[Serialize, Show]
		public DynamicFloat Angle {
			get;
			set;
		}
		
		[Serialize, Show]
		public Transform Target {
			get;
			set;
		}

		#region implemented abstract members of IDanmakuController

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