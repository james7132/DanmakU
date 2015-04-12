// Copyright (C) 2015  James Liu
//	
//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.
//		
//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.
//			
//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>

using UnityEngine;
using UnityEditor;
using Danmaku2D;

/// <summary>
/// Custom <a href="http://docs.unity3d.com/ScriptReference/Editor.html">Editor</a> for ProjectilePrefab
/// </summary>
[CustomEditor(typeof(DanmakuPrefab))]
internal class DanmakuPrefabEditor : UnityEditor.Editor {

	/// <summary>
	/// Creates the custom Inspector GUI for an instance of ProjectilePrefab.
	/// Adds an extra button to quickly set all the values of the instance so that the user does not need to manually drag in each one.
	/// </summary>
	public override void OnInspectorGUI () {
		base.OnInspectorGUI ();
		DanmakuPrefab prefab = target as DanmakuPrefab;
		if(GUILayout.Button("Reinitialize")) {
			SerializedProperty collider = serializedObject.FindProperty("circleCollider");
			SerializedProperty renderer = serializedObject.FindProperty("spriteRenderer");
			collider.objectReferenceValue = prefab.GetComponent<CircleCollider2D>();
			renderer.objectReferenceValue = prefab.GetComponent<SpriteRenderer>();
//				DanmakuControlBehavior[] controllerScripts = prefab.GetComponents<DanmakuControlBehavior>();
//				controllers.arraySize = controllerScripts.Length;
//				for(int i = 0; i < controllerScripts.Length; i++) {
//					controllers.GetArrayElementAtIndex(i).objectReferenceValue = controllerScripts[i];
//				}
			serializedObject.ApplyModifiedProperties();
		}
	}
}