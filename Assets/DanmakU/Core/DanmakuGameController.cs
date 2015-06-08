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
	public sealed class DanmakuGameController : MonoBehaviour {
		
		public bool FrameRateIndependent = true;

		[SerializeField]
		private int danmakuInitialCount = Danmaku.standardStart;
		
		[SerializeField]
		private int danmakuSpawnOnEmpty = Danmaku.standardSpawn;

		[SerializeField]
		private float angleResolution = 0.1f;


		private static DanmakuGameController instance;
		
		public static DanmakuGameController Instance {
			get { 
				if(instance == null) {
					instance = FindObjectOfType<DanmakuGameController>();
					if(instance == null) {
						instance = new GameObject(typeof(DanmakuGameController).Name).AddComponent<DanmakuGameController>();
					}
				}
				return instance; 
			}
		}

		void Awake () {
			DontDestroyOnLoad (gameObject);
			if(instance != null) {
				Destroy (this);
				return;
			}
			instance = this;
			Danmaku.Setup (danmakuInitialCount, danmakuSpawnOnEmpty, angleResolution);
		}

		void Update() {
			Danmaku.UpdateAll ();
		}

		void OnLevelWasLoaded(int level) {
			Danmaku.DeactivateAll ();
		}
	}
}