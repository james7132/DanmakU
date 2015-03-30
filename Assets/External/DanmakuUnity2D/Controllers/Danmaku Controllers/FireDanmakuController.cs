using UnityEngine;
using System.Collections;

namespace Danmaku2D {

	[System.Serializable]
	public class FireDanmakuController : IDanmakuController {

		[SerializeField]
		private float delay;

		[SerializeField]
		private bool repeat;

		[SerializeField]
		private bool deactivateAfterwards;

		[SerializeField]
		private DanmakuEmitter emitter;

		#region IDanmakuController implementation
		public void UpdateProjectile (Danmaku projectile, float dt) {
		}
		#endregion
		
	}

}
