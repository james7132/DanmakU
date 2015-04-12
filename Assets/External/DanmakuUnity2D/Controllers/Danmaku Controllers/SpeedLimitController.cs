using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D.DanmakuControllers {

	[System.Serializable]
	public class SpeedLimitController : IDanmakuController {

		public enum LimitType { Maximum, Minimum }

		[SerializeField]
		private LimitType limitType;

		[SerializeField]
		private float limit;

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

	namespace Wrapper {

	}
}

