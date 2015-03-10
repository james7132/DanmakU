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
    public class UtilCoroutines {

		private class NullBehavior : MonoBehaviour {
		}

		private static WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
		private static MonoBehaviour utilBehavior;
		private static MonoBehaviour UtilityBehaviour {
			get {
				if(utilBehavior == null) {
					GameObject temp = new GameObject();
					utilBehavior = temp.AddComponent<NullBehavior>();
					temp.hideFlags = HideFlags.HideInHierarchy;
				}
				return utilBehavior;
			}
		}

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

		/// <summary>
		/// A useful utility function for Coroutines in subclasses of PausableObject
		/// If the instance is not paused, it will wait return a WaitForEndOfFrame instance.
		/// If the instance is paused, it will wait until the object becomes AbstractProjectileControllerd before continuing.
		/// 
		/// <example>
		/// This is standard usage for this function:
		/// <code>
		/// yield return AbstractProjectileController()
		/// </code>
		/// </example>
		/// <see href="http://docs.unity3d.com/Manual/Coroutines.html">Unity Manual: Coroutines</see>
		/// </summary>
		/// <returns> The approriate YieldInstruction for the situation.</returns>
		public static YieldInstruction AbstractProjectileController(IPausable pausableObject) {
			if(pausableObject.Paused)
				return UtilityBehaviour.StartCoroutine (AbstractProjectileControllerWait (pausableObject));
			else
				return wfeof;
		}

		/// <summary>
		/// Coroutine to wait for the object to become AbstractProjectileControllerd
		/// <see href="http://docs.unity3d.com/Manual/Coroutines.html">Unity Manual: Coroutines</see>
		/// </summary>
		/// <returns> The Coroutine IEnumerator </returns>
		private static IEnumerator AbstractProjectileControllerWait(IPausable pausable) {
			while(pausable.Paused)
				yield return wfeof;
		}
    }
}
