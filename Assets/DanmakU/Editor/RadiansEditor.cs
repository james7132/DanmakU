using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DanmakU {

[CanEditMultipleObjects]
[CustomPropertyDrawer(typeof(RadiansAttribute))]
internal class RadiansEditor : PropertyDrawer {

  const float buttonSize = 30f;

  Dictionary<string, bool> _propertyType;

  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
    var min = property.FindPropertyRelative("_min");
    var max = property.FindPropertyRelative("_max");

    _propertyType = _propertyType ?? (_propertyType = new Dictionary<string, bool>());

    switch (property.propertyType) {
      case SerializedPropertyType.Float:
        var degVal = property.floatValue * Mathf.Rad2Deg;
        EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
        degVal = EditorGUI.FloatField(position, label, degVal);
        EditorGUI.showMixedValue = false;
        if (property.hasMultipleDifferentValues) {
          property.floatValue = degVal * Mathf.Deg2Rad;
        }
        break;
      default: 
        if (min != null && max != null) {
          bool isRange;
          if (!_propertyType.TryGetValue(property.propertyPath, out isRange)) {
            isRange = !Mathf.Approximately(min.floatValue, max.floatValue);
            _propertyType.Add(property.propertyPath, isRange);
          }
          var values = new[] { min.floatValue * Mathf.Rad2Deg, max.floatValue * Mathf.Rad2Deg };
          _propertyType[property.propertyPath] = DrawRangeEditor(position, property, label, isRange, values);
          if (!min.hasMultipleDifferentValues) min.floatValue = values[0] * Mathf.Deg2Rad;
          if (!max.hasMultipleDifferentValues) max.floatValue = values[1] * Mathf.Deg2Rad;
        } else {
          EditorGUI.PropertyField(position, property, label);
        }
        break;
    }
  }

  static bool DrawRangeEditor(Rect position, SerializedProperty property, GUIContent label, bool isRange, float[] values) {
    var fieldPosition = position;
    var buttonPosition = position;
    fieldPosition.width -= buttonSize;
    buttonPosition.x += fieldPosition.width;
    buttonPosition.width = buttonSize;

    var min = property.FindPropertyRelative("_min");
    var max = property.FindPropertyRelative("_max");

    EditorGUI.BeginProperty(position, label, property);
    if (isRange) {
        MultiFloatField(fieldPosition, label, 
                        new[] { new GUIContent("-"), new GUIContent("+")},
                        new[] { min.hasMultipleDifferentValues, max.hasMultipleDifferentValues },
                        values);
    } else {
      EditorGUI.showMixedValue = min.hasMultipleDifferentValues || max.hasMultipleDifferentValues;
      values[0] = values[1] = EditorGUI.FloatField(fieldPosition, label, values[0]);
      EditorGUI.showMixedValue = false;
    }
    if (GUI.Button(buttonPosition, isRange ? "\u2194" : "\u2022")) {
      isRange = !isRange;
      if (!isRange) {
        values[0] = values[1] = (values[0] + values[1]) / 2;
      }
    }
    EditorGUI.EndProperty();
    return isRange;
  }

  static void MultiFloatField(Rect position, GUIContent label, GUIContent[] subLabels, bool[] mixed, float[] values) {
    int controlId = GUIUtility.GetControlID("foldout".GetHashCode(), FocusType.Passive, position);
    position = MultiFieldPrefixLabel(position, controlId, label, subLabels.Length);
    position.height = 16f;
    MultiFloatField(position, subLabels, mixed, values);
  }

  static void MultiFloatField(Rect position, GUIContent[] subLabels, bool[] mixed, float[] values) {
    int length = values.Length;
    float num = (position.width - (float) (length - 1) * 2f) / (float) length;
    Rect position1 = new Rect(position);
    position1.width = num;
    float labelWidth1 = EditorGUIUtility.labelWidth;
    int indentLevel = EditorGUI.indentLevel;
    EditorGUIUtility.labelWidth = 13f;
    EditorGUI.indentLevel = 0;
    for (int index = 0; index < values.Length; ++index) {
      EditorGUI.showMixedValue = mixed[index];
      values[index] = EditorGUI.FloatField(position1, subLabels[index], values[index]);
      EditorGUI.showMixedValue = false;
      position1.x += num + 2f;
    }
    EditorGUIUtility.labelWidth = labelWidth1;
    EditorGUI.indentLevel = indentLevel;
  }

  static bool LabelHasContent(GUIContent label) {
    if (label == null || label.text != string.Empty)
      return true;
    return label.image != null;
  }

  static Rect MultiFieldPrefixLabel(Rect totalPosition, int id, GUIContent label, int columns) {
    if (!LabelHasContent(label))
      return EditorGUI.IndentedRect(totalPosition);
    var indent = EditorGUI.indentLevel * 15f;
    if (EditorGUIUtility.wideMode) {
      Rect labelPosition = new Rect(totalPosition.x + indent, totalPosition.y, EditorGUIUtility.labelWidth - indent, 16f);
      Rect rect = totalPosition;
      rect.xMin += EditorGUIUtility.labelWidth;
      if (columns > 1) {
        --labelPosition.width;
        --rect.xMin;
      }
      if (columns == 2) {
        float num = (float) (((double) rect.width - 4.0) / 3.0);
        rect.xMax -= num + 2f;
      }
      EditorGUI.HandlePrefixLabel(totalPosition, labelPosition, label, id);
      return rect;
    }
    Rect labelPosition1 = new Rect(totalPosition.x + indent, totalPosition.y, totalPosition.width - indent, 16f);
    Rect rect1 = totalPosition;
    rect1.xMin += indent + 15f;
    rect1.yMin += 16f;
    EditorGUI.HandlePrefixLabel(totalPosition, labelPosition1, label, id);
    return rect1;
  }

}

}