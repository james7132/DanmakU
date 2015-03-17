using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D.ProjectileControllers {

	public class CurvedProjectileControl : ControllerWrapperBehavior<CurvedProjectile> {

		[SerializeField]
		private CurvedProjectile controller;
	
		#region implemented abstract members of ControllerWrapperBehavior
		protected override CurvedProjectile CreateController () {
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
	/// A ProjectileController or ProjectileGroupController for creating bullets that move along a curved path.
	/// </summary>
	[System.Serializable]
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
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Danmaku2D.ProjectileControllers.CurvedProjectile"/> class.
		/// </summary>
		/// <param name="velocity">the velocity in absolute world coordinates traveled by the projectiles in one second.</param>
		/// <param name="angularVelocity">the change in rotation in degrees the bullets rotate by in one second</param>
		public CurvedProjectile(float velocity, float angularVelocity) : base(velocity) {
			AngularVelocity = angularVelocity;
		}
		
		public override void UpdateProjectile (Projectile projectile, float dt) {
			if(AngularVelocity != 0f) {
				projectile.Rotation += AngularVelocity * dt;
			}
			base.UpdateProjectile (projectile, dt);
		}
	}
}