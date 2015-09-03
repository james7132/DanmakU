// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using UnityEngine;

namespace Hourai {

    public static class Vector2Extensions {

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

        /// <summary>
        /// Computes the 2D cross product (the magnitude of the 3D cross product result).
        /// </summary>
        /// <param name="v1">the first vector.</param>
        /// <param name="v2">the second vector.</param>
        public static float Cross(this Vector2 v1, Vector2 v2) {
            return v1.x*v2.y - v1.y*v2.x;
        }

        /// <summary>
        /// Computes the <see href="http://en.wikipedia.org/wiki/Hadamard_product_%28matrices%29">Hadamard Product</see> between two Vector2s
        /// </summary>
        /// <returns>The Hadamard product between the two vectors.</returns>
        /// <param name="v1">the first vector</param>
        /// <param name="v2">the second vector</param>
        public static Vector2 Hadamard2(this Vector2 v1, Vector2 v2) {
            return new Vector2(v1.x*v2.x, v1.y*v2.y);
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
            return new Vector2(UnityEngine.Random.value*v.x,
                               UnityEngine.Random.value*v.y);
        }

        public static Vector2 Abs(this Vector2 v) {
            return new Vector2(Math.Abs(v.x), Math.Abs(v.y));
        }

    }

}