using UnityEngine;
using System.Collections;

namespace Danmaku2D {

	[System.Serializable]
	public class BurstModifier : FireModifier {

		public float Range = 360f;
		public int Count = 1;
		public float DeltaVelocity = 0f;
		public float DeltaAngularVelocity = 0f;

		#region implemented abstract members of FireModifier

		public override void Fire (Vector2 position, float rotation) {

			Count = Mathf.Abs (Count);

			float start = rotation - Range * 0.5f;
			float delta = Range / (Count - 1);

			for (int i = 0; i < Count; i++) {
				Velocity += DeltaVelocity;
				AngularVelocity += DeltaAngularVelocity;
				FireSingle(position, start + i * delta);
			}

		}

		#endregion

	}
}

namespace Danmaku2D.Wrapper {
	public class BurstModifier : ModifierWrapper<Danmaku2D.BurstModifier> {
	}
}