using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {

	[System.Serializable]
	public class BurstModifier : FireModifier {

		public DynamicFloat Range = 360f;
		public DynamicInt Count = 1;
		public DynamicFloat DeltaVelocity = 0f;
		public DynamicFloat DeltaAngularVelocity = 0f;

		#region implemented abstract members of FireModifier

		public override void Fire (Vector2 position, DynamicFloat rotation) {

			int count = Count.Value;
			float range = Range.Value;
			float deltaV = DeltaVelocity.Value;
			float deltaAV = DeltaAngularVelocity.Value; 

			count = Mathf.Abs (count);

			float start = rotation - range * 0.5f;
			float delta = range / (count - 1);

			for (int i = 0; i < count; i++) {
				Velocity += deltaV;
				AngularVelocity += deltaAV;
				FireSingle(position, start + i * delta);
			}

		}

		#endregion

	}

	namespace Wrapper {

		[AddComponentMenu("Danmaku 2D/Modifiers/Burst Modifier")]
		public class BurstModifier : Modifier<Danmaku2D.BurstModifier> {
		}

	}
}
