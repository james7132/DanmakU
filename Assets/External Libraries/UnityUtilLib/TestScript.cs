using UnityEngine;
using System.Collections;

namespace UnityUtilLib {
	public class TestScript : MonoBehaviour {
		public virtual void Awake() {
	#if UNITY_EDITOR
			//No problem to let these run in the editor
	#elif UNITY_EDITOR_WIN
			//Still no problem
	#else
			Destroy(this);
	#endif
		}
	}
}