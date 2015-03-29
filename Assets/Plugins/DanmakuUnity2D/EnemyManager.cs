using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityUtilLib;

namespace Danmaku2D {
	public class EnemyManager : Singleton<EnemyManager>, IPausable {

		private List<Enemy> registeredEnemies;

		public static void RegisterEnemy(Enemy enemy) {
			Instance.registeredEnemies.Add (enemy);
		}

		public static void UnregisterEnemy(Enemy enemy) {
			Instance.registeredEnemies.Remove (enemy);
		}

		private DanmakuGameController controller;

		[SerializeField]
		private FrameCounter roundStartSafeZone;
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
//					Rect area = chainData[i].spawnArea;
//					float rx = Random.Range(area.xMin, area.xMax);
//					float ry = Random.Range(area.yMin, area.yMax);
//					controller.SpawnEnemy(chainData[i].EnemyPrefab, new Vector2(rx, ry));
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