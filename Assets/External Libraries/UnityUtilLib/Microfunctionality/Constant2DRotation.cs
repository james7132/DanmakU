using UnityEngine;
using System.Collections;

namespace UnityUtilLib {

	public class Constant2DRotation : MonoBehaviour {

		[Range(-360f, 360f)]
		public float angularVelocity;
		
		// Update is called once per frame
		void Update () {
			Quaternion angV = Quaternion.Euler(0f, 0f, angularVelocity * Util.TargetDeltaTime);
			transform.localRotation *= angV;
		}
	}
}