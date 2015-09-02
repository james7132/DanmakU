// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Hourai.DanmakU {

    /// <summary>
    /// Class with util extension methods.
    /// The class is organized for easy reading.
    /// #region alphabetic order
    /// inside the region, method alphabetic order
    /// </summary>
    public static class UtilExtensions {
        #region ICollection

        public static T Random<T>(this ICollection<T> collection) {
            int count = collection.Count;
            int selected = UnityEngine.Random.Range(0, count);
            var list = collection as IList<T>;
            if (list != null)
                return list[selected];
            int current = 0;
            IEnumerator<T> iterator = collection.GetEnumerator();
            iterator.MoveNext();
            while (current < selected) {
                iterator.MoveNext();
                current++;
            }
            return iterator.Current;
        }

        #endregion

        #region Enumerated Unity Objects

        public static void Destroy<T>(this IEnumerable<T> set, Func<T, bool> filter = null, float t = 0f)
            where T : UnityObject {
            if(set == null)
                throw new ArgumentNullException("set");
            foreach(T obj in set.Where(o => o && (filter == null || filter(o))))
                UnityObject.Destroy(obj, t);
        }

        #endregion

        #region GameObject 

        public static IEnumerable<GameObject> Children(this GameObject gameObject, Func<GameObject, bool> filter = null) {
            if(!gameObject)
                throw new ArgumentNullException("gameObject");
            foreach (Transform child in gameObject.transform) {
                if(!child)
                    continue;
                GameObject childGO = child.gameObject;
                if (filter == null || filter(childGO))
                    yield return childGO;
            }
        }

        public static IEnumerable<GameObject> Descendants(this GameObject gameObject,
                                                          Func<GameObject, bool> filter = null) {
            if(!gameObject)
                throw new ArgumentNullException("gameObject");
            Stack<IEnumerator> iterators = new Stack<IEnumerator>();
            iterators.Push(gameObject.transform.GetEnumerator());
            while (iterators.Count > 0) {
                IEnumerator iterator = iterators.Peek();
                while (iterator.MoveNext()) {
                    var trans = iterator.Current as Transform;
                    if (!trans)
                        continue;
                    if (trans.childCount > 0) {
                        iterator = trans.GetEnumerator();
                        iterators.Push(iterator);
                    }
                    GameObject go = trans.gameObject;
                    if (filter == null || filter(go))
                        yield return go;
                }
                iterators.Pop();
            }
        }

        public static T FindClosest<T>(this GameObject gameObject) where T : Component
        {
            if (!gameObject)
                throw new ArgumentNullException("gameObject");
            return Util.FindClosest<T>(gameObject.transform.position);
        }

        #endregion

        #region Gradient

        /// <summary>
        /// Randomly chooses a color from a Gradient.
        /// </summary>
        /// <param name="gradient">the gradient to sample from</param>
        /// <exception cref="NullReferenceException">throw if the <paramref name="gradient"/> is null</exception>
        /// <returns>a random color from the gradient</returns>
        public static Color Random(this Gradient gradient) {
            if (gradient == null)
                throw new NullReferenceException();
            return gradient.Evaluate(UnityEngine.Random.value);
        }

        #endregion

        #region Quaternion

        /// <summary>
        /// Gets the 2D rotation value of a Quaternion. (Rotation along the Z axis)
        /// </summary>
        /// <returns>The rotation along the Z axis.</returns>
        /// <param name="rotation">Rotation.</param>
        public static float Rotation2D(this Quaternion rotation) {
            return rotation.eulerAngles.z;
        }

        #endregion

        #region Transform

        /// <summary>
        /// Gets the 2D rotation value of a Transform. (Rotation along the Z axis)
        /// </summary>
        /// <returns>The rotation along the Z axis.</returns>
        /// <exception cref="System.NullReferenceException">Thrown if the given transform is null.</exception>
        /// <param name="transform">The Transform to evaluate.</param>
        public static float Rotation2D(this Transform transform) {
            if (transform == null)
                throw new NullReferenceException();
            return transform.eulerAngles.z;
        }

        public static T FindCloset<T>(this Transform transform) where T : Component
        {
            if (transform == null)
                throw new ArgumentNullException("transform");
            return Util.FindClosest<T>(transform.position);
        }

        #endregion

        #region Rect

        public static Vector2 RandomPoint(this Rect rect) {
            return new Vector2(rect.x + UnityEngine.Random.value*rect.width,
                               rect.y + UnityEngine.Random.value*rect.height);
        }

        #endregion

        #region Bounds 

        public static Vector3 RandomPoint(this Bounds bounds) {
            Vector3 min = bounds.min;
            Vector3 size = bounds.size;
            return new Vector3(min.x + UnityEngine.Random.value*size.x,
                               min.y + UnityEngine.Random.value*size.y,
                               min.z + UnityEngine.Random.value*size.z);
        }

        #endregion
    }

}