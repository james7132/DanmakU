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

namespace Danmaku2D {
	public class EnemyManager : Singleton<EnemyManager>, IPausable {

		private List<Enemy> registeredEnemies;

		public static void RegisterEnemy(Enemy enemy) {
//			Instance.registeredEnemies.Add (enemy);
		}

		public static void UnregisterEnemy(Enemy enemy) {
//			Instance.registeredEnemies.Remove (enemy);
		}

		private DanmakuGameController controller;

		[SerializeField]
		private float roundStartSafeZone;
		private FrameCounter chainSpawnCountdown;

		[System.Serializable]
		public class EnemySpawnData {
			public Enemy EnemyPrefab;
			public Rect spawnArea;
			public float timeUntilNext;
		}

		[System.Serializable]
		public class EnemySpawnChain {
			public float weight = 25;
			public float delay = 10;
			public EnemySpawnData[] chain;
		}

		private float weightSum;

		public EnemySpawnChain[] chains;

		public override void Awake () {
			base.Awake ();
			registeredEnemies = new List<Enemy> ();
			controller = GetComponent<DanmakuGameController> ();
			if (controller == null) {
				print("Error: Enemy Manager without Game Controller");
			}
		}

		void Start() {
			weightSum = 0f;
			for (int i = 0; i < chains.Length; i++) {
				weightSum += chains[i].weight;
			}
			chainSpawnCountdown = new FrameCounter (roundStartSafeZone);
		}

		public void Update() {
			if (!Paused)
				NormalUpdate ();
		}

		public void NormalUpdate () {
			if (chainSpawnCountdown.Tick()) {
				float randSelect = Random.value * weightSum;
				for(int i = 0; i < chains.Length; i++) {
					randSelect -= chains[i].weight;
					if(randSelect <= 0f) {
						StartCoroutine(SpawnEnemyChain(chains[i]));
						chainSpawnCountdown = new FrameCounter(chains[i].delay);
						break;
					}
				}
			}
		}

		public void RoundReset() {
			Enemy[] allEnemies = registeredEnemies.ToArray ();
			for (int i = 0; i < allEnemies.Length; i++) {
				Destroy (allEnemies[i].gameObject);
			}
		}

		public IEnumerator SpawnEnemyChain(EnemySpawnChain chain) {
			if (chain != null && chain.chain != null) {
				EnemySpawnData[] chainData = chain.chain;
				for(int i = 0; i < chainData.Length; i++) {
					List<DanmakuField> fields = DanmakuField.fields;
					Rect area = chainData[i].spawnArea;
					float rx = Random.Range(area.xMin, area.xMax);
					float ry = Random.Range(area.yMin, area.yMax);
					for(int j = 0; j < fields.Count; j++) {
						fields[j].SpawnEnemy(chainData[i].EnemyPrefab, new Vector2(rx, ry));
					}
					float time = 0f;
					while(time < chainData[i].timeUntilNext) {
						yield return UtilCoroutines.WaitForUnpause(this);
						time += Util.TargetDeltaTime;
					}
				}
			}
		}

		#region IPausable implementation

		public bool Paused {
			get;
			set;
		}

		#endregion

	}
}