using UnityEngine;
using System.Collections;

/// <summary>
/// A utilty library of random useful and portable scripts for Unity
/// </summary>
namespace UnityUtilLib {

	/// <summary>
	/// A Test Script
	/// Any sublclass of this class will destroy itself if included with a non-Editor build
	/// Allows leaving in editor/test-only scripts that in the build while minimizing memory usage in deployment builds.
	/// </summary>
	public abstract class TestScript : CachedObject {

		[SerializeField]
		private bool keepAnyway;

		/// <summary>
		/// Called upon Component instantiation
		/// Destroys the Component when included with a non-editor or non-debug/non-development build.
		/// </summary>
		public override void Awake() {
			#if UNITY_EDITOR
			base.Awake ();
			#else
			if(keepAnyway || Debug.isDebugBuild)
				base.Awake();
			else
				Destroy(this);
			#endif
		}
	}
}