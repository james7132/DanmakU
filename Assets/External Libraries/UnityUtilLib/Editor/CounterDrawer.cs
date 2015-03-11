using UnityEngine;
using UnityEditor;

namespace UnityUtilLib.Editor {

	/// <summary>
	/// A custom <a href="http://docs.unity3d.com/ScriptReference/PropertyDrawer.html>PropertyDrawer</a> for Counter instances
	/// Abstracts away all of the hidden variables and only exposes a int field for easy editing
	/// </summary>
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
}