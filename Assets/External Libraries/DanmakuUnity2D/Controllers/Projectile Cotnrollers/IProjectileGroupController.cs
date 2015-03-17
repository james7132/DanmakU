using System;
using UnityEngine;

namespace Danmaku2D {

	public interface IProjectileGroupController {

		/// <summary>
		/// Gets or sets the ProjectileGroup controlled by this 
		/// </summary>
		/// <value>The projectile group.</value>
		ProjectileGroup ProjectileGroup { get; set; }
		void UpdateProjectile (Projectile projectile, float dt);
	}
}

