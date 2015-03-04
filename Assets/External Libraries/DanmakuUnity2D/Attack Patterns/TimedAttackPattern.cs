using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {
	/// <summary>
	/// A abstract class meant for time-limited AttackPatterns
	/// </summary>
	public abstract class TimedAttackPattern : AttackPattern {

		/// <summary>
		/// Defines how long the AttackPattern will last before automatically terminating
		/// </summary>
		[SerializeField]
		private FrameCounter timeout;

		/// <summary>
		/// The main execution loop that is executed each frame.
		/// Override this function to implement any firing of bullets or manipulaiton of in-game objects.
		/// </summary>
		/// <param name="execution">The paramater set for the current execution. <see cref="AttackPatternExecution"/>></param>
		protected override void MainLoop () {
			if(timeout.Tick (false)) {
				return;
			}
		}

		/// <summary>
		/// Raises the execution start event.
		/// </summary>
		protected override void OnExecutionStart () {
			timeout.Reset ();
		}

		protected sealed override bool IsFinished {
			get {
				return timeout.Ready();
			}
		}
	}
}