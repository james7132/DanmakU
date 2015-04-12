using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D {

	[System.Serializable]
	public class LineModifier : FireModifier {

		public DynamicInt Depth = 1;
		public DynamicFloat DeltaVelocity = 0f;
		public DynamicFloat DeltaAngularVelocity = 0f;

		#region implemented abstract members of FireModifier
		public override void Fire (Vector2 position, DynamicFloat rotation) {
			float deltaV = DeltaVelocity.Value;
			float deltaAV = DeltaAngularVelocity.Value;
			float depth = Depth.Value;
			for(int i = 0; i < depth; i++) {
				Velocity += deltaV;
				AngularVelocity += deltaAV;
				FireSingle(position, rotation);
			}

		}
		#endregion
	}

	namespace Wrapper {

		[AddComponentMenu("Danmaku 2D/Modifiers/Line Modifier")]
		internal class LineModifier : Modifier<Danmaku2D.LineModifier> {
		}

	}
}
