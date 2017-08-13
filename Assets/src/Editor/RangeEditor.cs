using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DanmakU {

    [CustomPropertyDrawer(typeof(Range))]
    public class RangeDrawer : PropertyDrawer {

        const float buttonSize = 30f;

        Dictionary<string, bool> _propertyType;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var fieldPosition = position;
            var buttonPosition = position;
            fieldPosition.width -= buttonSize;
            buttonPosition.x += fieldPosition.width;
            buttonPosition.width = buttonSize;

            var min = property.FindPropertyRelative("Min");
            var max = property.FindPropertyRelative("Max");

            if (_propertyType == null)
                _propertyType = new Dictionary<string, bool>();

            bool isRange;
            if (!_propertyType.TryGetValue(property.propertyPath, out isRange)) {
                isRange = !Mathf.Approximately(min.floatValue, max.floatValue);
                _propertyType.Add(property.propertyPath, isRange);
            }

            EditorGUI.BeginProperty(position, label, property);
            if (isRange) {
                var values = new[] { min.floatValue, max.floatValue };
                EditorGUI.MultiFloatField(fieldPosition, label, 
                                          new[] { new GUIContent("-"), new GUIContent("+")},
                                          values);
                min.floatValue = values[0];
                max.floatValue = values[1];
            } else {
                var value = EditorGUI.FloatField(fieldPosition, label ,min.floatValue);
                min.floatValue = value;
                max.floatValue = value;
            }
            if (GUI.Button(buttonPosition, isRange ? "\u2194" : "\u2022")) {
                isRange = !isRange;
                if (!isRange) {
                    var average = (min.floatValue + max.floatValue) / 2;
                    min.floatValue = average;
                    max.floatValue = average;
                }
                _propertyType[property.propertyPath] = isRange;
            }
            EditorGUI.EndProperty();
        }

    }

}

