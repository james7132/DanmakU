// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Controllers {

	[System.Serializable]
	public class SpeedLimitController : IDanmakuController {

		public enum LimitType { Maximum, Minimum }
		
		[SerializeField, Show]
		private LimitType type;
		public LimitType Type {
			get {
				return type;
			}
			set {
				type = value;
			}
		}
		
		[SerializeField, Show]
		private float limit;
		public float Limit {
			get {
				return limit;
			}
			set {
				limit = value;
			}
		}

		public SpeedLimitController () {
			Limit = float.NaN;
		}

		public SpeedLimitController(float limit, LimitType type) {
			this.limit = limit;
			this.type = type;
		}

		#region IDanmakuController implementation

		public void Update (Danmaku danmaku, float dt) {
			if(float.IsNaN(limit))
				return;
			if(type == LimitType.Maximum) {
				if(danmaku.Speed > limit)
					danmaku.Speed = limit;
			} else {
				if(danmaku.Speed < limit)
					danmaku.Speed = limit;
			}
		}

		#endregion
	}
}

