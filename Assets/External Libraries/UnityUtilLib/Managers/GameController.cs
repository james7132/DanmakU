using UnityEngine;
using System.Collections;

namespace UnityUtilLib {
	
	public abstract class GameController : SingletonBehavior<GameController> {

		private static float oldTimeScale;
		private static bool gamePaused;
		public static bool IsGamePaused {
			get {
				return gamePaused;
			}
		}

		public static void PauseGame() {
			MonoBehaviour[] behaviors = FindObjectsOfType<MonoBehaviour> ();
			for (int i = 0; i < behaviors.Length; i++) {
				if(behaviors[i] is IPausable) {
					(behaviors[i] as IPausable).Paused = true;
				}
			}
			gamePaused = true;
			oldTimeScale = Time.timeScale;
			Time.timeScale = 0f;
		}

		public static void UnpauseGame() {
			MonoBehaviour[] behaviors = FindObjectsOfType<MonoBehaviour> ();
			for (int i = 0; i < behaviors.Length; i++) {
				if(behaviors[i] is IPausable) {
					(behaviors[i] as IPausable).Paused = false;
				}
			}
			gamePaused = false;
			Time.timeScale = oldTimeScale;
		}
	}
}