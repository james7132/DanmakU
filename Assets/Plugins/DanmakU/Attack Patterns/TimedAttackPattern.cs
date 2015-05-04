// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;
using UnityUtilLib;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace DanmakU {
	/// <summary>
	/// A abstract class meant for time-limited AttackPatterns
	/// </summary>
	public abstract class TimedAttackPattern : AttackPattern {

		/// <summary>
		/// Defines how long the AttackPattern will last before automatically terminating
		/// </summary>
		[SerializeField]
		private FrameCounter timeout;

		protected override IEnumerator MainLoop () {
			if(timeout.Tick (false)) {
			}
			yield return null;
		}

		protected override void OnInitialize () {
			timeout.Reset ();
		}
	}
}