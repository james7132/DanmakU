using UnityEngine;
using System.Collections;
using UnityUtilLib;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
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

		protected override void MainLoop () {
			if(timeout.Tick (false)) {
				return;
			}
		}

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