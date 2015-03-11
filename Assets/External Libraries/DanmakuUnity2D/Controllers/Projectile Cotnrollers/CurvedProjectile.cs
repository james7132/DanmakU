using System;
using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D {

	/// <summary>
	/// A ProjectileController or ProjectileGroupController for creating bullets that move along a curved path.
	/// </summary>
	[Serializable]
	public class CurvedProjectile : LinearProjectile {

		[SerializeField]
		[Range(-360f, 360f)]
		private float angularVelocity;

		/// <summary>
		/// Gets or sets the angular velocity of the Projectile instance(s) controlled by this controller. <br>
		/// Expressed in degrees per second.
		/// </summary>
		/// <value>The angular velocity in degrees.</value>
		public float AngularVelocity {
			get {
				return angularVelocity;
			}
			set {
				angularVelocity = value;
			}
		}

		/// <summary>
		/// Gets or sets the angular velocity of the Projectile instance(s) controlled by this controller. <br>
		/// Expressed in radians per second.
		/// </summary>
		/// <value>The angular velocity in radians.</value>
		public float AngularVelocityRadians {
			get {
				return angularVelocity * Util.Degree2Rad;
			}
			set {
				angularVelocity = value * Util.Rad2Degree;
			}
		}


		public CurvedProjectile(float velocity, float angularVelocity) : base(velocity) {
			AngularVelocity = angularVelocity;
		}

		public override Vector2 UpdateProjectile (Projectile projectile, float dt) {
			if(AngularVelocity != 0f) {
				projectile.Rotation += AngularVelocity * dt;
			}
			return base.UpdateProjectile (projectile, dt);
		}
	}
}

