// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU.DanmakuControllers {

	[System.Serializable]
	public class SpeedLimitController : IDanmakuController {

		public enum LimitType { Maximum, Minimum }

		[SerializeField]
		private LimitType limitType;

		[SerializeField]
		private float limit;

		public LimitType LimitType2 {
			get {
				return limitType;
			}
			set {
				limitType = value;
			}
		}

		public float Limit {
			get {
				return limit;
			}
			set {
				limit = value;
			}
		}

		public SpeedLimitController () {
			limit = float.NaN;
		}

		public SpeedLimitController(float limit, LimitType type) {
			this.limit = limit;
			this.limitType = type;
		}

		#region IDanmakuController implementation

		public void UpdateDanmaku (Danmaku danmaku, float dt) {
			if(limitType == LimitType.Maximum) {
				if(danmaku.Speed > limit && !float.IsNaN(limit)) {
					danmaku.Speed = limit;
				}
			} else {
				if(danmaku.Speed < limit && !float.IsNaN(limit)) {
					danmaku.Speed = limit;
				}
			}
		}

		#endregion
	}
}

