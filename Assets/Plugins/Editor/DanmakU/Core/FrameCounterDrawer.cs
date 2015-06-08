// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityEditor;

namespace DanmakU.Editor {
	
	[CustomPropertyDrawer(typeof(FrameCounter))]
	internal class FrameCounterDrawer : PropertyDrawer {
		
		public override  void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);
			SerializedProperty timeProp = property.FindPropertyRelative ("Time");
			EditorGUI.PropertyField (position, timeProp, label);
			EditorGUI.EndProperty();
		}
		
	}
}