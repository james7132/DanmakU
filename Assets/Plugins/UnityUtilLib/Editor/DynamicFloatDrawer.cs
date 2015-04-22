// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using UnityEngine;
using UnityEditor;

namespace UnityUtilLib.Editor {

	[CustomPropertyDrawer(typeof(DynamicInt))]
	public class DynamicIntDrawer : PropertyDrawer {
		
		private const float typeWidth = 0.2f;
		private const float fieldWidth = 0.2f;
		
		override public void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty (position, label, property);
			SerializedProperty center = property.FindPropertyRelative ("centerValue");
			SerializedProperty range = property.FindPropertyRelative ("range");
			SerializedProperty mode = property.FindPropertyRelative ("Mode");
			Rect minRect = position;
			Rect maxRect = position;
			Rect combinedRect = position;
			Rect labelRect = position;
			Rect modeRect = position;
			modeRect.width = typeWidth * position.width;
			modeRect.x = position.x + position.width - modeRect.width;
			minRect.width = maxRect.width = fieldWidth * position.width;
			maxRect.x = modeRect.x - maxRect.width;
			minRect.x = maxRect.x - minRect.width;
			combinedRect.x = minRect.x;
			combinedRect.width = minRect.width + maxRect.width;
			labelRect.width = minRect.x - labelRect.x;
			EditorGUI.PrefixLabel (labelRect, label);
			EditorGUI.PropertyField (modeRect, mode, GUIContent.none);
			switch (mode.enumValueIndex) {
			case 0:	//constant
				center.floatValue = EditorGUI.IntField(combinedRect, GUIContent.none, Mathf.RoundToInt(center.floatValue));
				range.floatValue = 0f;
				break;
			case 1: // random
				float min, max;
				min = center.floatValue - range.floatValue;
				max = center.floatValue + range.floatValue;
				min = EditorGUI.IntField(minRect, GUIContent.none, Mathf.RoundToInt(min));
				max = EditorGUI.IntField(maxRect, GUIContent.none, Mathf.RoundToInt(max));
				center.floatValue = (min + max) /2;
				range.floatValue = Mathf.Abs(center.floatValue - min);
				break;
			}
			EditorGUI.EndProperty ();
		}
	}

	[CustomPropertyDrawer(typeof(DynamicFloat))]
	public class DynamicFloatDrawer : PropertyDrawer {

		private const float typeWidth = 0.2f;
		private const float fieldWidth = 0.2f;

		override public void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty (position, label, property);
			SerializedProperty center = property.FindPropertyRelative ("centerValue");
			SerializedProperty range = property.FindPropertyRelative ("range");
			SerializedProperty type = property.FindPropertyRelative ("type");
			Rect minRect = position;
			Rect maxRect = position;
			Rect combinedRect = position;
			Rect labelRect = position;
			Rect typeRect = position;
			typeRect.width = typeWidth * position.width;
			typeRect.x = position.x + position.width - typeRect.width;
			minRect.width = maxRect.width = fieldWidth * position.width;
			maxRect.x = typeRect.x - maxRect.width;
			minRect.x = maxRect.x - minRect.width;
			combinedRect.x = minRect.x;
			combinedRect.width = minRect.width + maxRect.width;
			labelRect.width = minRect.x - labelRect.x;
			EditorGUI.PrefixLabel (labelRect, label);
			EditorGUI.PropertyField (typeRect, type, GUIContent.none);
			switch (type.enumValueIndex) {
				case 0:	//constant
					EditorGUI.PropertyField(combinedRect, center, GUIContent.none);
					range.floatValue = 0f;
					break;
				case 1: // random
					float min, max;
					min = center.floatValue - range.floatValue;
					max = center.floatValue + range.floatValue;
					min = EditorGUI.FloatField(minRect, GUIContent.none, min);
					max = EditorGUI.FloatField(maxRect, GUIContent.none, max);
					center.floatValue = (min + max) /2;
					range.floatValue = Mathf.Abs(center.floatValue - min);
					break;
			}
			EditorGUI.EndProperty ();
		}
	}
}

