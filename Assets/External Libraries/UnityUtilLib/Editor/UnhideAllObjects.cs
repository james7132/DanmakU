using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnityUtilib.Editor {
	public static class UnhideAllObjects {

		[MenuItem("Tools/Unhide All Objects")]
		public static void UnhideAll() {
			Object[] allObjects = Resources.FindObjectsOfTypeAll<Object> ();
			for (int i = 0; i < allObjects.Length; i++) {
				if(allObjects[i].name.Contains("Scene"))
					allObjects[i].hideFlags = 0;
			}
		}

	}
}