// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Modifiers {

	public class SetPrefabModifier : DanmakuModifier {

		[SerializeField, Show]
		private DanmakuPrefab newPrefab;
		public DanmakuPrefab NewPrefab {
			get {
				return newPrefab;
			}
			set {
				newPrefab = value;
			}
		}

		public SetPrefabModifier(DanmakuPrefab prefab) {
			if(prefab == null)
				throw new System.ArgumentNullException();
			newPrefab = prefab;
		}

		#region implemented abstract members of DanmakuModifier

		public override void OnFire (Vector2 position, DynamicFloat rotation) {

			DanmakuPrefab oldPrefab = Prefab;
			Prefab = newPrefab;

			FireSingle (position, rotation);

			Prefab = oldPrefab;

		}

		#endregion
	}

}