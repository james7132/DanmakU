using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using Danmaku2D;

[CustomPropertyDrawer(typeof(FieldMovementPattern.AtomicMovement))]
public class AtomicMovementDrawer : PropertyDrawer {

	float individualHeight = 17f;

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
		return 4 * individualHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		Rect timeRect = new Rect (position.x, position.y, position.width, individualHeight);
		Rect targetLocationRect = new Rect (position.x, position.y +  1 * individualHeight, position.width, individualHeight);
		SerializedProperty tl = property.FindPropertyRelative ("targetLocation");
		SerializedProperty t = property.FindPropertyRelative ("time");
		EditorGUI.PropertyField (timeRect, t);
		EditorGUI.PropertyField (targetLocationRect, tl);
		SerializedProperty c1 = property.FindPropertyRelative ("curveControlPoint1");
		SerializedProperty c2 = property.FindPropertyRelative ("curveControlPoint2");
		Rect point1Rect = new Rect (position.x, position.y +  2 * individualHeight, position.width, individualHeight);
		Rect point2Rect = new Rect (position.x, position.y +  3 * individualHeight, position.width, individualHeight);
		EditorGUI.PropertyField(point1Rect, c1);
		EditorGUI.PropertyField(point2Rect, c2);
		EditorGUI.EndProperty();
	}
}
