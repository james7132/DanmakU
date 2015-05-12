// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Modifiers {

	[System.Serializable]
	public class RandomizeAngleModifier : DanmakuModifier {

		[Serialize, Show]
		public DynamicFloat Range {
			get;
			set;
		}

		#region implemented abstract members of FireModifier

		public override void Fire (Vector2 position, DynamicFloat rotation) {
			float rotationValue = rotation.Value;
			float rangeValue = Range.Value;
			FireSingle (position, Random.Range (rotationValue - 0.5f * rangeValue, rotationValue + 0.5f * rangeValue));
		}

		#endregion

	}
}