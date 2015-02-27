using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using UnityUtilLib;

[CustomPropertyDrawer(typeof(Counter))]
public class CounterDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		SerializedProperty maxCountProp = property.FindPropertyRelative ("maxCount");
		SerializedProperty countProp = property.FindPropertyRelative ("count");
		EditorGUI.PropertyField (position, maxCountProp, label);
		countProp.intValue = maxCountProp.intValue;
		EditorGUI.EndProperty();
	}
}
