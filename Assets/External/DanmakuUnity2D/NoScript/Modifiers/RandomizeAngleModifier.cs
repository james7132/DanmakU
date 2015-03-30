using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D {

	[System.Serializable]
	public class RandomizeAngleModifier : FireModifier {

		[SerializeField, Range(0f, 360f)]
		private DynamicFloat range = 0;

		#region implemented abstract members of FireModifier
		public override void Fire (Vector2 position, DynamicFloat rotation) {
			float rotationValue = rotation.Value;
			float rangeValue = range.Value;
			FireSingle (position, Random.Range (rotationValue - 0.5f * rangeValue, rotationValue + 0.5f * rangeValue));
		}
		#endregion

	}

	namespace Wrapper {

		internal class RandomizeAngleModifier : ModifierWrapper<Danmaku2D.RandomizeAngleModifier> {
		}

	}
}