using UnityEngine;
using UnityEditor;
using Danmaku2D;

/// <summary>
/// Custom <a href="http://docs.unity3d.com/ScriptReference/Editor.html">Editor</a> for ProjectilePrefab
/// </summary>
[CustomEditor(typeof(ProjectilePrefab))]
internal class ProjectilePrefabEditor : UnityEditor.Editor {

	/// <summary>
	/// Creates the custom Inspector GUI for an instance of ProjectilePrefab.
	/// Adds an extra button to quickly set all the values of the instance so that the user does not need to manually drag in each one.
	/// </summary>
	public override void OnInspectorGUI () {
		base.OnInspectorGUI ();
		ProjectilePrefab prefab = target as ProjectilePrefab;
		if(GUILayout.Button("Reinitialize")) {
			SerializedProperty collider = serializedObject.FindProperty("circleCollider");
			SerializedProperty renderer = serializedObject.FindProperty("spriteRenderer");
			collider.objectReferenceValue = prefab.GetComponent<CircleCollider2D>();
			renderer.objectReferenceValue = prefab.GetComponent<SpriteRenderer>();
//				ProjectileControlBehavior[] controllerScripts = prefab.GetComponents<ProjectileControlBehavior>();
//				controllers.arraySize = controllerScripts.Length;
//				for(int i = 0; i < controllerScripts.Length; i++) {
//					controllers.GetArrayElementAtIndex(i).objectReferenceValue = controllerScripts[i];
//				}
			serializedObject.ApplyModifiedProperties();
		}
	}
}