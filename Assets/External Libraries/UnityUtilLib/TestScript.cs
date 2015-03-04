using UnityEngine;
using System.Collections;

namespace UnityUtilLib {

	/// <summary>
	/// A Test Script
	/// Any sublclass of this class will destroy itself if included with a non-Editor build
	/// Allows leaving in editor/test-only scripts that in the build while minimizing memory usage in deployment builds.
	/// </summary>
	public abstract class TestScript : CachedObject {

		/// <summary>
		/// Called upon Component instantiation
		/// Destroys the Component when included with a non-editor build
		/// </summary>
		public override void Awake() {
			#if UNITY_EDITOR
			base.Awake ();
			#else
			Destroy(this);
			#endif
		}
	}
}