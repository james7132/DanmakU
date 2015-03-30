using UnityEngine;
using UnityUtilLib;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {
	
	/// <summary>
	/// A ProjectileController or ProjectileGroupController for creating bullets that move along a straight path.
	/// </summary>
	[System.Serializable]
	public class AccelerationController : IDanmakuController {

		[SerializeField]
		private DynamicFloat acceleration;

		[SerializeField]
		private DynamicFloat capSpeed;

		public AccelerationController() {
			this.acceleration = (DynamicFloat)0f;
			this.capSpeed = (DynamicFloat)0f;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Danmaku2D.ProjectileControllers.LinearProjectile"/> class.
		/// </summary>
		/// <value>The velocity of the controlled Projectile(s) in absolute world coordinates per second</value>
		public AccelerationController (DynamicFloat acceleration, DynamicFloat capSpeed) : base() {
			this.acceleration = acceleration;
			this.capSpeed = capSpeed;
		}
		
		#region IProjectileController implementation
		
		public virtual void UpdateProjectile (Danmaku projectile, float dt) {
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

	namespace Wrapper {
		
		public class AccelerationProjectileController : ControllerWrapperBehavior<AccelerationController> {
		}
		
	}

}

