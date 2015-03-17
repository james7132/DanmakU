using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtilLib {
    /// <summary>
    /// Class with util extension methods.
    /// The class is organized for easy reading.
    /// #region alphabetic order
    /// inside the region, method alphabetic order
    /// </summary>
    public static class UtilExtensionMethods {
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

		#region Vector2

		public static float ManhattanMagnitude(this Vector2 v) {
			float dist = 0f;
			if (v.x > 0)
				dist += v.x;
			else
				dist += -v.x;

			if (v.y > 0)
				dist += v.y;
			else
				dist += -v.y;
			return dist;
		}

		public static float FastApproximateMagnitude(this Vector2 v) {
			float dx, dy, min, max;
			dx = v.x;
			dy = v.y;
			
			if ( dx < 0 ) dx = -dx;
			if ( dy < 0 ) dy = -dy;
			
			if ( dx < dy ) {
				min = dx;
				max = dy;
			} else {
				min = dy;
				max = dx;
			}
			
			float approx = ( max * 1007 ) + ( min * 441 );
			if ( max < ( min * 16 ))
				approx -= ( max * 40 );
			
			// add 512 for proper rounding
			return (( approx + 512 ) / 1024 );
		}

		#endregion

		#region HashSet

		public static T[] ToArray<T>(this HashSet<T> hashSet, int size = -1) {
			if (size < 0)
				size = hashSet.Count;
			T[] temp = new T[size];
			hashSet.CopyTo (temp);
			return temp;
		}

		#endregion
    }
}
