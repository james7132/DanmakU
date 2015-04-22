using UnityEngine;

namespace DanmakU {

	public class HomingController : IDanmakuController {

		#region IDanmakuController implementation
		
		public void UpdateDanmaku (Danmaku danmaku, float dt) {
			danmaku.AngularSpeed = 0f;
			danmaku.Rotation = danmaku.Field.AngleTowardPlayer (danmaku.Position);
		}
		
		#endregion

	}


}