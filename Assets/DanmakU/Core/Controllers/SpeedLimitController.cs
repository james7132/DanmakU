// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using Vexe.Runtime.Types;

namespace DanmakU.Controllers {

	[System.Serializable]
	public class SpeedLimitController : IDanmakuController {

		public enum LimitType { Maximum, Minimum }

		[Serialize, Show]
		public LimitType Type {
			get;
			set;
		}
		
		[Serialize, Show]
		public float Limit {
			get;
			set;
		}

		public SpeedLimitController () {
			Limit = float.NaN;
		}

		public SpeedLimitController(float limit, LimitType type) {
			this.Limit = limit;
			this.Type = type;
		}

		#region IDanmakuController implementation

		public void Update (Danmaku danmaku, float dt) {
			if(float.IsNaN(Limit))
				return;
			if(Type == LimitType.Maximum) {
				if(danmaku.Speed > Limit)
					danmaku.Speed = Limit;
			} else {
				if(danmaku.Speed < Limit)
					danmaku.Speed = Limit;
			}
		}

		#endregion
	}
}

