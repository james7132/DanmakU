using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D {

	[System.Serializable]
	public class RandomizeVelocityModifier : FireModifier {

		[SerializeField]
		private DynamicFloat range = 0;

		#region implemented abstract members of FireModifier
		public override void Fire (Vector2 position, DynamicFloat rotation) {
			float oldVelocity = Velocity;
			float rangeValue = range.Value;
			Velocity = oldVelocity + Random.Range (-0.5f * rangeValue, 0.5f * rangeValue);
			FireSingle (position, rotation);
			Velocity = oldVelocity;
		}
		#endregion

	}

	namespace Wrapper {

		internal class RandomizeVelocityModifier : ModifierWrapper<Danmaku2D.RandomizeVelocityModifier> {
		}

	}

}
