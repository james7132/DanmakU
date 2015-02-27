using UnityEngine;
using System.Collections;

namespace UnityUtilLib.GUI {
	public class RemoveMouse : MonoBehaviour  {
		void Awake () {
			#if UNITY_EDITOR
			Destroy (this);
			#else
			Screen.lockCursor = true;
			#endif
		}
	}
}