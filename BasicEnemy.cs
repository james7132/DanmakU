using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AbstractMovementPattern))]
public class BasicEnemy : AbstractEnemy {

	[SerializeField]
	private float maxHealth;
	private float currentHealth;
	public float CurrentHealth {
		get {
			return currentHealth;
		}
	}

	private AbstractMovementPattern movementPattern;

	private AbstractAttackPattern attackPattern;
	/// <summary>
	/// Gets the current attack pattern.
	/// </summary>
	/// <value>The current attack pattern.</value>
	public override AbstractAttackPattern CurrentAttackPattern {
		get {
			return attackPattern;
		}
	}

	public override void Awake() {
		base.Awake ();
		currentHealth = maxHealth;
		movementPattern = GetComponent<AbstractMovementPattern> ();
		movementPattern.DestroyOnEnd = true;
		attackPattern = GetComponent<AbstractAttackPattern> ();
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
