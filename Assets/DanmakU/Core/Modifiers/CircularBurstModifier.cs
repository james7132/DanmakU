// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Modifiers {

	[System.Serializable]
	public class CircularBurstModifier : DanmakuModifier {

		[SerializeField, Show]
		private DynamicFloat range = 360f;
		public DynamicFloat Range {
			get {
				return range;
			}
			set {
				range = value;
			}
		}
		
		[SerializeField, Show]
		private DynamicInt count = 1;
		public DynamicInt Count {
			get {
				return count;
			}
			set {
				count = value;
			}
		}
		
		[SerializeField, Show]
		private DynamicFloat deltaVelocity = 0f;
		public DynamicFloat DeltaVelocity {
			get {
				return deltaVelocity;
			}
			set {
				deltaVelocity = value;
			}
		}
		
		[SerializeField, Show]
		private DynamicFloat deltaAngularVelocity = 0f;
		public DynamicFloat DeltaAngularVelocity {
			get {
				return deltaAngularVelocity;
			}
			set {
				deltaAngularVelocity = value;
			}
		}

		#region implemented abstract members of FireModifier

		public override void Fire (Vector2 position, DynamicFloat rotation) {

			int burstCount = count.Value;

			burstCount = Mathf.Abs (burstCount);

			if (burstCount == 1) {
				FireSingle (position, rotation);
			} else {
				float burstRange = range.Value;
				float start = rotation - burstRange * 0.5f;
				float delta = burstRange / (burstCount - 1);
				
				float deltaV = deltaVelocity.Value;
				float deltaAV = deltaAngularVelocity.Value;
				for (int i = 0; i < burstCount; i++) {
					Speed += deltaV;
					AngularSpeed += deltaAV;
					FireSingle(position, start + i * delta);
				}
			}

		}

		#endregion

	}
}
