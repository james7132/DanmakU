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

		public virtual void Update() {
			Danmaku.UpdateAll ();
		}

		public override void Awake () {
			base.Awake ();
			Danmaku.Setup (danmakuInitialCount, danmakuSpawnOnEmpty, angleResolution);
		}

		protected virtual void OnLevelWasLoaded(int level) {
			Danmaku.DeactivateAll ();
		}
	}
}