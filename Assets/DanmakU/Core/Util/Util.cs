// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System;
using System.Collections.Generic;

namespace DanmakU {

	/// <summary>
	/// A static utility class of random functions and constants that are useful in various Unity projects
	/// </summary>
	public static class Util {

		public const float TwoPI = 2 * Mathf.PI;

		/// <summary>
		/// Creates an array of masks for collisions/raycasts in 2D physics
		/// Useful for mirroring collision behavior.
		/// </summary>
		/// <returns>the masks for each layer</returns>
		public static int[] CollisionLayers2D() {
			int[] collisionMask = new int[32];
			for(int i = 0; i < 32; i++) {
				collisionMask[i] = 0;
				for (int j = 0; j < 32; j++) {
					collisionMask[i] |= (Physics2D.GetIgnoreLayerCollision(i, j)) ? 0 : (1 << j);
				}
			}
			return collisionMask;
		}
		
		/// <summary>
		/// Creates an array of masks for collisions/raycasts in 3D physics
		/// Useful for mirroring collision behavior.
		/// </summary>
		/// <returns>the masks for each layer</returns>
		public static int[] CollisionLayers3D() {
			int[] collisionMask = new int[32];
			for(int i = 0; i < 32; i++) {
				collisionMask[i] = 0;
				for (int j = 0; j < 32; j++) {
					collisionMask[i] |= (Physics.GetIgnoreLayerCollision(i, j)) ? 0 : (1 << j);
				}
			}
			return collisionMask;
		}
	
		/// <summary>
		/// Actually computes the sign of a floating point number
		///  * If it is less than 0: returns -1
		///  * If it is equal to 0: returns 0
		///  * If it is more than 0: returns 1
		/// </summary>
		/// <param name="e">the sign of the given floating point value</param>
		public static float Sign(float e) {
			return (e == 0f) ? 0f : Mathf.Sign (e);
		}

		public static Vector3 BerzierCurveVectorLerp(Vector3 start, Vector3 end, Vector3 c1, Vector3 c2, float t) {
			float u, uu, uuu, tt, ttt;
			Vector3 p, p0 = start, p1 = c1, p2 = c2, p3 = end;
			u = 1 - t;
			uu = u*u;
			uuu = uu * u;
			tt = t * t;
			ttt = tt * t;
			
			p = uuu * p0; //first term
			p += 3 * uu * t * p1; //second term
			p += 3 * u * tt * p2; //third term
			p += ttt * p3; //fourth term

			return p;
		}

		public static T GetComponent<T>(GameObject gameObject) where T : class {
			return gameObject.GetComponent (typeof(T)) as T;
		}

		public static T[] GetComponents<T> (GameObject gameObject) where T : class {
			Component[] components = gameObject.GetComponents (typeof(T));
			int num = components.Length;
			T[] temp = new T[num];
			for(int i = 0; i < num; i++) {
				temp[i] = components[i] as T;
			}
			return temp;
		}

		public static T[] GetComponents<T>(Component component) where T : class {
			Component[] components = component.GetComponents (typeof(T));
			int num = components.Length;
			T[] temp = new T[num];
			for(int i = 0; i < num; i++) {
				temp[i] = components[i] as T;
			}
			return temp;
		}

		public static T GetComponentInChildren<T>(GameObject gameObject) where T : class {
			return gameObject.GetComponentInChildren (typeof(T)) as T;
		}

		public static T[] GetComponentsInChildren<T>(GameObject gameObject) where T : class {
			Component[] components = gameObject.GetComponentsInChildren (typeof(T));
			int num = components.Length;
			T[] temp = new T[num];
			for(int i = 0; i < num; i++) {
				temp[i] = components[i] as T;
			}
			return temp;
		}

		public static T GetComponentInParent<T>(GameObject gameObject) where T : class {
			return gameObject.GetComponentInParent (typeof(T)) as T;
		}
		
