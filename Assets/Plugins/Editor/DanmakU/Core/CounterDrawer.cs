// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityEditor;

namespace DanmakU.Editor {

	[CustomPropertyDrawer(typeof(Counter))]
	internal class CounterDrawer : PropertyDrawer {
		
		public override  void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);
			SerializedProperty maxCountProp = property.FindPropertyRelative ("MaxCount");
			SerializedProperty countProp = property.FindPropertyRelative ("count");
			EditorGUI.PropertyField (position, maxCountProp, label);
			countProp.intValue = maxCountProp.intValue;
			EditorGUI.EndProperty();
		}

	}
}