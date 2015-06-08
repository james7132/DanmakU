// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityEditor;

namespace DanmakU.Editor {
	
	[CustomPropertyDrawer(typeof(DynamicInt))]
	public class DynamicIntDrawer : PropertyDrawer {
		
		private const float typeWidth = 0.2f;
		private const float fieldWidth = 0.2f;
		
		override public void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty (position, label, property);
			SerializedProperty maxProp = property.FindPropertyRelative ("max");
			SerializedProperty minProp = property.FindPropertyRelative ("min");
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
				maxProp.intValue = EditorGUI.IntField(combinedRect, GUIContent.none, Mathf.RoundToInt(maxProp.intValue));
				minProp.intValue = maxProp.intValue;
				break;
			case 1: // random
				maxProp.intValue = EditorGUI.IntField(minRect, GUIContent.none, maxProp.intValue);
				minProp.intValue = EditorGUI.IntField(minRect, GUIContent.none, minProp.intValue);
				if(minProp.intValue > maxProp.intValue) {
					int temp = maxProp.intValue;
					maxProp.intValue = minProp.intValue;
					minProp.intValue = temp;
				}
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
			SerializedProperty minProp = property.FindPropertyRelative ("min");
			SerializedProperty maxProp = property.FindPropertyRelative ("max");
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
				EditorGUI.PropertyField(combinedRect, minProp, GUIContent.none);
				maxProp.floatValue = minProp.floatValue;
				break;
			case 1: // random
				minProp.floatValue = EditorGUI.FloatField(minRect, GUIContent.none, minProp.floatValue);
				maxProp.floatValue = EditorGUI.FloatField(maxRect, GUIContent.none, maxProp.floatValue);
				if(minProp.floatValue > maxProp.floatValue) {
					float temp = maxProp.floatValue;
					maxProp.floatValue = minProp.floatValue;
					minProp.floatValue = temp;
				}
				break;
			}
			EditorGUI.EndProperty ();
		}
	}
}