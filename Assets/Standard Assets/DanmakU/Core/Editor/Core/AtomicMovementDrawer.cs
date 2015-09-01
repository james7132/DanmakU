//// Copyright (c) 2015 James Liu
////	
//// See the LISCENSE file for copying permission.
//
//using UnityEngine;
//using UnityEditor;
//using System;
//using System.Collections;
//using DanmakU;
//
///// <summary>
///// Custom <a href="http://docs.unity3d.com/ScriptReference/PropertyDrawer.html">PropertyDrawer</a> for FieldMovementPattern.AtomicMovement
///// </summary>
//[CustomPropertyDrawer(typeof(FieldMovementPattern.AtomicMovement))]
//internal class AtomicMovementDrawer : PropertyDrawer {
//
//	float individualHeight = 17f;
//
//	/// <summary>
//	/// Gets the height of the property in the <a href="http://docs.unity3d.com/Manual/Inspector.html">Inspector</a>.
//	/// </summary>
//	/// <returns>The height in pixels.</returns>
//	/// <param name="property">The <a href="http://docs.unity3d.com/ScriptReference/SerializedProperty.html">SerializedProperty</a> to make the custom GUI for.</param>
//	/// <param name="label">The label of this property</param>
//	public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
//		return 4 * individualHeight;
//	}
//
//	/// <summary>
//	/// Creates the custom GUI an instance of FieldMovementPattern.AtomicMovement
//	/// </summary>
//	/// <param name="position"> the rectangle on the screen to use for the property GUI.</param>
//	/// <param name="property">The <a href="http://docs.unity3d.com/ScriptReference/SerializedProperty.html">SerializedProperty</a> to make the custom GUI for.</param>
//	/// <param name="label">The label of this property</param>
//	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
//		EditorGUI.BeginProperty(position, label, property);
//		Rect timeRect = new Rect (position.x, position.y, position.width, individualHeight);
//		Rect targetLocationRect = new Rect (position.x, position.y +  1 * individualHeight, position.width, individualHeight);
//		SerializedProperty tl = property.FindPropertyRelative ("targetLocation");
//		SerializedProperty t = property.FindPropertyRelative ("time");
//		EditorGUI.PropertyField (timeRect, t);
//		EditorGUI.PropertyField (targetLocationRect, tl);
//		SerializedProperty c1 = property.FindPropertyRelative ("curveControlPoint1");
//		SerializedProperty c2 = property.FindPropertyRelative ("curveControlPoint2");
//		Rect point1Rect = new Rect (position.x, position.y +  2 * individualHeight, position.width, individualHeight);
//		Rect point2Rect = new Rect (position.x, position.y +  3 * individualHeight, position.width, individualHeight);
//		EditorGUI.PropertyField(point1Rect, c1);
//		EditorGUI.PropertyField(point2Rect, c2);
//		EditorGUI.EndProperty();
//	}
//}