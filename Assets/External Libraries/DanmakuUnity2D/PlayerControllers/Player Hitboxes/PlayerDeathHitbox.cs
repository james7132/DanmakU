using UnityEngine;
using UnityUtilLib;
using System.Collections;

namespace Danmaku2D {
	public class PlayerDeathHitbox : CachedObject {

		private AbstractPlayableCharacter player;

		void Start() {
			player = GetComponentInParent<AbstractPlayableCharacter> ();
			if (player == null) {
				Debug.LogError("PlayerDeathHitbox should be on a child object of a GameObject with an Avatar sublcass script");
			}
		}

		void OnProjectileCollision(Projectile proj) {
			if (player != null) {
				player.Hit (proj);
				proj.Deactivate();
			}
		}
	}
}