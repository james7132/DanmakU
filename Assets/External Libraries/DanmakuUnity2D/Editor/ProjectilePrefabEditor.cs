using UnityEngine;
using UnityEditor;
using System.Collections;
using Danmaku2D;

[CustomEditor(typeof(ProjectilePrefab))]
public class ProjectilePrefabEditor : Editor {

	public override void OnInspectorGUI () {
		base.OnInspectorGUI ();
		ProjectilePrefab pp = (ProjectilePrefab)target;
		if(GUILayout.Button("Reinitialize")) {
			pp.Reinit();
		}
	}
}
