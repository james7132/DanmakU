using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// A utilty library of random useful and portable scripts for Unity
/// </summary>
namespace UnityUtilLib {

	/// <summary>
	/// A static utility class of random functions and constants that are useful in various Unity projects
	/// </summary>
	public static class Util {

		/// <summary>
		/// A constant used to convert degrees to radians
		/// Multiply a degree measure by this to convert.
		/// </summary>
		public const float Degree2Rad = Mathf.PI / 180f;

		/// <summary>
		/// A constant used to convert radians to degrees
		/// Multiply a radian measure by this to convert.
		/// </summary>
		public const float Rad2Degree = 180f / Mathf.PI;

		public const float TwoPI = 2 * Mathf.PI;

		/// <summary>
		/// The normal target frames per second
		/// This is the value used by <see cref="TargetFPS"/> if Time.timeScale is not 0 but Application.targetFrameRate is 0. 
		/// </summary>
		public static float NormalTargetFPS = 60f;

		/// <summary>
		/// Gets the expected frames per second of the current Application.
		/// Normally this is set to Application.targetFrameRate if it is not 0.
		/// If the game is paused via setting Time.timeScale to 0, this evaluates to Infinity
		/// </summary>
		/// <value>The expected frames per second.</value>
		public static float TargetFPS {
			get {
				if(Time.timeScale != 0)
					return (Application.targetFrameRate > 0f) ? Application.targetFrameRate : NormalTargetFPS;
				else
					return float.PositiveInfinity;
			}
		}

		/// <summary>
		/// Gets the expected time between each frame for the current Application.
		/// It is equal to the inverse of <see cref="TargetFPS"/>
		/// TargetDeltaTime = 1/TargetFPS
		/// </summary>
		/// <value>The expected delta time.</value>
		public static float TargetDeltaTime {
			get {
				return 1f / TargetFPS;
			}
		}

		/// <summary>
		/// Converts floating point time to an integer number of frames based on TargetDeltaTime/TargetFPS.
		/// Useful in converting a fixed time to a count for frames.
		/// </summary>
		/// <returns>the time elapsed in the given frames</returns>
		/// <param name="time">the elapsed time to convert to frames</param>
		public static int TimeToFrames(float time) {
			return Mathf.CeilToInt (time * TargetFPS);
		}

		/// <summary>
		/// Converts floating point time to an integer number of frames based on TargetDeltaTime/TargetFPS.
		/// Useful in converting a fixed time to a count for frames.
		/// </summary>
		/// <returns>the time elapsed in the given frames</returns>
		/// <param name="time">the elapsed time to convert to frames</param>
		public static float FramesToTime(int frames) {
			return (float)frames * TargetDeltaTime;
		}

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

		public static T[] GetComponents<T> (GameObject gameObject) where T : class {
			Component[] components = gameObject.GetComponents (typeof(T));
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
			return new Vector2 ((float)Math.Abs (v.x), (float)Math.Abs (v.y));
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
			float radians = Degree2Rad * degrees;
			return new Vector2 ((float)System.Math.Cos(radians), (float)System.Math.Sin(radians));
		}

		public static Vector2 OnUnitCircleRadians(float radians) {
			return new Vector2 ((float)Math.Cos(radians), (float)System.Math.Sin(radians));
		}
		
		public static float AngleBetween2D(Vector2 v1, Vector2 v2) {
			Vector2 diff = v2 - v1;
			return Mathf.Atan2 (diff.y, diff.x) * 180f / Mathf.PI - 90f; 
		}
		
		public static Quaternion RotationBetween2D(Vector2 v1, Vector2 v2) {
			return Quaternion.Euler (0f, 0f, AngleBetween2D (v1, v2));
		}
	}
}