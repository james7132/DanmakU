using UnityEngine;
using System;
using System.Collections.Generic;

namespace Danmaku2D {
	public interface IProjectileController {
		Projectile Projectile { get; set; }
		Vector2 UpdateProjectile (float dt);

		int Damage { get; set; }
		ProjectilePrefab Prefab { get; }
		Sprite Sprite { get; }
		Color Color { get; set; }

		Vector2 Position { get; set; }
		float Rotation { get; set; }
		Vector2 Direction { get; }
		
		float Time { get; }
		int Frames { get; }
	}
}