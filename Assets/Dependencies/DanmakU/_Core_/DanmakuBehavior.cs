// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections;
using UnityEngine;
using Vexe.Runtime.Types;

namespace Hourai.DanmakU {

    public abstract class DanmakuBehaviour : HouraiBehaviour {

        /// <summary>
        /// Shorthand for <c>TimeUtil.DeltaTime</c>.
        /// </summary>
        protected static float Dt {
            get { return TimeUtil.DeltaTime; }
        }

		/// <summary>
		/// Calculates the cosine of the 
		/// </summary>
		/// <remarks>
		/// This function has much better performance than Mathf.Cos; however it can have much lower accuracy.
		/// </remarks>
		/// <param name="degree">Degree.</param>
        protected static float Cos(float degree) {
            return FastTrig.Cos(degree);
        }

        protected static float Sin(float degree) {
            return FastTrig.Sin(degree);
        }

        protected static float Tan(float degree) {
            return FastTrig.Tan(degree);
        }

        protected static Vector2 UnitCircle(float degree) {
            return FastTrig.UnitCircle(degree);
        }
    }

}