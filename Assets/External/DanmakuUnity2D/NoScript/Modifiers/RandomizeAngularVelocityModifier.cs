using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D {
	
	[System.Serializable]
	public class RandomizeAngularVelocityModifier : FireModifier {
		
		[SerializeField]
		private DynamicFloat range = 0;
		
		#region implemented abstract members of FireModifier
		public override void Fire (Vector2 position, DynamicFloat rotation) {
			float oldAV = AngularVelocity;
			float rangeValue = range.Value;
			AngularVelocity = oldAV + Random.Range (-0.5f * rangeValue, 0.5f * rangeValue);
			FireSingle (position, rotation);
			AngularVelocity = oldAV;
		}
		#endregion
		
	}
	
	namespace Wrapper {

		[AddComponentMenu("Danmaku 2D/Modifiers/Randomize Angular Velocity Modifier")]
		internal class RandomizeAngularVelocityModifier : Modifier<Danmaku2D.RandomizeAngularVelocityModifier> {
		}
		
	}
	
}