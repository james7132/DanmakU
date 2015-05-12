// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;


namespace DanmakU.Modifiers {

	[System.Serializable]
	public class LinearBurstModifier : DanmakuModifier {

		[Serialize, Show, Default(1)]
		public DynamicInt Depth {
			get;
			set;
		}

		[Serialize, Show, Default(0f)]
		public DynamicFloat DeltaVelocity {
			get;
			set;
		}
		
		[Serialize, Show, Default(0f)]
		public DynamicFloat DeltaAngularVelocity {
			get;
			set;
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
