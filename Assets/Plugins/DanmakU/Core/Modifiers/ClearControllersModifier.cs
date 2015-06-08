// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU.Modifiers {
	
	public class ClearControllersModifier : DanmakuModifier {

		#region implemented abstract members of DanmakuModifier

		public override void OnFire (Vector2 position, DynamicFloat rotation) {

			Controller = null;
			FireSingle (position, rotation);

		}

		#endregion

	}

}
