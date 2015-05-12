// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Modifiers {
	
	[System.Serializable]
	public class RandomizeAngularVelocityModifier : DanmakuModifier {
		
		[SerializeField, Show]
		DynamicFloat range;
		public DynamicFloat Range {
			get {
				return range;
			}
			set {
				range = value;
			}
		}
		
		#region implemented abstract members of FireModifier

		public override void Fire (Vector2 position, DynamicFloat rotation) {
			float oldAV = AngularSpeed;
			float rangeValue = Range.Value;
			AngularSpeed = oldAV + Random.Range (-0.5f * rangeValue, 0.5f * rangeValue);
			FireSingle (position, rotation);
			AngularSpeed = oldAV;
		}

		#endregion
		
	}
}