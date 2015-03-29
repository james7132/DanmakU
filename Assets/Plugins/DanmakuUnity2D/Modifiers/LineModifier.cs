using UnityEngine;
using System.Collections;

namespace Danmaku2D {

	[System.Serializable]
	public class LineModifier : FireModifier {

		public int Depth = 1;
		public float DeltaVelocity = 0f;
		public float DeltaAngularVelocity = 0f;

		#region implemented abstract members of FireModifier
		public override void Fire (Vector2 position, float rotation) {

			for(int i = 0; i < Depth; i++) {
				Velocity += DeltaVelocity;
				AngularVelocity += DeltaAngularVelocity;
				FireSingle(position, rotation);
			}

		}
		#endregion
	
	}

}

namespace Danmaku2D.Wrapper {

	public class LineModifier : ModifierWrapper<Danmaku2D.LineModifier> {
	}
}