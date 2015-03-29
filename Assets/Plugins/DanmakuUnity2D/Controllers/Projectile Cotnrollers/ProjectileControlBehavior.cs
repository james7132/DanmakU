using System;
using UnityEngine;

namespace Danmaku2D {

	public abstract class ProjectileControlBehavior : MonoBehaviour, IProjectileController {
		
		public ProjectileGroup ProjectileGroup {
			get;
			set;
		}
		
		public virtual void Awake() {
			ProjectileGroup = new ProjectileGroup ();
			ProjectileGroup.AddController(this);
		}

		#region IProjectileController implementation

		public abstract void UpdateProjectile (Projectile projectile, float dt);

		#endregion
	}
}

