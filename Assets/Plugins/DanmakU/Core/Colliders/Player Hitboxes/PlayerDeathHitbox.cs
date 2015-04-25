// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU {

	public class PlayerDeathHitbox : PlayerHitbox<DanmakuPlayer> {

		#region Fields

		private SpriteRenderer spriteRenderer;

		#endregion

		#region Standard Unity Callbacks

		public override void Awake() {
			base.Awake ();
			spriteRenderer = GetComponent<SpriteRenderer> ();
		}

		void Update() {
			if (spriteRenderer != null && Player != null) {
				spriteRenderer.enabled = Player.IsFocused;
			}
		}

		#endregion

		#region implemented abstract members of DanmakuCollider

		protected override void DanmakuCollision (Danmaku danmaku, RaycastHit2D info) {
			Player.Hit (danmaku);
			danmaku.Deactivate ();
		}

		#endregion
	}
}