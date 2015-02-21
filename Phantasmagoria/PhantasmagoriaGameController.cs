using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Game controller.
/// </summary>
public class PhantasmagoriaGameController : AbstractDanmakuGameController {
	
	//TODO: Document Comment
	[Serializable]
	public class PlayerData {

		[SerializeField]
		private PhantasmagoriaField field;
		/// <summary>
		/// Gets the field.
		/// </summary>
		/// <value>The field.</value>
		public PhantasmagoriaField Field {
			get {
				return field;
			}
		}

		/// <summary>
		/// The score.
		/// </summary>
		public int score = 0;
	}

	/// <summary>
	/// The player1.
	/// </summary>
	public PlayerData player1;

	/// <summary>
	/// The player2.
	/// </summary>
	public PlayerData player2;

	[SerializeField]
	private int winningScore = 3;
	/// <summary>
	/// Gets the winning score.
	/// </summary>
	/// <value>The winning score.</value>
	public int WinningScore {
		get {
			return winningScore;
		}
	}

	/// <summary>
	/// The round time.
	/// </summary>
	[SerializeField]
	private float roundTime;

	private float roundTimeRemaining;
	/// <summary>
	/// Gets the remaining round time.
	/// </summary>
	/// <value>The remaining round time.</value>
	public float RemainingRoundTime {
		get {
			return roundTimeRemaining;
		}
	}

	/// <summary>
	/// The guardian.
	/// </summary>
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

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		Physics2D.raycastsHitTriggers = true;
		if(player1.Field != null && player2.Field != null) {
			player1.Field.SetTargetField(player2.Field);
			player2.Field.SetTargetField(player1.Field);
			player1.Field.PlayerNumber = 1;
			player2.Field.PlayerNumber = 2;
			StartRound();
		}
	}

	/// <summary>
	/// Fixeds the update.
	/// </summary>
	void FixedUpdate() {
		bool reset = false;
		if (player1.Field.LivesRemaining <= 0) {
			player2.score++;
			reset = true;
		}
		if (player2.Field.LivesRemaining <= 0) {
			player1.score++;
			reset = true;
		}
		if(player1.score >= winningScore && player2.score >= winningScore) {
			//Signal Sudden Death
			player1.score = player2.score = 0;
			winningScore = 0;
		} else if(player1.score >= winningScore) {
			//Declare Player 1 the winner
		} else if(player1.score >= winningScore) {
			//Declare Player 2 the winner
		} else if(reset) {
			StartCoroutine(RoundReset ());
		}
		roundTimeRemaining -= Time.fixedDeltaTime;
		if (roundTimeRemaining < 0f && !guardianSummoned) {
			if(guardian != null) {
				SpawnEnemy(guardian, guardianSpawnLocation);
				guardianSummoned = true;
			} else {
				Debug.LogWarning("Tried to summon a Guardian enemy that doesn't exist");
			}
		}
	}

	/// <summary>
	/// Starts the round.
	/// </summary>
	public void StartRound() {
		roundTimeRemaining = roundTime;
		guardianSummoned = false;
	}

	/// <summary>
	/// Reset this instance.
	/// </summary>
	public IEnumerator RoundReset() {
		float oldTimeScale = Time.timeScale;
		float duration = closureDuration / 2f;
		Vector3 scale = closureTop.localScale;
		Vector3 oldScale = scale;
		Time.timeScale = 0;
		float t = 0;
		scale.y = t;
		while (t <= 1f) {
			scale.y = t;
			closureTop.localScale = scale;
			closureBottom.localScale = scale;
			yield return new WaitForEndOfFrame ();
			t += Time.unscaledDeltaTime / duration;
		}
		scale.y = 1f;
		closureTop.localScale = scale;
		closureBottom.localScale = scale;
		player1.Field.RoundReset ();
		player2.Field.RoundReset ();
		Projectile[] allProjectiles = FindObjectsOfType<Projectile> ();
		for(int i = 0; i < allProjectiles.Length; i++) {
			allProjectiles[i].DeactivateImmediate();
		}
		AbstractEnemy[] allEnemies = FindObjectsOfType<AbstractEnemy> ();
		for(int i = 0; i < allEnemies.Length; i++) {
			Destroy (allEnemies[i].GameObject);
		}
		BulletCancelArea[] bcas = FindObjectsOfType<BulletCancelArea> ();
		for(int i = 0; i < bcas.Length; i++) {
			Destroy (bcas[i].GameObject);
		}
		while (t > 0f) {
			scale.y = t;
			closureTop.localScale = scale;
			closureBottom.localScale = scale;
			yield return new WaitForEndOfFrame ();
			t -= Time.unscaledDeltaTime / duration;
		}
		closureTop.localScale = oldScale;
		closureBottom.localScale = oldScale;
		Time.timeScale = oldTimeScale;
	}

	/// <summary>
	/// Spawns the enemy.
	/// </summary>
	/// <param name="prefab">Prefab.</param>
	/// <param name="relativeLocations">Relative locations.</param>
	public override void SpawnEnemy(AbstractEnemy prefab, Vector2 relativeLocations) {
		if(player1.Field != null && player2.Field != null) {
			player1.Field.SpawnEnemy(prefab, relativeLocations);
			player2.Field.SpawnEnemy(prefab, relativeLocations);
		}
	}
}
