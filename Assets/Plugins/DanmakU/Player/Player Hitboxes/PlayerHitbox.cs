// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU {

	[RequireComponent(typeof(Collider2D))]
	public abstract class PlayerHitbox<T> : DanmakuCollider where T : DanmakuPlayer {

		[RequiredFromParents]
		protected T Player;

		public override void Awake() {
			base.Awake ();
			Player = GetComponentInParent<T> ();
		}

	}
}

