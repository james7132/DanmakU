// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;

namespace UnityUtilLib {

	internal class ConstantRotation2D : CachedObject {

		[Range(-360f, 360f)]
		public float AngularVelocity;
		
		// Update is called once per frame
		void Update () {
			Quaternion angV = Quaternion.Euler(0f, 0f, AngularVelocity * Util.DeltaTime);
			transform.localRotation *= angV;
		}
	}
}