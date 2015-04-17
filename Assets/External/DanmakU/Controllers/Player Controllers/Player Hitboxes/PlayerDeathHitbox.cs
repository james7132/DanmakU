// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityUtilLib;
using System.Collections;

namespace DanmakU {

	public class PlayerDeathHitbox : MonoBehaviour, IDanmakuCollider {

		private DanmakuPlayer player;
		private SpriteRenderer spriteRenderer;

		void Awake() {
			spriteRenderer = GetComponent<SpriteRenderer> ();
			player = GetComponentInParent<DanmakuPlayer> ();
			if (player == null) {
				Debug.LogError("PlayerDeathHitbox should be on a child object of a GameObject with an Avatar sublcass script");
			}
		}

		void Update() {
			if (spriteRenderer != null && player != null) {
				spriteRenderer.enabled = player.IsFocused;
			}
		}

		#region IDanmakuCollider implementation
		public void OnDanmakuCollision (Danmaku danmaku) {
			if (player != null) {
				player.Hit (danmaku);
				danmaku.Deactivate();
			}
		}
		#endregion
	}
}