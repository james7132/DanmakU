using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hourai.Editor {

    /// <summary>
    /// Removes the extra "Script" field on any editor derived from this.
    /// </summary>
    [CanEditMultipleObjects]
    public abstract class ScriptlessEditor : UnityEditor.Editor {

        private List<string> toIgnore = new List<string>();

        protected virtual void OnEnable() {
            toIgnore = new List<string>();
            toIgnore.Add("m_Script");
        }

        public void AddException(string propertyName) {
            toIgnore.Add(propertyName);
        }

        public new void DrawDefaultInspector() {
            SerializedProperty iterator = serializedObject.GetIterator();
            iterator.Next(true);
            while (iterator.NextVisible(false)) {
                if (!toIgnore.Contains(iterator.name))
                    EditorGUILayout.PropertyField(iterator, true);
            }

            if (GUI.changed)
                serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
        }

    }

    [CustomEditor(typeof (MonoBehaviour), true, isFallback = true)]
    internal sealed class MonoBehaviourEditor : ScriptlessEditor {

    }

    [CustomEditor(typeof (ScriptableObject), true, isFallback = true)]
    internal sealed class ScriptableObjectEditor : ScriptlessEditor {

    }
}