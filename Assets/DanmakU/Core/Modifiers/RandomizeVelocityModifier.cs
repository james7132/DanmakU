// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;


namespace DanmakU {

	[System.Serializable]
	public class RandomizeVelocityModifier : DanmakuModifier {

		[SerializeField]
		private DynamicFloat range = 0;

		#region implemented abstract members of FireModifier
		public override void Fire (Vector2 position, DynamicFloat rotation) {
			DynamicFloat oldVelocity = Speed;
			float rangeValue = range.Value;
			Speed = oldVelocity + Random.Range (-0.5f * rangeValue, 0.5f * rangeValue);
			FireSingle (position, rotation);
			Speed = oldVelocity;
		}
		#endregion

	}

}
