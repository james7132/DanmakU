using UnityEngine;

/// <summary>
/// A set of small utility GUI scripts that can be easily ported from one game to another
/// </summary>
namespace UnityUtilLib.GUI {

	/// <summary>
	/// An FPS counter displayed using a <a href="http://docs.unity3d.com/ScriptReference/GUIText.html">GUIText</a>.
	/// This is identical in functionality to the script provided in Unity's Standard Assets
	/// </summary>
	[RequireComponent(typeof(GUIText))]
	public class FPSCounter : MonoBehaviour {

		private float updateInterval = 0.5f;

		private float accum = 0.0f; // FPS accumulated over the interval

		private float frames = 0f; // Frames drawn over the interval

		private float timeleft; // Left time for current interval

		private GUIText display;

		void Start () {
			display = GetComponent<GUIText>();
		}

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
