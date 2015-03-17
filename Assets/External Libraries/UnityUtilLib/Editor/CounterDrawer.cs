using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor scripts for various components of UnityUtilLib
/// </summary>
namespace UnityUtilLib.Editor {

	/// <summary>
	/// Custom <a href="http://docs.unity3d.com/ScriptReference/PropertyDrawer.html">PropertyDrawer</a> for Counter
	/// </summary>
	[CustomPropertyDrawer(typeof(Counter))]
	internal class CounterDrawer : PropertyDrawer {

		/// <summary>
		/// Creates the custom GUI an instance of Counter
		/// Abstracts away all of the hidden variables and only exposes a int field for easy editing
		/// </summary>
		/// <param name="position"> the rectangle on the screen to use for the property GUI.</param>
		/// <param name="property">The <a href="http://docs.unity3d.com/ScriptReference/SerializedProperty.html">SerializedProperty</a> to make the custom GUI for.</param>
		/// <param name="label">The label of this property</param>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);
			SerializedProperty maxCountProp = property.FindPropertyRelative ("MaxCount");
			SerializedProperty countProp = property.FindPropertyRelative ("Count");
			EditorGUI.PropertyField (position, maxCountProp, label);
			countProp.intValue = maxCountProp.intValue;
			EditorGUI.EndProperty();
		}
	}
}