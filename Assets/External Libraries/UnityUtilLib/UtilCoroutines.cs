using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtilLib
{
    /// <summary>
    /// Class with util coroutines.
    /// This helps to reuse the coroutines.
    /// Use example(inside some MonoBehavior):
    /// StartCoroutine(UtilCoroutines.C_MoveToPoint(transform, Vector3.up*10, 5f));
    /// </summary>
    public class UtilCoroutines
    {
        /// <summary>
        /// Coroutine that move an object with lerp, but match exactly the time configured.
        /// PS: Some other coroutines can be created with the same logic, to match the time configured. Like scale coroutine, etc..
        /// Use example(inside some MonoBehavior):
        /// StartCoroutine(UtilCoroutines.C_MoveToPoint(transform, Vector3.up*10, 5f));
        /// </summary>
        /// <param name="transform">The object's transform. This will be moved.</param>
        /// <param name="to">The destination vector.</param>
        /// <param name="time">The time in seconds.</param>
        /// <returns>IEnumerator</returns>
        public static IEnumerator C_MoveToPoint(Transform transform, Vector3 to, float time)
        {
            float i = 0.0f;
            float rate = 1.0f / time;
            Vector3 start = transform.position;
            Vector3 end = to;

            while (i < 1.0f)
            {
                i += Time.deltaTime * rate;
                transform.position = Vector3.Lerp(start, end, i);
                yield return null;
            }

        }
    }
}
