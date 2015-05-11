// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;


namespace DanmakU {

	[System.Serializable]
	public class CircularBurstModifier : DanmakuModifier {

		public DynamicFloat Range = 360f;
		public DynamicInt Count = 1;
		public DynamicFloat DeltaVelocity = 0f;
		public DynamicFloat DeltaAngularVelocity = 0f;

		#region implemented abstract members of FireModifier

		public override void Fire (Vector2 position, DynamicFloat rotation) {

			int count = Count.Value;

			count = Mathf.Abs (count);

			if (count == 1) {
				FireSingle (position, rotation);
			} else {
				float range = Range.Value;
				float start = rotation - range * 0.5f;
				float delta = range / (count - 1);
				
				float deltaV = DeltaVelocity.Value;
				float deltaAV = DeltaAngularVelocity.Value;
				for (int i = 0; i < count; i++) {
					Speed += deltaV;
					AngularSpeed += deltaAV;
					FireSingle(position, start + i * delta);
				}
			}
		}

		#endregion

	}
}
