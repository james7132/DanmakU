using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor scripts for various components of UnityUtilLib as well as 
/// </summary>
namespace UnityUtilLib.Editor {

	/// <summary>
	/// Custom <a href="http://docs.unity3d.com/ScriptReference/PropertyDrawer.html">PropertyDrawer</a> for FrameCounter
	/// </summary>
	[CustomPropertyDrawer(typeof(FrameCounter))]
	internal class FrameCounterEditor : PropertyDrawer {

		/// <summary>
		/// Creates the custom GUI an instance of FrameCounter
		/// Abstracts away all of the hidden variables and only exposes a float field for easy editing
		/// </summary>
		/// <param name="position"> the rectangle on the screen to use for the property GUI.</param>
		/// <param name="property">The <a href="http://docs.unity3d.com/ScriptReference/SerializedProperty.html">SerializedProperty</a> to make the custom GUI for.</param>
		/// <param name="label">The label of this property</param>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);
			SerializedProperty maxCountProp = property.FindPropertyRelative ("Time");
			EditorGUI.PropertyField (position, maxCountProp, label);
			EditorGUI.EndProperty();
		}
	}
}