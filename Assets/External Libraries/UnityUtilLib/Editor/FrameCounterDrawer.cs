using UnityEngine;
using System.Collections;
using UnityEditor;

namespace UnityUtilLib.Editor {
	
	/// <summary>
	/// A custom <a href="http://docs.unity3d.com/ScriptReference/PropertyDrawer.html>PropertyDrawer</a> for FrameCounter instances
	/// Abstracts away all of the hidden variables and only exposes a float field for easy editing
	/// </summary>
	[CustomPropertyDrawer(typeof(FrameCounter))]
	public class FrameCounterEditor : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);
			SerializedProperty maxCountProp = property.FindPropertyRelative ("delay");
			EditorGUI.PropertyField (position, maxCountProp, label);
			EditorGUI.EndProperty();
		}
	}
}