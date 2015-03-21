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

		[SerializeField]
		private float updateInterval = 0.5f;

		private float accum = 0.0f; // FPS accumulated over the interval

		private float frames = 0f; // Frames drawn over the interval

		private float timeleft; // Left time for current interval

		private GUIText display;

		private bool controllerCheck;

		void Start () {
			display = GetComponent<GUIText>();
			controllerCheck = GameController.Instance != null;
		}

		void Update () {
			if (controllerCheck) {
				DisplayFPS(GameController.FPS);
			} else {
				float dt = Time.unscaledDeltaTime;
				timeleft -= dt ;
				accum += dt;
				++frames;
				
				// Interval ended - update GUI text and start new interval
				if( timeleft <= 0.0 ) {
					// display two fractional digits (f2 format)
					DisplayFPS(frames / accum);
					timeleft = updateInterval;
					accum = 0.0f;
					frames = 0f;
				}
			}
		}

		private void DisplayFPS(float fps) {
			display.text = fps.ToString("f2") + " fps";
		}
	}
}
