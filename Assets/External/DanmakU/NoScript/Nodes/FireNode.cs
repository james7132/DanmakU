// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;

namespace DanmakU {

	public class FireNode : DanmakuNode {

		[SerializeField]
		private Transform target;

		protected override void Process() {
			if (target != null) {
				Target.Position = target.position;
				Target.Rotation = target.eulerAngles.z;
				DanmakuField.FindClosest (Target.Position).Fire (Target);
			} else {
				Debug.LogError("Tried to fire from a null transform");
			}
		}

	}

}
