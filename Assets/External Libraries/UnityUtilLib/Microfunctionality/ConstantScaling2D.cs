using UnityEngine;
using System.Collections;

namespace UnityUtilLib {

	internal class ConstantScaling2D : CachedObject {

		[SerializeField]
		private Vector2 scaling;

		void Update() {
			transform.localScale += (Vector3)(scaling * Util.TargetDeltaTime);
		}

	}
}