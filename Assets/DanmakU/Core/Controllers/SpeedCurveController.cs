// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Controllers {

	[System.Serializable]
	public class SpeedCurveController : IDanmakuController {

		[Serialize, Show]
		public bool Absolute {
			get;
			set;
		}

		[Serialize, Show]
		private AnimationCurve speedCuve;
		public AnimationCurve SpeedCurve {
			get {
				return SpeedCurve;
			}
		}

		#region IDanmakuController implementation
		public virtual void Update (Danmaku danmaku, float dt) {
			if (Absolute) {
				danmaku.Speed = speedCuve.Evaluate (danmaku.Time);
			} else {
				float time = danmaku.Time;
				float oldTime = time - dt;
				if(oldTime > 0) {
					float deltaV = speedCuve.Evaluate(time) - speedCuve.Evaluate(oldTime);
					danmaku.Speed += deltaV;
				}
			}
		}
		#endregion
	}

	[System.Serializable]
	public class AngularSpeedCurveController : IDanmakuController {

		[Serialize, Show]
		public bool Absolute {
			get;
			set;
		}

		[Serialize, Show]
		private AnimationCurve angularSpeedCurve;
		public AnimationCurve AngularSpeedCurve {
			get {
				return angularSpeedCurve;
			}
		}
		
		#region IDanmakuController implementation

		public virtual void Update (Danmaku danmaku, float dt) {
			if (Absolute) {
				danmaku.AngularSpeed = angularSpeedCurve.Evaluate(danmaku.Time);
			} else {
				float time = danmaku.Time;
				float oldTime = time - dt;
				if(oldTime > 0) {
					float deltaV = angularSpeedCurve.Evaluate(time) - angularSpeedCurve.Evaluate(oldTime);
					danmaku.AngularSpeed += deltaV;
				}
			}
		}

		#endregion

	}
}

