using UnityEngine;

/// <summary>
/// A utilty library of random useful and portable scripts for Unity
/// </summary>
namespace UnityUtilLib {

	/// <summary>
	/// An abstract God object overseeing the entire game.
	/// Only one can exist at single time.
	/// It is best to subclass this and include overarching expansive actions that affect the entire game.
	/// </summary>
	public abstract class GameController : Singleton<GameController> {

		[SerializeField]
		private float fpsUpdateInterval = 0.5f;
		
		private float accum = 0.0f; // FPS accumulated over the interval
		
		private float frames = 0f; // Frames drawn over the interval
		
		private float timeleft; // Left time for current interval

		private static float fps = float.NaN;

		private static float oldTimeScale;
		private static bool gamePaused;

		/// <summary>
		/// Gets whether game paused.
		/// </summary>
		/// <value><c>true</c> if the game is paused; otherwise, <c>false</c>.</value>
		public static bool IsGamePaused {
			get {
				return gamePaused;
			}
		}

		/// <summary>
		/// Gets the game's FPS.
		/// Will return float.NaN if no GameController Instance does not exist.
		/// </summary>
		/// <value>The FPS.</value>
		public static float FPS {
			get {
				return fps;
			}
		}

		public virtual void Update() {
			float dt = Time.unscaledDeltaTime;
			timeleft -= dt ;
			accum += dt;
			++frames;
			
			// Interval ended - update GUI text and start new interval
			if( timeleft <= 0.0 ) {
				// display two fractional digits (f2 format)
				fps = (frames/accum);
				timeleft = fpsUpdateInterval;
				accum = 0.0f;
				frames = 0f;
			}
		}

		/// <summary>
		/// Pauses the game. All MonoBehaviours that implement IPausable will be paused.
		/// </summary>
		public static void PauseGame() {

			//if the game is already paused, return
			if (gamePaused) 
				return;

			//pause all pausable objects
			IPausable[] pausables = Util.FindObjectsOfType<IPausable> ();
			for (int i = 0; i < pausables.Length; i++) {
				pausables[i].Paused = true;
			}

			//state that the game is paused
			gamePaused = true;

			//cache the old timescale
			oldTimeScale = Time.timeScale;

			//stop all physics
			Time.timeScale = 0f;
		}

		/// <summary>
		/// Unpauses the game. All MonoBehaviours that implement IPausable will be unpaused.
		/// </summary>
		public static void UnpauseGame() {

			//if the game isn't paused, return
			if (!gamePaused)
				return;

			//unpause all paused objects
			IPausable[] pausables = Util.FindObjectsOfType<IPausable> ();
			for (int i = 0; i < pausables.Length; i++) {
				pausables[i].Paused = false;
			}

			//state that the game is no longer paused
			gamePaused = false;

			//reset the timescale back to normal
			Time.timeScale = oldTimeScale;
		}
	}
}