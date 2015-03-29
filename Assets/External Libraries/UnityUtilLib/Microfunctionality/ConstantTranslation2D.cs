using UnityEngine;
using System.Collections;

namespace UnityUtilLib {

	internal class ConstantTranslation2D : CachedObject {

		[SerializeField]
		private Vector2 velocity;

		void Update() {
			transform.localPosition += (Vector3)(velocity * Util.TargetDeltaTime);
		}

	}

}