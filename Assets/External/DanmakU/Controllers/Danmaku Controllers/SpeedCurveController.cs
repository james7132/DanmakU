// Copyright (C) 2015  James Liu
//	
//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.
//		
//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.
//			
//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>

using UnityEngine;

namespace DanmakU.DanmakuControllers {

	[System.Serializable]
	public class SpeedCurveController : IDanmakuController {

		[SerializeField]
		private bool absolute;

		public bool Absolute {
			get {
				return absolute;
			}
			set {
				absolute = value;
			}
		}

		[SerializeField]
		private AnimationCurve speedCuve;

		public AnimationCurve SpeedCurve {
			get {
				return SpeedCurve;
			}
		}

		#region IDanmakuController implementation
		public virtual void UpdateDanmaku (Danmaku danmaku, float dt) {
			if (absolute) {
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

		[SerializeField]
		private bool absolute;

		public bool Absolute {
			get {
				return absolute;
			}
			set {
				absolute = value;
			}
		}

		[SerializeField]
		private AnimationCurve angularSpeedCurve;

		public AnimationCurve AngularSpeedCurve {
			get {
				return angularSpeedCurve;
			}
		}
		
		#region IDanmakuController implementation
		public virtual void UpdateDanmaku (Danmaku danmaku, float dt) {
			if (absolute) {
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

