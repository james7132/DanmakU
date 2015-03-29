using UnityEngine;
using System.Collections;

namespace Danmaku2D {

	[System.Serializable]
	public class AutoDeactivateController : IProjectileController {
		
		public bool useTime;
		public int frames;

		#region IProjectileController implementation
		public void UpdateProjectile (Projectile projectile, float dt) {
			if (projectile.frames > frames) {
				projectile.Deactivate();
			}
		}
		#endregion
		
	}

}