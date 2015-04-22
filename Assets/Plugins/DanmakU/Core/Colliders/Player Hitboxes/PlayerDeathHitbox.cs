// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityUtilLib;
using System.Collections;

namespace DanmakU {

	public class PlayerDeathHitbox : PlayerHitbox<DanmakuPlayer> {

		private SpriteRenderer spriteRenderer;

		public override void Awake() {
			base.Awake ();
			spriteRenderer = GetComponent<SpriteRenderer> ();
		}

		void Update() {
			if (spriteRenderer != null && Player != null) {
				spriteRenderer.enabled = Player.IsFocused;
			}
		}

		#region implemented abstract members of DanmakuCollider

		protected override void ProcessDanmaku (Danmaku danmaku, RaycastHit2D info) {
			Player.Hit (danmaku);
			danmaku.Deactivate ();
		}

		#endregion
	}
}