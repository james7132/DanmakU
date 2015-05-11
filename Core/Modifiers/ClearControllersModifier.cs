// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityUtilLib;

namespace DanmakU {
	
	public class ClearControllersModifier : DanmakuModifier {

		#region implemented abstract members of DanmakuModifier

		public override void Fire (Vector2 position, DynamicFloat rotation) {

			Controller = null;
			FireSingle (position, rotation);

		}

		#endregion

	}

}
