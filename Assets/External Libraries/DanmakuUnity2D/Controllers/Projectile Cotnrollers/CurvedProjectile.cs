using System;
using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D {

	[Serializable]
	public class CurvedProjectile : LinearProjectile {

		[SerializeField]
		[Range(-360f, 360f)]
		private float angularVelocity;

		public float AngularVelocity {
			get {
				return angularVelocity;
			}
			set {
				angularVelocity = value;
			}
		}
		
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

