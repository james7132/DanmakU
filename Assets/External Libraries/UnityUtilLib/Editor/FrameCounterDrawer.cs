using UnityEngine;
using System.Collections;
using UnityEditor;

namespace UnityUtilLib {
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