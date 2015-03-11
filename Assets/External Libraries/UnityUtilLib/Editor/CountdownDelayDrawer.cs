using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using UnityUtilLib;

namespace UnityUtilLib.Editor {

	/// <summary>
	/// A custom <a href="http://docs.unity3d.com/ScriptReference/PropertyDrawer.html>PropertyDrawer</a> for CountdownDelay instances
	/// Abstracts away all of the hidden variables and only exposes a float field for easy editing
	/// </summary>
	[CustomPropertyDrawer(typeof(CountdownDelay))]
	public class CountdownDelayDrawer : PropertyDrawer {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);
			SerializedProperty maxDelayProp = property.FindPropertyRelative ("maxDelay");
			SerializedProperty currentDelayProp = property.FindPropertyRelative ("currentDelay");
			EditorGUI.PropertyField (position, maxDelayProp, label);
			currentDelayProp.floatValue = maxDelayProp.floatValue;
			EditorGUI.EndProperty();
		}
	}
}