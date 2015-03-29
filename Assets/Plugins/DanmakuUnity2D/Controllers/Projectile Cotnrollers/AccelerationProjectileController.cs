using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D.ProjectileControllers {

	public class AccelerationProjectileController : ControllerWrapperBehavior<AccelerationController> {
		[SerializeField]
		private AccelerationController controller;

		#region implemented abstract members of ControllerWrapperBehavior
		protected override AccelerationController CreateController () {
			return controller;
		}
		#endregion
	}

}

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {
	
	/// <summary>
	/// A ProjectileController or ProjectileGroupController for creating bullets that move along a straight path.
	/// </summary>
	[System.Serializable]
	public class AccelerationController : IProjectileController {

		[SerializeField]
		private float acceleration = 0;

		[SerializeField]
		private float capSpeed;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Danmaku2D.ProjectileControllers.LinearProjectile"/> class.
		/// </summary>
		/// <value>The velocity of the controlled Projectile(s) in absolute world coordinates per second</value>
		public AccelerationController (float acceleration, float capSpeed) : base() {
			this.acceleration = acceleration;
			this.capSpeed = capSpeed;
		}
		
		#region IProjectileController implementation
		
		public virtual void UpdateProjectile (Projectile projectile, float dt) {
			if (acceleration != 0) {
				float velocity = projectile.Velocity;
				velocity += acceleration * projectile.Time;
				if (acceleration < 0 && velocity < capSpeed) {
					velocity = capSpeed;
				} else if (acceleration > 0 && velocity > capSpeed) {
					velocity = capSpeed;
				}
				projectile.Velocity = velocity;
			}
		}
		
		#endregion
	}
}

