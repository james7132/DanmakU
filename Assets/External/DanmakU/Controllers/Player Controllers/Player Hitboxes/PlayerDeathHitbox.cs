// Copyright (C) 2015  James Liu
//	
//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.
//		
//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.
//			
//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>

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

		public void OnDanmakuCollision(Danmaku danmaku) {
			if (player != null) {
				player.Hit (danmaku);
				danmaku.Deactivate();
			}
		}
	}
}