// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System;

namespace DanmakU {

	public static class Vector3Extensions {
		
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
	}

}
