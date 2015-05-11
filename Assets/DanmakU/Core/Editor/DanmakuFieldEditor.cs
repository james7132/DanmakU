// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DanmakU;


namespace DanmakU.Editor {

	[CustomEditor(typeof(DanmakuField))]
	internal class DanmakuFieldEditor : UnityEditor.Editor {

		private DanmakuField field;

		private SerializedProperty useClipBoundary;
		private SerializedProperty clipBoundary;
		private SerializedProperty camera3D;
		private SerializedProperty camera2D;
		private SerializedProperty size;

		public void OnEnable() {
			field = target as DanmakuField;

			useClipBoundary = serializedObject.FindProperty("UseClipBoundary");
			clipBoundary = serializedObject.FindProperty ("ClipBoundary");
			size = serializedObject.FindProperty("FieldSize");
			camera2D = serializedObject.FindProperty ("camera2D");
			camera3D = serializedObject.FindProperty ("otherCameras");
		}

		public override bool RequiresConstantRepaint () {
			return true;
		}

		public override void OnInspectorGUI () {
			serializedObject.Update ();
			EditorGUILayout.PropertyField(useClipBoundary);
			if (useClipBoundary.boolValue) {
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField (clipBoundary);
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.PropertyField (camera2D, new GUIContent("2D Cameras"));

			EditorGUI.indentLevel++;
			if (camera2D.objectReferenceValue == null) {
				EditorGUILayout.PropertyField(size);
			} else {
				EditorGUILayout.PropertyField(camera3D,true);
			}
			EditorGUI.indentLevel--;

			if (UnityEngine.GUI.changed) {
				serializedObject.ApplyModifiedProperties ();
				field.enabled = true;
			}
		}
	}
}