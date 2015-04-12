using UnityEngine;
namespace Danmaku2D.DanmakuControllers {

	[System.Serializable]
	public class AnimationCurveController : IDanmakuController {

		[SerializeField]
		private AnimationCurve velocityCurve;

		#region IDanmakuController implementation
		public virtual void UpdateDanmaku (Danmaku danmaku, float dt) {
			float velocity = velocityCurve.Evaluate (danmaku.Time);
			if (velocity != 0)
				danmaku.Position += danmaku.Direction * velocity * dt;
		}
		#endregion
	}

	namespace Wrapper {
		
		[AddComponentMenu("Danmaku 2D/Controllers/Animation Curve Controller")]
		internal class AnimationCurveController : ControllerWrapperBehavior<AnimationCurveController> {
		}

	}
}

