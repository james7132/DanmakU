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
		
		public RandomizeAngularVelocityModifier (DynamicFloat range) {
			this.range = range;
		}

		#region implemented abstract members of FireModifier

		public override void OnFire (Vector2 position, DynamicFloat rotation) {
			float oldAV = AngularSpeed;
			float rangeValue = Range.Value;
			AngularSpeed = oldAV + 0.5f *  Random.Range (-rangeValue, rangeValue);
			FireSingle (position, rotation);
			AngularSpeed = oldAV;
		}

		#endregion
		
	}
}