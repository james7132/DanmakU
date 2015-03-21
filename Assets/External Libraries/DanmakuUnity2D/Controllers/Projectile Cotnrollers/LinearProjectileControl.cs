using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D.ProjectileControllers {

	public class LinearProjectileControl : ControllerWrapperBehavior<LinearProjectile> {
		[SerializeField]
		private LinearProjectile controller;

		#region implemented abstract members of ControllerWrapperBehavior
		protected override LinearProjectile CreateController () {
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
	public class LinearProjectile : IProjectileController {
		
		[SerializeField]
		private float velocity = 5;

		[SerializeField]
		private float acceleration = 0;

		[SerializeField]
		private float capSpeed;

		/// <summary>
		/// Gets or sets the velocity of the controlled Projectile(s)
		/// </summary>
		/// <value>The velocity of the controlled Projectile(s) in absolute world coordinates per second</value>
		public float Velocity {
			get {
				return velocity;
			}
			set {
				velocity = value;
			}
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Danmaku2D.ProjectileControllers.LinearProjectile"/> class.
		/// </summary>
		/// <value>The velocity of the controlled Projectile(s) in absolute world coordinates per second</value>
		public LinearProjectile (float velocity, float acceleration = 0f, float capSpeed = float.NaN) : base() {
			Velocity = velocity;
			SetAcceleration (acceleration, capSpeed);
		}

		public void SetAcceleration(float accel, float cap) {
			if(!float.IsNaN(cap)) {
				if(Util.Sign(accel) == Util.Sign(cap - velocity)) {
					acceleration = accel;
					capSpeed = cap;
				} else {
					acceleration = 0f;
					velocity = cap;
				}
			} else {
				acceleration = 0f;
				capSpeed = 0f;
			}
		}
		
		#region IProjectileController implementation
		
		public virtual void UpdateProjectile (Projectile projectile, float dt) {
//			if(acceleration != 0) {
//				float accelSign = Util.Sign(acceleration);
//				if(accelSign == Util.Sign(capSpeed - velocity)) {
//					velocity += acceleration * dt;
//					if((accelSign < 0 && velocity < capSpeed) || (accelSign > 0 && velocity > capSpeed)) {
//						velocity = capSpeed;
//					}
//				} else {
//					velocity = capSpeed;
//					acceleration = 0;
//				}
//			}
			if (velocity != 0)
				projectile.Position += projectile.Direction * velocity * dt;
		}
		
		#endregion
	}
}

