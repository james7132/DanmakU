using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityUtilLib;

namespace Danmaku2D {
	public class EnemyManager : SingletonBehavior<EnemyManager> {

		private List<AbstractEnemy> registeredEnemies;

		public static void RegisterEnemy(AbstractEnemy enemy) {
			Instance.registeredEnemies.Add (enemy);
		}

		public static void UnregisterEnemy(AbstractEnemy enemy) {
			Instance.registeredEnemies.Remove (enemy);
		}

		private AbstractDanmakuGameController controller;

		[SerializeField]
		private float roundStartSafeZone;
		private float chainSpawnCountdown;

		[System.Serializable]
		public class EnemySpawnData {
			public AbstractEnemy EnemyPrefab;
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
			registeredEnemies = new List<AbstractEnemy> ();
			controller = GetComponent<AbstractDanmakuGameController> ();
			if (controller == null) {
				Debug.Log("Error: Enemy Manager without Game Controller");
			}
		}

		void Start() {
			chainSpawnCountdown = roundStartSafeZone;
			weightSum = 0f;
			for (int i = 0; i < chains.Length; i++) {
				weightSum += chains[i].weight;
			}
		}

		void FixedUpdate() {
			chainSpawnCountdown -= Time.fixedDeltaTime;
			//Debug.Log (chainSpawnCountdown);
			if (chainSpawnCountdown <= 0f) {
				float randSelect = Random.value * weightSum;
				for(int i = 0; i < chains.Length; i++) {
					randSelect -= chains[i].weight;
					if(randSelect <= 0f) {
						StartCoroutine(SpawnEnemyChain(chains[i]));
						chainSpawnCountdown = chains[i].delay;
						break;
					}
				}
			}
		}

		public void RoundReset() {
			AbstractEnemy[] allEnemies = registeredEnemies.ToArray ();
			for (int i = 0; i < allEnemies.Length; i++) {
				Destroy (allEnemies[i].gameObject);
			}
		}

		public IEnumerator SpawnEnemyChain(EnemySpawnChain chain) {
			if (chain != null && chain.chain != null) {
				EnemySpawnData[] chainData = chain.chain;
				for(int i = 0; i < chainData.Length; i++) {
					Rect area = chainData[i].spawnArea;
					float rx = Random.Range(area.xMin, area.xMax);
					float ry = Random.Range(area.yMin, area.yMax);
					controller.SpawnEnemy(chainData[i].EnemyPrefab, new Vector2(rx, ry));
					float time = 0f;
					while(time < chainData[i].timeUntilNext) {
						yield return new WaitForFixedUpdate();
						time += Time.fixedDeltaTime;
					}
				}
			}
		}
	}
}