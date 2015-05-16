// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Modifiers {

	[System.Serializable]
	public class RandomizeAngleModifier : DanmakuModifier {
		
		[SerializeField, Show]
		private DynamicFloat range;
		public DynamicFloat Range {
			get {
				return range;
			}
			set {
				range = value;
			}
		}
		
		public RandomizeAngleModifier (DynamicFloat range) {
			this.range = range;
		}

		#region implemented abstract members of FireModifier

		public override void OnFire (Vector2 position, DynamicFloat rotation) {
			float rotationValue = rotation.Value;
			float rangeValue = Range.Value;
			FireSingle (position, rotationValue + 0.5f * Random.Range (-rangeValue, rangeValue));
		}

		#endregion

	}
}