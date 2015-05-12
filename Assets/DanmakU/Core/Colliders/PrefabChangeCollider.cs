// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Collider {
	
	[AddComponentMenu("DanmakU/Colliders/Prefab Change Collider")]
	public class PrefabChangeCollider : DanmakuCollider {

		[SerializeField, Show]
		private DanmakuPrefab prefab;
		public DanmakuPrefab Prefab {
			get {
				return prefab;
			}
			set {
				prefab = value;
			}
		}

		private DanmakuGroup affected;

		public override void Awake () {
			base.Awake ();
			affected = new DanmakuGroup ();
		}

		#region implemented abstract members of DanmakuCollider
		protected override void DanmakuCollision (Danmaku danmaku, RaycastHit2D info) {
			if (affected.Contains (danmaku))
				return;

			danmaku.MatchPrefab (prefab);

			affected.Add (danmaku);

		}
		#endregion
	}

}
