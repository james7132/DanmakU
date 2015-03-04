using System;
using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D {
	public class CurvedProjectile : LinearProjectile {

		public float AngularVelocity {
			get;
			set;
		}
		
		public float AngularVelocityRadians {
			get {
				return AngularVelocity * Util.Degree2Rad;
			}
			set {
				AngularVelocity = value * Util.Rad2Degree;
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

