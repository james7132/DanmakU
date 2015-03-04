using UnityEngine;
using System;
using System.Collections.Generic;

namespace Danmaku2D {
	public abstract class ProjectileController : IProjectileController {
		#region IProjectileController implementation

		public abstract Vector2 UpdateProjectile (float dt);

		public Projectile Projectile {
			get;
			set;
		}

		public int Damage {
			get {
				return Projectile.Damage;
			}
			set {
				Projectile.Damage = value;
			}
		}

		public ProjectilePrefab Prefab {
			get {
				return Projectile.Prefab;
			}
		}

		public Sprite Sprite {
			get {
				return Projectile.Sprite;
			}
		}

		public Color Color {
			get {
				return Projectile.Color;
			}
			set {
				Projectile.Color = value;
			}
		}

		public Vector2 Position {
			get {
				return Projectile.Position;
			}
			set {
				Projectile.Position = value;
			}
		}

		public float Rotation {
			get {
				return Projectile.Rotation;
			}
			set {
				Projectile.Rotation = value;
			}
		}

		public Vector2 Direction {
			get {
				return Projectile.Direction;
			}
		}

		public float Time {
			get {
				return Projectile.Time;
			}
		}

		public int Frames {
			get {
				return Projectile.Frames;
			}
		}

		#endregion


	}
}