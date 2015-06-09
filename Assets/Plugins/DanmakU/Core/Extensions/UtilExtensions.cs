// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DanmakU
{
    /// <summary>
    /// Class with util extension methods.
    /// The class is organized for easy reading.
    /// #region alphabetic order
    /// inside the region, method alphabetic order
    /// </summary>
    public static class UtilExtensions
    {
        #region ICollection

        public static T Random<T>(this ICollection<T> collection)
        {
            int count = collection.Count;
            int selected = UnityEngine.Random.Range(0, count);
            var list = collection as IList<T>;
            if (list != null)
                return list[selected];
            else
            {
                int current = 0;
                IEnumerator<T> iterator = collection.GetEnumerator();
                iterator.MoveNext();
                while (current < selected)
                {
                    iterator.MoveNext();
                    current++;
                }
                return iterator.Current;
            }
        }

        #endregion

        #region MonoBehaviour

        public static Task StartTask(this MonoBehaviour behaviour, IEnumerator task)
        {
            if (task == null)
                throw new ArgumentNullException("Cannot start a null Task");
            return new Task(behaviour, task);
        }

        public static Task StartTask(this MonoBehaviour behaviour, IEnumerable task)
        {
            if (task == null)
                throw new ArgumentNullException("Cannot start a null Task");
            return new Task(behaviour, task);
        }

        #endregion

        #region Quaternion

        /// <summary>
        /// Gets the 2D rotation value of a Quaternion. (Rotation along the Z axis)
        /// </summary>
        /// <returns>The rotation along the Z axis.</returns>
        /// <param name="rotation">Rotation.</param>
        public static float Rotation2D(this Quaternion rotation)
        {
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
        public static float Rotation2D(this Transform transform)
        {
            if (transform == null)
                throw new System.NullReferenceException();
            return transform.eulerAngles.z;
        }

        #endregion

        #region Rect

        public static Vector2 RandomPoint(this Rect rect)
        {
            return new Vector2(rect.x + UnityEngine.Random.value*rect.width,
                rect.y + UnityEngine.Random.value*rect.height);
        }

        #endregion

        #region Bounds 

        public static Vector3 RandomPoint(this Bounds bounds)
        {
            Vector3 min = bounds.min;
            Vector3 size = bounds.size;
            return new Vector3(min.x + UnityEngine.Random.value*size.x,
                min.y + UnityEngine.Random.value*size.y,
                min.z + UnityEngine.Random.value*size.z);
        }

        #endregion
    }
}