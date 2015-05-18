// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace DanmakU {

	/// <summary>
	/// A GameController implementation for 2D Danmaku games.
	/// </summary>
	[AddComponentMenu("DanmakU/Danmaku Game Controller")]
	public class DanmakuGameController : Singleton<DanmakuGameController> {
		
		public bool FrameRateIndependent = true;

		[SerializeField]
		private int danmakuInitialCount = Danmaku.standardStart;
		
		[SerializeField]
		private int danmakuSpawnOnEmpty = Danmaku.standardSpawn;

		[SerializeField]
		private float angleResolution = 0.1f;

		private static Queue<FireBuilder> fires;

		public virtual void Update() {
			if (fires.Count > 0) {
				FireBuilder current;
				while (fires.Count > 0) {
					current = fires.Dequeue ();
					current.Execute ();
				}
			}
			Danmaku.UpdateAll ();
		}

		internal void QueueFire(FireBuilder builder) {
			if (builder != null)
				builder.Execute ();
		}

		public override void Awake () {
			base.Awake ();
			if(fires == null)
				fires = new Queue<FireBuilder> ();
			Danmaku.Setup (danmakuInitialCount, danmakuSpawnOnEmpty, angleResolution);
		}

		protected virtual void OnLevelWasLoaded(int level) {
			fires.Clear ();
			Danmaku.DeactivateAll ();
		}
	}
}