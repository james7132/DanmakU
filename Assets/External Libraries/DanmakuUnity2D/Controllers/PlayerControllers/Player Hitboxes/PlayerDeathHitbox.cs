using UnityEngine;
using UnityUtilLib;
using System.Collections;

namespace Danmaku2D {
	public class PlayerDeathHitbox : MonoBehaviour, IProjectileCollider {

		private DanmakuPlayer player;

		void Start() {
			player = GetComponentInParent<DanmakuPlayer> ();
			if (player == null) {
				Debug.LogError("PlayerDeathHitbox should be on a child object of a GameObject with an Avatar sublcass script");
			}
		}

		public void OnProjectileCollision(Projectile proj) {
			if (player != null) {
				player.Hit (proj);
				proj.Deactivate();
			}
		}
	}
}