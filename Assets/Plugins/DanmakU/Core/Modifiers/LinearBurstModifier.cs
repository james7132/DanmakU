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
		private DynamicFloat deltaSpeed;
		public DynamicFloat DeltaSpeed {
			get {
				return deltaSpeed;
			}
			set {
				deltaSpeed = value;
			}
		}
		
		[SerializeField, Show]
		private DynamicFloat deltaAngularSpeed;
		public DynamicFloat DeltaAngularSpeed {
			get {
				return deltaAngularSpeed;
			}
			set {
				deltaAngularSpeed = value;
			}
		}

		public LinearBurstModifier(DynamicInt depth, 
		                           DynamicFloat deltaSpeed, 
		                           DynamicFloat deltaAngularSpeed) {
			this.depth = depth;
			this.deltaSpeed = deltaSpeed;
			this.deltaAngularSpeed = deltaAngularSpeed;
		}

		#region implemented abstract members of FireModifier

		public override void OnFire (Vector2 position, DynamicFloat rotation) {
			DynamicFloat tempSpeed = Speed;
			DynamicFloat tempASpeed = AngularSpeed;
			DynamicFloat deltaV = DeltaSpeed;
			DynamicFloat deltaAV = DeltaAngularSpeed;
			float depth = Depth.Value;
			for(int i = 0; i < depth; i++) {
				Speed += deltaV;
				AngularSpeed += deltaAV;
				FireSingle(position, rotation);
			}
			Speed = tempSpeed;
			AngularSpeed = tempASpeed;
		}

		#endregion
	}
}
