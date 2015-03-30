using UnityEngine;
using Danmaku2D.ProjectileControllers;

namespace Danmaku2D {

	[System.Serializable]
	public class AnimationCurveController : IDanmakuController {

		[SerializeField]
		private AnimationCurve velocityCurve;

		#region IProjectileController implementation
		public virtual void UpdateProjectile (Danmaku projectile, float dt) {
			float velocity = velocityCurve.Evaluate (projectile.Time);
			if (velocity != 0)
				projectile.Position += projectile.Direction * velocity * dt;
		}
		#endregion
	}

	namespace Wrapper {
		internal class AnimationCurveController : ControllerWrapperBehavior<AnimationCurveController> {
		}
	}
}

