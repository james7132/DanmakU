using UnityEngine;
using System.Collections;

namespace Danmaku2D {

	[System.Serializable]
	public class AutoDeactivateController : IDanmakuController {
		
		public bool useTime;
		public int frames;

		#region IProjectileController implementation
		public void UpdateProjectile (Danmaku projectile, float dt) {
			if (projectile.frames > frames) {
				projectile.Deactivate();
			}
		}
		#endregion
		
	}

}