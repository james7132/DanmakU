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