		public static T[] GetComponentsInParent<T>(GameObject gameObject) where T : class {
			Component[] components = gameObject.GetComponentsInParent (typeof(T));
			int num = components.Length;
			T[] temp = new T[num];
			for(int i = 0; i < num; i++) {
				temp[i] = components[i] as T;
			}
			return temp;
		}

		public static T[] GetComponentsPrealloc<T>(GameObject gameObject, T[] prealloc, out int count) where T : class {
			Component[] components = gameObject.GetComponents (typeof(T));
			count = components.Length;
			if (prealloc.Length < count) {
				Debug.Log(count);
				prealloc = new T[count];
			}
			for(int i = 0; i < count; i++) {
				prealloc[i] = components[i] as T;
			}
			return prealloc;
		}

		/// <summary>
		/// Finds the <a href="http://docs.unity3d.com/ScriptReference/Object.html">UnityEngine.Object</a> that derive from a certain type.
		/// Unlike <a href="http://docs.unity3d.com/ScriptReference/Object.FindObjectsOfType.html">UnityEngine.Object.FindObjectsOfType</a>, this method works on interface types as well.
		/// This method is for general search. For a more efficent search that only works on classes derived from <a href="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html">MonoBehavior</a> 
		/// use FindBehaviorsOfType instead.
		/// </summary>
		/// <returns>The objects of type T.</returns>
		/// <typeparam name="T">the type to search for</typeparam>
		public static T[] FindObjectsOfType<T>() where T : class  {
			return FindObjectByType<T, UnityEngine.Object> ();
		}

		/// <summary>
		/// Finds the <a href="http://docs.unity3d.com/ScriptReference/Object.html">UnityEngine.Object</a> that derive from a certain type.
		/// Unlike <a href="http://docs.unity3d.com/ScriptReference/Object.FindObjectsOfType.html">UnityEngine.Object.FindObjectsOfType</a>, this method works on interface types as well.
		/// This method is for specific search on classes derived from <a href="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html">MonoBehavior</a>.
		/// For a more general search over all objects, use FindObjectsOfType instead.
		/// </summary>
		/// <returns>The objects of type T.</returns>
		/// <typeparam name="T">the type to search for</typeparam>
		public static T[] FindBehaviorsOfType<T> () where T : class  {
			return FindObjectByType<T, MonoBehaviour> ();
		}

		private static T[] FindObjectByType<T, V> () where T : class where V : UnityEngine.Object {
			UnityEngine.Object[] objects = UnityEngine.Object.FindObjectsOfType<V> ();
			List<T> matches = new List<T> ();
			for(int i = 0; i < objects.Length; i++) {
				if(objects[i] is T) {
					matches.Add (objects[i] as T);
				}
			}
			return matches.ToArray ();
		}

		public static Vector2 Abs(Vector2 v) {
			return new Vector2 (Math.Abs (v.x), Math.Abs (v.y));
		}

		/// <summary>
		/// Finds the closest described component to the given point
		/// </summary>
		/// <returns>The closest instance of the given Component type</returns>
		/// <param name="position">The closest instance of T to the given point</param>
		/// <typeparam name="T">The Component Type to search for</typeparam>
		public static T FindClosest<T>(Vector3 position) where T : Component {
			T returnValue = default(T);
			T[] objects = UnityEngine.Object.FindObjectsOfType<T> ();
			float minDist = float.MaxValue;
			for (int i = 0; i < objects.Length; i++) {
				float dist = (objects[i].transform.position - position).magnitude;
				if(dist < minDist) {
					returnValue = objects[i];
					minDist = dist;
				}
			}
			return returnValue;
		}

		public static Vector2 OnUnitCircle(float degrees) {
			float radians = Mathf.Deg2Rad * degrees;
			return new Vector2 ((float)System.Math.Cos(radians), (float)System.Math.Sin(radians));
		}

		public static Vector2 OnUnitCircleRadians(float radians) {
			return new Vector2 ((float)Math.Cos(radians), (float)System.Math.Sin(radians));
		}
	}
}