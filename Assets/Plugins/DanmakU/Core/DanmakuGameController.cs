// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityUtilLib;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace DanmakU {

	/// <summary>
	/// A GameController implementation for 2D Danmaku games.
	/// </summary>
//	[RequireComponent(typeof(EnemyManager))]
	[AddComponentMenu("DanmakU/Danmaku Game Controller")]
	public class DanmakuGameController : GameController {

		[SerializeField]
		private int maximumLives = 5;

		[SerializeField]
		private int danmakuInitialCount = 1000;
		
		[SerializeField]
		private int danmakuSpawnOnEmpty = 100;

		[SerializeField]
		private float angleResolution = 0.1f;

		/// <summary>
		/// The maximum number of lives a player can reach.
		/// </summary>
		/// <value>The maximum lives.</value>
		public static int MaximumLives {
			get {
				return (Instance as DanmakuGameController).maximumLives;
			}
		}

		public override void Update() {
			base.Update ();
			if (!IsGamePaused) {
				Danmaku.UpdateAll();
			}
		}

		public override void Awake () {
			base.Awake ();
			Danmaku.Setup (danmakuInitialCount, danmakuSpawnOnEmpty, angleResolution);
		}
	}
}