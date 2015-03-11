using UnityEngine;
using System.Collections;

namespace Danmaku2D {

	/// <summary>
	/// A basic enemy. <br>
	/// Usually used as cannon fodder in Danmaku games.
	/// </summary>
	[RequireComponent(typeof(MovementPattern))]
	public class BasicEnemy : Enemy {

		[SerializeField]
		private float maxHealth;
		private float currentHealth;

		/// <summary>
		/// Gets the current health. 
		/// </summary>
		/// <value>The current health.</value>
		public float CurrentHealth {
			get {
				return currentHealth;
			}
		}

		private MovementPattern movementPattern;

		private AttackPattern attackPattern;
		/// <summary>
		/// Gets the current attack pattern.
		/// </summary>
		/// <value>The current attack pattern.</value>
		public override AttackPattern CurrentAttackPattern {
			get {
				return attackPattern;
			}
		}

		public override void Awake() {
			base.Awake ();
			currentHealth = maxHealth;
			movementPattern = GetComponent<MovementPattern> ();
			movementPattern.DestroyOnEnd = true;
			attackPattern = GetComponent<AttackPattern> ();
			if (attackPattern != null) {
				attackPattern.TargetField = Field.TargetField;
				attackPattern.Fire();
			}
			movementPattern.StartMovement ();
		}

		protected override void Damage (float damage) {
			currentHealth -= damage;
			Debug.Log (currentHealth);
		}
		
		public override bool IsDead {
			get {
				return currentHealth <= 0;
			}
		}
	}
}
