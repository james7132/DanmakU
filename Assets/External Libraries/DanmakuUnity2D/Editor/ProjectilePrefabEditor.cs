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
			pp.CircleCollider = pp.GetComponent<CircleCollider2D>();
			pp.SpriteRenderer = pp.GetComponent<SpriteRenderer>();
		}
	}
}
