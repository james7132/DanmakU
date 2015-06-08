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
		private DynamicFloat deltaSpeed = 0f;
		public DynamicFloat DeltaSpeed {
			get {
				return deltaSpeed;
			}
			set {
				deltaSpeed = value;
			}
		}
		
		[SerializeField, Show]
		private DynamicFloat deltaAngularSpeed = 0f;
		public DynamicFloat DeltaAngularSpeed {
			get {
				return deltaAngularSpeed;
			}
			set {
				deltaAngularSpeed = value;
			}
		}

		public CircularBurstModifier(DynamicFloat range, 
		                             DynamicInt count, 
		                             DynamicFloat deltaSpeed, 
		                             DynamicFloat deltaAngularSpeed) {
			this.range = range;
			this.count = count;
			this.deltaSpeed = deltaSpeed;
			this.deltaAngularSpeed = deltaAngularSpeed;
		}

		#region implemented abstract members of FireModifier

		public override void OnFire (Vector2 position, DynamicFloat rotation) {

			int burstCount = Mathf.Abs(count.Value);

			if (burstCount == 1) {
				FireSingle (position, rotation);
			} else {
				float burstRange = range.Value;
				float start = rotation - burstRange * 0.5f;
				float delta = burstRange / (burstCount - 1);
				
				float deltaV = deltaSpeed.Value;
				float deltaAV = deltaAngularSpeed.Value;

				DynamicFloat tempSpeed = Speed;
				DynamicFloat tempASpeed = AngularSpeed;

				for (int i = 0; i < burstCount; i++) {
					Speed += deltaV;
					AngularSpeed += deltaAV;
					FireSingle(position, start + i * delta);
				}

				Speed = tempSpeed;
				AngularSpeed = tempASpeed;
			}

		}

		#endregion

	}
}
