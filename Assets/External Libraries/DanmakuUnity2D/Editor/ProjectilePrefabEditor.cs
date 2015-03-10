using UnityEngine;
using UnityEditor;
using System.Collections;
using Danmaku2D;

[CustomEditor(typeof(ProjectilePrefab))]
public class ProjectilePrefabEditor : Editor {

	public override void OnInspectorGUI () {
		base.OnInspectorGUI ();
		ProjectilePrefab prefab = target as ProjectilePrefab;
		if(GUILayout.Button("Reinitialize")) {
			SerializedProperty collider = serializedObject.FindProperty("circleCollider");
			SerializedProperty renderer = serializedObject.FindProperty("spriteRenderer");
			SerializedProperty controllers = serializedObject.FindProperty("extraControllers");
			collider.objectReferenceValue = prefab.GetComponent<CircleCollider2D>();
			renderer.objectReferenceValue = prefab.GetComponent<SpriteRenderer>();
			ProjectileControlBehavior[] controllerScripts = prefab.GetComponents<ProjectileControlBehavior>();
			controllers.arraySize = controllerScripts.Length;
			for(int i = 0; i < controllerScripts.Length; i++) {
				controllers.GetArrayElementAtIndex(i).objectReferenceValue = controllerScripts[i];
			}
			serializedObject.ApplyModifiedProperties();
		}
	}
}
