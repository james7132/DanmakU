using UnityEngine;
using System.Collections;

namespace UnityUtilLib.GUI {

	/// <summary>
	/// A simple script to quickly remove the mouse from a game
	/// </summary>
	public class RemoveMouse : MonoBehaviour  {
		void Awake () {
			#if UNITY_EDITOR
			Destroy (this);
			#else
			Screen.lockCursor = true;
			Destroy (this);
			#endif
		}
	}
}