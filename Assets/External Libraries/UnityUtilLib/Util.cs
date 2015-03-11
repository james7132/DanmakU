using UnityEngine;
using System.Collections;

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

		/// <summary>
		/// Creates a random Vector2 between (0,0) and the given vector's components.
		/// </summary>
		/// <returns>the random vector</returns>
		/// <param name="v">the maximum component values</param>
		public static Vector2 RandomVect2(Vector2 v) {
			return new Vector2 (Random.value * v.x, Random.value * v.y);
		}

		/// <summary>
		/// Creates a random Vector3 between (0,0) and the given vector's components.
		/// </summary>
		/// <returns>the random vector</returns>
		/// <param name="v">the maximum component values</param>
		public static Vector3 RandomVect3(Vector3 v) {
			return new Vector3 (Random.value * v.x, Random.value * v.y, Random.value * v.z);
		}

		/// <summary>
		/// Creates a random Vector4 between (0,0) and the given vector's components.
		/// </summary>
		/// <returns>the random vector</returns>
		/// <param name="v">the maximum component values</param>
		public static Vector4 RandomVect4(Vector4 v) {
			return new Vector4 (Random.value * v.x, Random.value * v.y, Random.value * v.z, Random.value * v.y);
		}

		/// <summary>
		/// Computes the <see href="http://en.wikipedia.org/wiki/Hadamard_product_%28matrices%29">Hadamard Product</see> between two Vector2s
		/// </summary>
		/// <returns>The Hadamard product between the two vectors.</returns>
		/// <param name="v1">the first vector</param>
		/// <param name="v2">the second vector</param>
		public static Vector2 HadamardProduct2(Vector2 v1, Vector2 v2) {
			return new Vector2(v1.x * v2.x, v1.y * v2.y);
		}

		/// <summary>
		/// Computes the <see href="http://en.wikipedia.org/wiki/Hadamard_product_%28matrices%29">Hadamard Product</see> between two Vector3s
		/// </summary>
		/// <returns>The Hadamard product between the two vectors.</returns>
		/// <param name="v1">the first vector</param>
		/// <param name="v2">the second vector</param>
		public static Vector3 HadamardProduct3(Vector3 v1, Vector3 v2) {
			return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
		}

		/// <summary>
		/// Computes the <see href="http://en.wikipedia.org/wiki/Hadamard_product_%28matrices%29">Hadamard Product</see> between two Vector4s
		/// </summary>
		/// <returns>The Hadamard product between the two vectors.</returns>
		/// <param name="v1">the first vector</param>
		/// <param name="v2">the second vector</param>
		public static Vector4 HadamardProduct4(Vector4 v1, Vector4 v2) {
			return new Vector4(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z, v1.w * v2.w);
		}
		
		/// <summary>
		/// Finds the largest component in the given Vector2
		/// </summary>
		/// <returns> the value of the smallest component</returns>
		/// <param name="v">the vector to evaluate</param>
		public static float MaxComponent2(Vector2 v) {
			return (v.x > v.y) ? v.x : v.y;
		}
		
		/// <summary>
		/// Finds the largest component in the given Vector3
		/// </summary>
		/// <returns> the value of the smallest component</returns>
		/// <param name="v">the vector to evaluate</param>
		public static float MaxComponent3(Vector3 v) {
			if(v.x > v.y)
				return (v.z > v.y) ? v.z : v.y;
			else
				return (v.z > v.x) ? v.z : v.x;
		}
		
		/// <summary>
		/// Finds the smallest component in the given Vector2
		/// </summary>
		/// <returns> the value of the smallest component</returns>
		/// <param name="v">the vector to evaluate</param>
		public static float MinComponent2(Vector2 v) {
			return (v.x < v.y) ? v.x : v.y;
		}

		/// <summary>
		/// Finds the smallest component in the given Vector3
		/// </summary>
		/// <returns> the value of the smallest component</returns>
		/// <param name="v">the vector to evaluate</param>
		public static float MinComponent3(Vector3 v) {
			if(v.x < v.y)
				return (v.z < v.y) ? v.z : v.y;
			else
				return (v.z < v.x) ? v.z : v.x;
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

		/// <summary>
		/// Finds the closest described component to the given point
		/// </summary>
		/// <returns>The closest instance of the given Component type</returns>
		/// <param name="position">The closest instance of T to the given point</param>
		/// <typeparam name="T">The Component Type to search for</typeparam>
		public static T FindClosest<T>(Vector3 position) where T : Component {
			T returnValue = default(T);
			T[] objects = Object.FindObjectsOfType<T> ();
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

		public static float AngleBetween2D(Vector2 v1, Vector2 v2) {
			Vector2 diff = v2 - v1;
			return Mathf.Atan2 (diff.y, diff.x) * 180f / Mathf.PI - 90f; 
		}

		public static Quaternion RotationBetween2D(Vector2 v1, Vector2 v2) {
			return Quaternion.Euler (0f, 0f, AngleBetween2D (v1, v2));
		}
	}
}