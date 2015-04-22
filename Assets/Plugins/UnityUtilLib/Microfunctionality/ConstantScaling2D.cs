// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;

namespace UnityUtilLib {

	public class ConstantScaling2D : CachedObject {

		public Vector2 Scaling;

		void Update() {
			transform.localScale += (Vector3)(Scaling * Util.DeltaTime);
		}

	}
}