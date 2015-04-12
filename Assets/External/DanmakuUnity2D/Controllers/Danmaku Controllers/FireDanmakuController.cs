using UnityEngine;
using System.Collections;

namespace Danmaku2D.DanmakuControllers {

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
		public void UpdateDanmaku (Danmaku danmaku, float dt) {
			//TODO: IMPLEMENT
		}
		#endregion
		
	}

	namespace Wrapper {

		[AddComponentMenu("Danmaku 2D/Controllers/Fire Danmaku Controller")]
		public class FireDanmakuController : ControllerWrapperBehavior<Danmaku2D.DanmakuControllers.FireDanmakuController> {
		}

	}
}
