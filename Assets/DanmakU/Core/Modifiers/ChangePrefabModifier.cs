// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU.Modifiers {

	public class ChangePrefabModifier : DanmakuModifier {

		[SerializeField]
		private DanmakuPrefab prefab;

		public DanmakuPrefab Prefab {
			get {
				return prefab;
			}
			set {
				prefab = value;
			}
		}

		#region implemented abstract members of DanmakuModifier
		public override void Fire (Vector2 position, DynamicFloat rotation) {

			DanmakuPrefab oldPrefab = Prefab;
			Prefab = prefab;

			FireSingle (position, rotation);

			Prefab = oldPrefab;

		}
		#endregion
	}

}