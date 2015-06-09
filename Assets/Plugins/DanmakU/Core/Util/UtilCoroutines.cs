// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

    /// <summary>
    /// A static class of utility functions for coroutines.
    /// </summary>
    public class UtilCoroutines {

        private static NullBehaviour utilBehavior;

        internal static NullBehaviour UtilityBehaviour {
            get {
                if (utilBehavior == null) {
                    GameObject temp = new GameObject();
                    utilBehavior = temp.AddComponent<NullBehaviour>();
                    temp.hideFlags = HideFlags.HideInHierarchy;
                }
                return utilBehavior;
            }
        }

        /// <summary>
        /// Coroutine that move an object with lerp, but match exactly the time configured.
        /// PS: Some other coroutines can be created with the same logic, to match the time configured. Like scale coroutine, etc..
        /// Use example(inside some MonoBehavior):
        /// StartCoroutine(UtilCoroutines.MoveToPoint(transform, Vector3.up*10, 5f));
        /// </summary>
        /// <param name="transform">The object's transform. This will be moved.</param>
        /// <param name="to">The destination vector.</param>
        /// <param name="time">The time in seconds.</param>
        /// <returns>IEnumerator</returns>
        public static IEnumerator MoveToPoint(Transform transform,
                                              Vector3 to,
                                              float time) {
            float i = 0.0f;
            float rate = 1.0f/time;
            Vector3 start = transform.position;
            Vector3 end = to;

            while (i < 1.0f) {
                i += Time.deltaTime*rate;
                transform.position = Vector3.Lerp(start, end, i);
                yield return null;
            }
        }

        internal class NullBehaviour : MonoBehaviour {

            private HashSet<Task> latentTasks;

            private void Awake() {
                latentTasks = new HashSet<Task>();
                DontDestroyOnLoad(this);
                StartCoroutine(StartTasks());
            }

            public void QueueTask(Task task) {
                latentTasks.Add(task);
            }

            private IEnumerator StartTasks() {
                while (true) {
                    if (latentTasks.Count > 0) {
                        foreach (var task in latentTasks) {
                            if (!task.started)
                                task.Start();
                        }
                        latentTasks.Clear();
                    }
                    yield return new WaitForEndOfFrame();
                }
            }

        }

    }

}