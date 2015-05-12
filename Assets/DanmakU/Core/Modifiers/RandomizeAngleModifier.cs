// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;


namespace DanmakU.Modifiers {

	[System.Serializable]
	public class RandomizeAngleModifier : DanmakuModifier {

		[SerializeField, Range(0f, 360f)]
		private DynamicFloat range = 0;

		#region implemented abstract members of FireModifier
		public override void Fire (Vector2 position, DynamicFloat rotation) {
			float rotationValue = rotation.Value;
			float rangeValue = range.Value;
			FireSingle (position, Random.Range (rotationValue - 0.5f * rangeValue, rotationValue + 0.5f * rangeValue));
		}
		#endregion

	}
}