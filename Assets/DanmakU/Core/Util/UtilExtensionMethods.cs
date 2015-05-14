// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DanmakU {

    /// <summary>
    /// Class with util extension methods.
    /// The class is organized for easy reading.
    /// #region alphabetic order
    /// inside the region, method alphabetic order
    /// </summary>
    public static class UtilExtensionMethods {

		#region IList

		public static T Random<T>(this IList<T> list) {
			int count = list.Count;
			return list [UnityEngine.Random.Range (0, count)];
		}

		#endregion

		#region MonoBehaviour

		public static Task StartTask(this MonoBehaviour behaviour, IEnumerator task) {
			if (task == null)
				throw new System.ArgumentNullException ("Cannot start a null Task");
			return new Task(behaviour, task);
		}

		public static Task StartTask(this MonoBehaviour behaviour, IEnumerable task) {
			if (task == null)
				throw new System.ArgumentNullException ("Cannot start a null Task");
			return new Task(behaviour, task);
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

		public static GameObject[] GetChildren(this GameObject go) {
			return go.GetChildrenList ().ToArray ();
		}

		public static GameObject FindChild(this GameObject go, string childName) {
			GameObject[] children = go.GetChildren ();
			for(int i = 0; i < children.Length; i++) {
				if(children[i].name == childName) {
					return children[i];
				}
			}
			return null;
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

		#region Quaternion
		
		/// <summary>
		/// Gets the 2D rotation value of a Quaternion. (Rotation along the Z axis)
		/// </summary>
		/// <returns>The rotation along the Z axis.</returns>
		/// <param name="rotation">Rotation.</param>
		public static float Rotation2D (this Quaternion rotation) {
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
		public static float Rotation2D (this Transform transform) {
			if (transform == null)
				throw new System.NullReferenceException ();
			return transform.eulerAngles.z;
		}
		
		#endregion

		#region Vector2

		public static float ManhattanMagnitude(this Vector2 v) {
			float dist = 0f;
			if (v.x > 0)
				dist += v.x;
			else
				dist -= +v.x;

			if (v.y > 0)
				dist += v.y;
			else
				dist -= v.y;
			return dist;
		}

		public static float Cross(this Vector2 v1, Vector2 v2) {
			return v1.x * v2.y - v1.y * v2.x;
		}

		/// <summary>
		/// Computes the <see href="http://en.wikipedia.org/wiki/Hadamard_product_%28matrices%29">Hadamard Product</see> between two Vector2s
		/// </summary>
		/// <returns>The Hadamard product between the two vectors.</returns>
		/// <param name="v1">the first vector</param>
		/// <param name="v2">the second vector</param>
		public static Vector2 Hadamard2(this Vector2 v1, Vector2 v2) {
			return new Vector2(v1.x * v2.x, v1.y * v2.y);
		}
		
		/// <summary>
		/// Finds the largest component in the given Vector2
		/// </summary>
		/// <returns> the value of the smallest component</returns>
		/// <param name="v">the vector to evaluate</param>
		public static float Max(this Vector2 v) {
			return (v.x > v.y) ? v.x : v.y;
		}
		
		/// <summary>
		/// Finds the smallest component in the given Vector2
		/// </summary>
		/// <returns> the value of the smallest component</returns>
		/// <param name="v">the vector to evaluate</param>
		public static float Min(this Vector2 v) {
			return (v.x < v.y) ? v.x : v.y;
		}

		/// <summary>
		/// Creates a random Vector2 between (0,0) and the given vector's components.
		/// </summary>
		/// <returns>the random vector</returns>
		/// <param name="v">the maximum component values</param>
		public static Vector2 Random(this Vector2 v) {
			return new Vector2 (UnityEngine.Random.value * v.x, UnityEngine.Random.value * v.y);
		}
		
		public static Vector2 Abs(this Vector2 v) {
			return new Vector2 (Math.Abs (v.x), Math.Abs (v.y));
		}

//		public static float FastApproximateMagnitude(this Vector2 v) {
//			float dx, dy, min, max;
//			dx = v.x;
//			dy = v.y;
//			
//			if ( dx < 0 ) dx = -dx;
//			if ( dy < 0 ) dy = -dy;
//			
//			if ( dx < dy ) {
//				min = dx;
//				max = dy;
//			} else {
//				min = dy;
//				max = dx;
//			}
//			
//			float approx = ( max * 1007 ) + ( min * 441 );
//			if ( max < ( min * 16 ))
//				approx -= ( max * 40 );
//			
//			// add 512 for proper rounding
//			return (( approx + 512 ) / 1024 );
//		}

		#endregion

		#region Vector3

		public static float ManhattanMagnitude(this Vector3 v) {
			float dist = 0f;
			if (v.x > 0)
				dist += v.x;
			else
				dist -= v.x;
			
			if (v.y > 0)
				dist += v.y;
			else
				dist -= v.y;

			if (v.z > 0)
				dist += v.z;
			else 
				dist -= v.z;

			return dist;
		}

		/// <summary>
		/// Creates a random Vector3 between (0,0) and the given vector's components.
		/// </summary>
		/// <returns>the random vector</returns>
		/// <param name="v">the maximum component values</param>
		public static Vector3 Random(this Vector3 v) {
			return new Vector3 (UnityEngine.Random.value * v.x, UnityEngine.Random.value * v.y, UnityEngine.Random.value * v.z);
		}

		/// <summary>
		/// Computes the <see href="http://en.wikipedia.org/wiki/Hadamard_product_%28matrices%29">Hadamard Product</see> between two Vector3s
		/// </summary>
		/// <returns>The Hadamard product between the two vectors.</returns>
		/// <param name="v1">the first vector</param>
		/// <param name="v2">the second vector</param>
		public static Vector3 Hadamard3(this Vector3 v1, Vector3 v2) {
			return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
		}

		/// <summary>
		/// Finds the largest component in the given Vector3
		/// </summary>
		/// <returns> the value of the smallest component</returns>
		/// <param name="v">the vector to evaluate</param>
		public static float Max(this Vector3 v) {
			if(v.x > v.y)
				return (v.z > v.y) ? v.z : v.y;
			else
				return (v.z > v.x) ? v.z : v.x;
		}
		
		/// <summary>
		/// Finds the smallest component in the given Vector3
		/// </summary>
		/// <returns> the value of the smallest component</returns>
		/// <param name="v">the vector to evaluate</param>
		public static float Min(this Vector3 v) {
			if(v.x < v.y)
				return (v.z < v.y) ? v.z : v.y;
			else
				return (v.z < v.x) ? v.z : v.x;
		}

		#endregion

		#region Vector4

		public static float ManhattanMagnitude(this Vector4 v) {
			float dist = 0f;
			if (v.x > 0)
				dist += v.x;
			else
				dist -= v.x;
			
			if (v.y > 0)
				dist += v.y;
			else
				dist -= v.y;
			
			if (v.z > 0)
				dist += v.z;
			else 
				dist -= v.z;

			if (v.w > 0)
				dist += v.w;
			else
				dist -= v.w;
			return dist;
		}

		/// <summary>
		/// Computes the <see href="http://en.wikipedia.org/wiki/Hadamard_product_%28matrices%29">Hadamard Product</see> between two Vector4s
		/// </summary>
		/// <returns>The Hadamard product between the two vectors.</returns>
		/// <param name="v1">the first vector</param>
		/// <param name="v2">the second vector</param>
		public static Vector4 Hadamard4(this Vector4 v1, Vector4 v2) {
			return new Vector4(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z, v1.w * v2.w);
		}
		
		/// <summary>
		/// Creates a random Vector4 between (0,0) and the given vector's components.
		/// </summary>
		/// <returns>the random vector</returns>
		/// <param name="v">the maximum component values</param>
		public static Vector4 Random(this Vector4 v) {
			return new Vector4 (UnityEngine.Random.value * v.x, UnityEngine.Random.value * v.y, UnityEngine.Random.value * v.z, UnityEngine.Random.value * v.y);
		}

		public static float Max(this Vector4 v) {
			if (v.x > v.y) {
				if (v.z > v.w) {
					return (v.x > v.z) ? v.x : v.z;
				} else {
					return (v.x > v.w) ? v.x : v.w;
				}
			} else {
				if (v.z > v.w) {
					return (v.y > v.z) ? v.y : v.z;
				} else {
					return (v.y > v.w) ? v.y : v.w;
				}
			}
		}

		public static float Min(this Vector4 v) {
			if (v.x < v.y) {
				if (v.z < v.w) {
					return (v.x < v.z) ? v.x : v.z;
				} else {
					return (v.x < v.w) ? v.x : v.w;
				}
			} else {
				if (v.z < v.w) {
					return (v.y < v.z) ? v.y : v.z;
				} else {
					return (v.y < v.w) ? v.y : v.w;
				}
			}
		}

		#endregion

		#region Rect

		public static Vector2 RandomPoint(this Rect rect) {
			return new Vector2 (rect.x + UnityEngine.Random.value * rect.width, rect.y +  UnityEngine.Random.value * rect.height);
		}

		#endregion

		#region Bounds 

		public static Vector3 RandomPoint(this Bounds bounds) {
			Vector3 min = bounds.min;
			Vector3 size = bounds.size;
			return new Vector3 (min.x + UnityEngine.Random.value * size.x,
			                   min.y + UnityEngine.Random.value * size.y,
			                   min.z + UnityEngine.Random.value * size.z);
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
