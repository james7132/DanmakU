// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU {
	
	public static class Vector4Extensions {
		
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
	}
}