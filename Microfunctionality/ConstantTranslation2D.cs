// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;

namespace UnityUtilLib {

	public class ConstantTranslation2D : CachedObject {

		public Vector2 Velocity;

		void Update() {
			transform.localPosition += (Vector3)(Velocity * Util.DeltaTime);
		}

	}

}