using UnityEngine;
using System.Collections;

namespace UnityUtilLib.GUI {

	/// <summary>
	/// FPS counter.
	/// </summary>
	[RequireComponent(typeof(GUIText))]
	public class FPSCounter : MonoBehaviour {

		/// <summary>
		/// The update interval.
		/// </summary>
		private float updateInterval = 0.5f;

		/// <summary>
		/// The accum.
		/// </summary>
		private float accum = 0.0f; // FPS accumulated over the interval

		/// <summary>
		/// The frames.
		/// </summary>
		private float frames = 0f; // Frames drawn over the interval

		/// <summary>
		/// The timeleft.
		/// </summary>
		private float timeleft; // Left time for current interval

		/// <summary>
		/// The display.
		/// </summary>
		private GUIText display;

		/// <summary>
		/// Start this instance.
		/// </summary>
		void Start () {
			display = guiText;
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		void Update () {
			float dt = Time.deltaTime;
			timeleft -= dt ;
			accum += Time.timeScale / dt;
			++frames;
			
			// Interval ended - update GUI text and start new interval
			if( timeleft <= 0.0 )
			{
				// display two fractional digits (f2 format)
				display.text = (accum/frames).ToString("f2") + " fps";
				timeleft = updateInterval;
				accum = 0.0f;
				frames = 0f;
			}
		}
	}
}
