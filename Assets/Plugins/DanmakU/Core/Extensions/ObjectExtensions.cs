// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU
{
    /// <summary>
    /// Extension Methods for all UnityEngine.Object instances.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Destroy the specified object.
        /// </summary>
        /// <remarks>
        /// A shorter way of writing <c>Object.Destroy(unityObject)</c>.
        /// </remarks>
        /// <param name="unityObject">The Unity object to destroy.</param>
        /// <param name="t">The delay in time, in seconds, before destroying the object. Defaults to 0.</param>
        public static void Destroy(this UnityEngine.Object unityObject, float t = 0.0f)
        {
            UnityEngine.Object.Destroy(unityObject, t);
        }

        /// <summary>
        /// Destroy the specified object immediately.
        /// </summary>
        /// <remarks>
        /// A shorter way of writing <c>Object.DestroyImmediate(unityObject)</c>.
        /// </remarks>
        /// <param name="unityObject">The Unity object to destroy.</param>
        public static void DestroyImmediate(this UnityEngine.Object unityObject)
        {
            UnityEngine.Object.DestroyImmediate(unityObject);
        }
    }
}