// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Controllers {

	[System.Serializable]
	public class SpeedLimitController : IDanmakuController {
		
		[SerializeField, Show]
		private float min;
		public float Min {
			get {
				return min;
			}
			set {
				min = value;
				if(min > max) {
					float temp = max;
					max = min;
					min = temp;
				}
			}
		}

		[SerializeField, Show]
		private float max;
		public float Max {
			get {
				return max;
			}
			set {
				max = value;
				if(min > max) {
					float temp = max;
					max = min;
					min = temp;
				}
			}
		}

		public SpeedLimitController () {
			min = float.NegativeInfinity;
			max = float.PositiveInfinity;
		}

		public SpeedLimitController(float minimum,
		                            float maximum) {
			min = minimum;
			max = maximum;
			if(min > max) {
				float temp = max;
				max = min;
				min = temp;
			}
		}

		public SpeedLimitController (float value) {
			float absValue = Mathf.Abs(value);
			min = -absValue;
			max = absValue;
		}

		public SpeedLimitController(float value, bool max) {
			if(max) {
				this.max = value;
				min = float.NegativeInfinity;
			} else {
				this.max = float.PositiveInfinity;
				min = value;
			}
		}

		#region IDanmakuController implementation

		/// <summary>
		/// Updates the Danmaku controlled by the controller instance.
		/// </summary>
		/// <param name="danmaku">the bullet to update.</param>
		/// <param name="dt">the change in time since the last update</param>
		public void Update (Danmaku danmaku, float dt) {
			float speed = danmaku.Speed;
			if(speed < min)
				danmaku.Speed = min;
			if(speed > max)
				danmaku.Speed = max;
		}

		#endregion
	}
}

