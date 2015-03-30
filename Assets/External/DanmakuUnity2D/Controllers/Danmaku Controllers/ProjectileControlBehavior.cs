using System;
using UnityEngine;

namespace Danmaku2D {

	public abstract class ProjectileControlBehavior : MonoBehaviour, IDanmakuController {
		
		public DanmakuGroup ProjectileGroup {
			get;
			set;
		}
		
		public virtual void Awake() {
			ProjectileGroup = new DanmakuGroup ();
			ProjectileGroup.AddController(this);
		}

		#region IProjectileController implementation

		public abstract void UpdateProjectile (Danmaku projectile, float dt);

		#endregion
	}
}

