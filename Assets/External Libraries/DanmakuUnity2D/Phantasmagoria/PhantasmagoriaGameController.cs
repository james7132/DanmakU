using UnityEngine;
using System;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D.Phantasmagoria {
	public class PhantasmagoriaGameController : DanmakuGameController {

		[Serializable]
		public class PlayerData {

			[SerializeField]
			private PhantasmagoriaField field;
			public PhantasmagoriaField Field {
				get {
					return field;
				}
			}

			public int score = 0;
		}

		public PlayerData player1;
		public PlayerData player2;

		[SerializeField]
		private int winningScore = 3;
		public int WinningScore {
			get {
				return winningScore;
			}
		}

		[SerializeField]
		private float roundTime;

		private float roundTimeRemaining;
		public float RemainingRoundTime {
			get {
				return roundTimeRemaining;
			}
		}

		[SerializeField]
		private BasicEnemy guardian;

		[SerializeField]
		private Vector2 guardianSpawnLocation = new Vector2 (0.5f, 1.1f);
		private bool guardianSummoned;

		[SerializeField]
		private float closureDuration;

		[SerializeField]
		private Transform closureTop;

		[SerializeField]
		private Transform closureBottom;

		private bool reseting = false;

		public override void Awake() {
			base.Awake ();
			Physics2D.raycastsHitTriggers = true;
			if(player1.Field != null && player2.Field != null) {
				player1.Field.SetTargetField(player2.Field);
				player2.Field.SetTargetField(player1.Field);
				player1.Field.PlayerNumber = 1;
				player2.Field.PlayerNumber = 2;
				StartRound();
			}
		}

		void Update() {
			if (!reseting && (player1.Field.Player.LivesRemaining <= 0 || player2.Field.Player.LivesRemaining <= 0)) {
				StartCoroutine(RoundReset ());
			}
			roundTimeRemaining -= Util.TargetDeltaTime;
			if (roundTimeRemaining < 0f && !guardianSummoned) {
				if(guardian != null) {
					SpawnEnemy(guardian, guardianSpawnLocation);
					guardianSummoned = true;
				} else {
					Debug.LogWarning("Tried to summon a Guardian enemy that doesn't exist");
				}
			}
		}

		public void StartRound() {
			roundTimeRemaining = roundTime;
			guardianSummoned = false;
		}

		public IEnumerator RoundReset() {
			if(reseting)
				yield break;
			reseting = true;
			WaitForEndOfFrame wfeof = new WaitForEndOfFrame ();
			float duration = closureDuration / 2f;
			Vector3 scale = closureTop.localScale;
			Vector3 oldScale = scale;
			float dt = Util.TargetDeltaTime;
			float t = 0;
			scale.y = t;
			PauseGame ();
			while (t <= 1f) {
				scale.y = t;
				closureTop.localScale = scale;
				closureBottom.localScale = scale;
				yield return wfeof;
				t += dt / duration;
			}
			scale.y = 1f;
			closureTop.localScale = scale;
			closureBottom.localScale = scale;
			bool p1dead = player1.Field.Player.LivesRemaining <= 0;
			bool p2dead = player2.Field.Player.LivesRemaining <= 0;
			player1.score += (p2dead && !p1dead) ? 1 : 0;
			player2.score += (p1dead && !p2dead) ? 1 : 0;
			bool p1win = player1.score >= winningScore;
			bool p2win = player2.score >= winningScore;
			if(p1win && p2win) {
				//Signal Sudden Death
				player1.score = player2.score = 0;
				winningScore = 1;
			} else if(p1win) {
				//Declare Player 1 the winner
			} else if(p2win) {
				//Declare Player 2 the winner
			}
			player1.Field.RoundReset ();
			player2.Field.RoundReset ();
			ProjectileManager.DeactivateAll ();
			Enemy[] allEnemies = FindObjectsOfType<Enemy> ();
			for(int i = 0; i < allEnemies.Length; i++) {
				Destroy (allEnemies[i].GameObject);
			}
			BulletCancelArea[] bcas = FindObjectsOfType<BulletCancelArea> ();
			for(int i = 0; i < bcas.Length; i++) {
				Destroy (bcas[i].GameObject);
			}
			AttackPattern[] attackPatterns = FindObjectsOfType<AttackPattern> ();
			for (int i = 0; i < attackPatterns.Length; i++) {
				attackPatterns[i].Active = false;
			}
			while (t > 0f) {
				scale.y = t;
				closureTop.localScale = scale;
				closureBottom.localScale = scale;
				yield return wfeof;
				t -= dt / duration;
			}
			closureTop.localScale = oldScale;
			closureBottom.localScale = oldScale;
			UnpauseGame ();
			reseting = false;
		}

		public override void SpawnEnemy(Enemy prefab, Vector2 relativeLocations) {
			if(player1.Field != null && player2.Field != null) {
				player1.Field.SpawnEnemy(prefab, relativeLocations);
				player2.Field.SpawnEnemy(prefab, relativeLocations);
			}
		}
	}
}