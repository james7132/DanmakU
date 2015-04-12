using UnityEngine;
using System;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D.Phantasmagoria {
	public class PhantasmagoriaGameController : DanmakuGameController {

		[Serializable]
		public class PlayerData {

			[SerializeField]
			private DanmakuField field;
			public DanmakuField Field {
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
		private float closureDuration;

		[SerializeField]
		private Transform[] closure;

		public GameObject p1winner;
		public GameObject p2winner;
		public GameObject p1loser;
		public GameObject p2loser;
		public float postConclusionWait;

		private bool reseting = false;

		public override void Awake() {
			base.Awake ();
			Physics2D.raycastsHitTriggers = true;
			if(player1.Field != null && player2.Field != null) {
				player1.Field.TargetField = player2.Field;
				player2.Field.TargetField = player1.Field;
				StartRound();
			}
		}

		public override void Update() {
			base.Update ();
			if (!reseting && (player1.Field.Player.LivesRemaining <= 0 || player2.Field.Player.LivesRemaining <= 0)) {
				StartCoroutine(RoundReset ());
			}
			roundTimeRemaining -= Util.TargetDeltaTime;
			bool p1win = player1.score >= winningScore;
			bool p2win = player2.score >= winningScore;
			if (p1win && p2win) {
				//Signal Sudden Death
				player1.score = player2.score = 0;
				winningScore = 1;
			} else if (p1win) {
				//Declare Player 1 the winner
				p1winner.SetActive (true);
				p2loser.SetActive(true);
				p1loser.SetActive(false);
				p2winner.SetActive(false);
				StartCoroutine(WaitForReturn());
			} else if (p2win) {
				//Declare Player 2 the winner
				p1winner.SetActive (false);
				p2loser.SetActive(false);
				p1loser.SetActive(true);
				p2winner.SetActive(true);
				StartCoroutine(WaitForReturn());
			}
		}

		public IEnumerator WaitForReturn() {
			yield return new WaitForSeconds (postConclusionWait);
			Application.LoadLevel(0);
		}

		public void StartRound() {
			roundTimeRemaining = roundTime;
		}

		private int playerNumber = 1;
		public int PlayerNumber {
			get {
				return playerNumber; 
			}
			set { 
				playerNumber = value; 
			}
		}
		
		public static void Transfer(Danmaku projectile) {
			DanmakuField field = projectile.Field;
			Vector2 relativePos = field.ViewPoint (projectile.Position);
			projectile.Position = field.TargetField.WorldPoint (relativePos);
		}

		public IEnumerator RoundReset() {
			if(reseting)
				yield break;
			reseting = true;
			WaitForEndOfFrame wfeof = new WaitForEndOfFrame ();
			float duration = closureDuration / 2f;
			Vector3 scale = new Vector3(1f, 0f, 1f);
			Vector3 oldScale = scale;
			float dt = Util.TargetDeltaTime;
			float t = 0;
			scale.y = t;
			PauseGame ();
			while (t <= 1f) {
				scale.y = t;
				for(int i = 0; i < closure.Length; i++) {
					closure[i].localScale = scale;
				}
				yield return wfeof;
				t += dt / duration;
			}
			scale.y = 1f;
			for(int i = 0; i < closure.Length; i++) {
				closure[i].localScale = Vector3.one;
			}
			bool p1dead = player1.Field.Player.LivesRemaining <= 0;
			bool p2dead = player2.Field.Player.LivesRemaining <= 0;
			player1.score += (p2dead && !p1dead) ? 1 : 0;
			player2.score += (p1dead && !p2dead) ? 1 : 0;
			player1.Field.Player.Reset (MaximumLives);
			player2.Field.Player.Reset (MaximumLives);
			player1.Field.Camera2DRotation = 0f;
			player2.Field.Camera2DRotation = 0f;
			Danmaku.DeactivateAll ();
			Enemy[] allEnemies = FindObjectsOfType<Enemy> ();
			for(int i = 0; i < allEnemies.Length; i++) {
				Destroy (allEnemies[i].gameObject);
			}
			BulletCancelArea[] bcas = FindObjectsOfType<BulletCancelArea> ();
			for(int i = 0; i < bcas.Length; i++) {
				Destroy (bcas[i].gameObject);
			}
			AttackPattern[] attackPatterns = FindObjectsOfType<AttackPattern> ();
			for (int i = 0; i < attackPatterns.Length; i++) {
				attackPatterns[i].Active = false;
			}
			while (t > 0f) {
				scale.y = t;
				for(int i = 0; i < closure.Length; i++) {
					closure[i].localScale = scale;
				}
				yield return null;
				t -= dt / duration;
			}
			for(int i = 0; i < closure.Length; i++) {
				closure[i].localScale = oldScale;
			}
			UnpauseGame ();
			reseting = false;
		}

//		public override void SpawnEnemy(Enemy prefab, Vector2 relativeLocations) {
//			if(player1.Field != null && player2.Field != null) {
//				player1.Field.SpawnEnemy(prefab, relativeLocations);
//				player2.Field.SpawnEnemy(prefab, relativeLocations);
//			}
//		}
	}
}