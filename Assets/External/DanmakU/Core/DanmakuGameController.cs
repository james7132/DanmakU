// Copyright (C) 2015  James Liu
//	
//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.
//		
//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.
//			
//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>

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
	[AddComponentMenu("Danmaku 2D/Danmaku Game Controller")]
	public class DanmakuGameController : GameController {

		[SerializeField]
		private int maximumLives;

		[SerializeField]
		private int projectileInitialCount = 1000;
		
		[SerializeField]
		private int projectileSpawnOnEmpty = 100;

		[SerializeField]
		private float angleResolution = 0.1f;

//		private static bool inTasks;
//		private static List<Task> tasks;

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
			Danmaku.Setup (projectileInitialCount, projectileSpawnOnEmpty, angleResolution);
		}
	}
}