// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU {

	public static class TimeUtil {

		static TimeUtil() {
			normalFPS = 60f;
			normalDeltaTime = 1f / normalFPS;
		}

		private static float normalFPS;
		private static float normalDeltaTime;


		public static bool FrameRateIndependent = true;
		
		/// <summary>
		/// The normal target frames per second
		/// This is the value used by <see cref="TargetFPS"/> if Time.timeScale is not 0 but Application.targetFrameRate is 0. 
		/// </summary>
		public static float NormalFPS {
			get {
				return normalFPS;
			}
			set {
				if (value == 0f || float.IsNaN(value)) {
					normalFPS = 0f;
					normalDeltaTime = float.PositiveInfinity;
				} else if(float.IsInfinity(value)) { 
					normalFPS = float.PositiveInfinity;
					normalDeltaTime = 0f;
				} else {
					normalFPS = value;
					normalDeltaTime = 1f / value;
				}
			}
		}
		
		public static float NormalDeltaTime {
			get {
				return normalDeltaTime;
			}
			set {
				if(value == 0f || float.IsNaN(value)) {
					normalDeltaTime = 0f;
					normalFPS = float.PositiveInfinity;
				} else if(float.IsInfinity(value)) { 
					normalDeltaTime = float.PositiveInfinity;
					normalFPS = 0f;
				} else {
					normalDeltaTime = value;
					normalFPS = 1f / value;
				}
			}
		}
		
		/// <summary>
		/// Gets the expected frames per second of the current Application.
		/// Normally this is set to Application.targetFrameRate if it is not 0.
		/// If the game is paused via setting Time.timeScale to 0, this evaluates to Infinity
		/// </summary>
		/// <value>The expected frames per second.</value>
		public static float FPS {
			get {
				if (Mathf.Abs (Time.timeScale - 0) > float.Epsilon) {
					if (FrameRateIndependent)
						return 1f / Time.deltaTime;
					else
						return (Application.targetFrameRate > 0f) ? Application.targetFrameRate : normalFPS;
				} else
					return float.PositiveInfinity;
			}
		}
		
		/// <summary>
		/// Gets the expected time between each frame for the current Application.
		/// It is equal to the inverse of <see cref="TargetFPS"/>
		/// TargetDeltaTime = 1/TargetFPS
		/// </summary>
		/// <value>The expected delta time.</value>
		public static float DeltaTime {
			get {
				if(FrameRateIndependent) {
					return Time.deltaTime;
				} else {
					if(Mathf.Abs (Time.timeScale - 0) > float.Epsilon)
								return Time.timeScale * ((Application.targetFrameRate <= 0f) ? normalDeltaTime : 1f / Application.targetFrameRate);
					else
						return 0f;
				}
			}
		}
		
		/// <summary>
		/// Converts floating point time to an integer number of frames based on TargetDeltaTime/TargetFPS.
		/// Useful in converting a fixed time to a count for frames.
		/// </summary>
		/// <returns>the time elapsed in the given frames</returns>
		/// <param name="time">the elapsed time to convert to frames</param>
		public static int TimeToFrames(float time) {
			return Mathf.CeilToInt (time * FPS);
		}
		
		/// <summary>
		/// Converts floating point time to an integer number of frames based on TargetDeltaTime/TargetFPS.
		/// Useful in converting a fixed time to a count for frames.
		/// </summary>
		/// <returns>the time elapsed in the given frames</returns>
		/// <param name="time">the elapsed time to convert to frames</param>
		public static float FramesToTime(int frames) {
			return (float)frames * DeltaTime;
		}
	}

}
