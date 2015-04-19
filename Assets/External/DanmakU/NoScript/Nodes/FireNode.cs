// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityUtilLib;

namespace DanmakU {

	public class FireNode : DanmakuNode {

		[SerializeField]
		private Transform source;

		public override void Process(FireBuilder target) {
			if (target != null) {
				Vector2 tempPos = target.Position;
				DynamicFloat tempRotation = target.Rotation;
				DanmakuField.CoordinateSystem coordSys = target.CoordinateSystem;
				target.Position = source.position;
				target.Rotation = source.eulerAngles.z;
				target.CoordinateSystem = DanmakuField.CoordinateSystem.World;
				Field.Fire (target);
				target.Position = tempPos;
				target.Rotation = tempRotation;
				target.CoordinateSystem = coordSys;
			} else {
				Debug.LogError("Tried to fire from a null transform");
			}
		}

	}

}
