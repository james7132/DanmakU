using System;
using UnityEngine;

namespace Danmaku2D {
	public interface IProjectileGroupController {
		ProjectileGroup ProjectileGroup { get; set; }
		Vector2 UpdateProjectile (Projectile projectile, float dt);
	}
}

