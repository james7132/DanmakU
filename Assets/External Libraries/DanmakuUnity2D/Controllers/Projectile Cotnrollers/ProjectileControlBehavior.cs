using System;
using UnityEngine;

namespace Danmaku2D {

	public abstract class ProjectileControlBehavior : MonoBehaviour, IProjectileGroupController {
		
		private SpriteRenderer spriteRenderer;
		public SpriteRenderer SpriteRenderer {
			get {
				if(spriteRenderer == null)
					spriteRenderer = (SpriteRenderer)GetComponent<Renderer>();
				return spriteRenderer;
			}
		}
		
		private CircleCollider2D circleColldier;
		public CircleCollider2D CircleCollider {
			get {
				if(circleColldier == null)
					circleColldier = (CircleCollider2D)GetComponent<Collider2D>();
				return circleColldier;
			}
		}
		
		public ProjectileGroup ProjectileGroup {
			get;
			set;
		}
		
		public virtual void Awake() {
			ProjectileGroup = new ProjectileGroup ();
			ProjectileGroup.Controller = this;
		}
		
		public abstract void UpdateProjectile(Projectile projectile, float dt);
	}
}

