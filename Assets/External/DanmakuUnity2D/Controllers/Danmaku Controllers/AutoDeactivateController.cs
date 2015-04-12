using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D.DanmakuControllers {

	[System.Serializable]
	public class AutoDeactivateController : IDanmakuController {

		[SerializeField]
		private bool useTime;

		[SerializeField]
		private int frames;

		public int Frames {
			get {
				return frames;
			}
			set {
				frames = value;
			}
		}

		public float Time {
			get {
				return Util.FramesToTime(frames);
			}
			set {
				frames = Util.TimeToFrames(value);
			}
		}

		public AutoDeactivateController () {
			frames = -1;
		}

		public AutoDeactivateController(int frames) {
			this.frames = frames;
		}

		public AutoDeactivateController(float time) {
			frames = Util.TimeToFrames (time);
		}

		#region IDanmakuController implementation
		public void UpdateDanmaku (Danmaku danmaku, float dt) {
			if (danmaku.frames > frames) {
				danmaku.Deactivate();
			}
		}
		#endregion
		
	}

	namespace Wrapper {
		
		[AddComponentMenu("Danmaku 2D/Controllers/Auto Deactivate Controller")]
		internal class AutoDeactivateController : ControllerWrapperBehavior<Danmaku2D.DanmakuControllers.AutoDeactivateController> {
		}

	}

}