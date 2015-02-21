using UnityEngine;
using UnityUtilLib;
using System.Collections;

/// <summary>
/// Enemy.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class AbstractEnemy : CachedObject {

	/// <summary>
	/// Gets the current attack pattern.
	/// </summary>
	/// <value>The current attack pattern.</value>
	public virtual AbstractAttackPattern CurrentAttackPattern { 
		get {
			return null;
		}
	}

	private AbstractDanmakuField field;
	/// <summary>
	/// Gets or sets the field.
	/// </summary>
	/// <value>The field.</value>
	public AbstractDanmakuField Field {
		get {
			return field;
		}
		set {
			field = value;
		}
	}

	public abstract bool IsDead { get; }

	/// <summary>
	/// Start this instance.
	/// </summary>
	public virtual void Start() {
		EnemyManager.RegisterEnemy (this);
	}

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player Shot")) {
			Projectile proj = other.GetComponent<Projectile>();
			if(proj != null) {
				Hit (proj.Damage);
				proj.Deactivate ();
				OnDamage();
			}
		}
	}

	/// <summary>
	/// Hit the specified proj.
	/// </summary>
	/// <param name="proj">Proj.</param>
	public void Hit(float damage) {
		Damage (damage);
		if(IsDead) {
			Die ();
		}
	}

	protected abstract void Damage (float damage);

	private void Die () {
		EnemyManager.UnregisterEnemy (this);
		Destroy (GameObject);
		OnDeath ();
	}

	protected virtual void OnDeath() {
	}

	protected virtual void OnDamage() {
	}

	void OnProjectileCollision(Projectile proj) {
		Hit (proj.Damage);
		proj.Deactivate();
	}
}
