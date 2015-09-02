using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Vexe.Runtime.Extensions;
using UnityObject = UnityEngine.Object;

namespace Hourai.Editor {

    public static class EditorUtil {

/*       
        This somehow generates a compiler error. Commenting it out for now.

        public class HorizontalArea : IDisposable {

            public HorizontalArea(GUIStyle style, GUILayoutOption[] options) {
                if (options == null) {
                    if (style == null) {
                        EditorGUILayout.BeginHorizontal();
                    } else {
                        EditorGUILayout.BeginHorizontal(style);
                    }
                } else {
                    if (style == null) {
                        EditorGUILayout.BeginHorizontal(options);
                    } else {
                        EditorGUILayout.BeginHorizontal(style, options);
                    }
                }
            }
            
            public void Dispose() {
                EditorGUILayout.EndHorizontal();
            }
        }

        public static IDisposable Horizontal(GUIStyle style = null, params GUILayoutOption[] options) {
            return new HorizontalArea(style, options);
        }*/

        /// <summary>
        /// Since GameObject.GetComponentInChildren does not work on deactivated objects, which include 
        /// prefabs in the Editor. This method is a workaround for finding Components on children of
        /// prefabs.
        /// 
        /// This method also works with interfaces.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public static T GetComponentInChildren<T>(GameObject prefab) where T : class {
            if (prefab == null)
                throw new ArgumentNullException("prefab");

            return
                prefab.GetChildren()
                      .Select(child => child.GetComponent<T>())
                      .FirstOrDefault(instance => instance != null);
        }

        /// <summary>
        /// Since GameObject.GetComponentsInChildren does not work on deactivated objects, which include 
        /// prefabs in the Editor. This method is a workaround for finding Components on children of
        /// prefabs.
        /// 
        /// This method also works with interfaces.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public static T[] GetComponentsInChildren<T>(GameObject prefab) where T : class {
            if (prefab == null)
                throw new ArgumentNullException("prefab");


            return prefab.GetChildren().SelectMany(child => child.GetComponents<T>()).ToArray();
        }

        public static T GetComponentInChildren<T>(Component prefab) where T : class {
            if (prefab == null)
                throw new ArgumentNullException("prefab");
            return GetComponentInChildren<T>(prefab.gameObject);
        }

        public static T[] GetComponentsInChildren<T>(Component prefab) where T : class {
            if (prefab == null)
                throw new ArgumentNullException("prefab");
            return GetComponentsInChildren<T>(prefab.gameObject);
        }

        public static string Text(string label, string text) {
            return EditorGUILayout.TextField(text, label);
        }

        public static void Space(int? space = null) {
            if (space == null)
                GUILayout.FlexibleSpace();
            else
                GUILayout.Space((int) space);
        }

    }

}