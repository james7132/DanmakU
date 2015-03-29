using System;

namespace Danmaku2D {
	public interface IProjectileCollider {
		void OnProjectileCollision(Projectile proj);
	}
}

