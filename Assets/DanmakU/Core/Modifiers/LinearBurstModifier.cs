// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;


namespace DanmakU.Modifiers {

	[System.Serializable]
	public class LinearBurstModifier : DanmakuModifier {
		
		[SerializeField, Show]
		private DynamicInt depth = 1;
		public DynamicInt Depth {
			get {
				return depth;
			}
			set {
				depth = value;
			}
		}
		
		[SerializeField, Show]
		private DynamicFloat deltaVelocity;
		public DynamicFloat DeltaVelocity {
			get {
				return deltaVelocity;
			}
			set {
				deltaVelocity = value;
			}
		}
		
		[SerializeField, Show]
		private DynamicFloat deltaAngularVelocity;
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
			DynamicFloat deltaV = DeltaVelocity;
			DynamicFloat deltaAV = DeltaAngularVelocity;
			float depth = Depth.Value;
			for(int i = 0; i < depth; i++) {
				Speed += deltaV;
				AngularSpeed += deltaAV;
				FireSingle(position, rotation);
			}

		}

		#endregion
	}
}
