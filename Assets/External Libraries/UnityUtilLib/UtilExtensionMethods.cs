using System;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilLib.Interface;

namespace UnityUtilLib
{
    /// <summary>
    /// Class with util extension methods.
    /// The class is organized for easy reading.
    /// #region alphabetic order
    /// inside the region, method alphabetic order
    /// </summary>
    public static class UtilExtensionMethods
    {
        #region Array

        /// <summary>
        /// Clones an array. To use it, your type must implement the IClonable interface.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="array">Array</param>
        /// <returns>Returns a copy of the original array</returns>
        public static T[] Clone<T>(this T[] array) where T : IClonable<T>
        {
            var newArray = new T[array.Length];
            for (var i = 0; i < array.Length; i++)
                newArray[i] = array[i].Clone();
            return newArray;
        }

        #endregion

        #region GameObject

        /// <summary>
        /// Returns a List<> of the object's children.
        /// </summary>
        /// <param name="go">GameObject</param>
        /// <returns>The object's children.</returns>
        public static List<GameObject> GetChildrenList(this GameObject go) {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform tran in go.transform) {
                children.Add(tran.gameObject);
            }
            return children;
        }

        /// <summary>
        /// Safe get component method. Is a defensive alternative to the GetComponent method.
        /// This will tell you when it does not find the component.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="obj">Game Object</param>
        /// <returns>Returns the component or null.</returns>
        public static T SafeGetComponent<T>(this GameObject obj) where T : MonoBehaviour {
            T component = obj.GetComponent<T>();
            if (component == null) {
				Debug.LogError ("ExtensionMethods.SafeGetComponent Expected to find component of type " + typeof(T) + " but found none!", obj);
			}
            return component;
        }

        #endregion
    }
}
