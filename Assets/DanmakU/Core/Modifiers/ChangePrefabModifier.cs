// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Modifiers {

	public class ChangePrefabModifier : DanmakuModifier {

		[SerializeField]
		public DanmakuPrefab NewPrefab {
			get;
			set;
		}

		#region implemented abstract members of DanmakuModifier
		public override void Fire (Vector2 position, DynamicFloat rotation) {

			DanmakuPrefab oldPrefab = Prefab;
			Prefab = NewPrefab;

			FireSingle (position, rotation);

			Prefab = oldPrefab;

		}
		#endregion
	}

}