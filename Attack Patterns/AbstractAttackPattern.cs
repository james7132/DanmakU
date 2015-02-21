using UnityEngine;
using UnityUtilLib;
using System;
using System.Collections;

/// <summary>
/// Attack pattern.
/// </summary>
public abstract class AbstractAttackPattern : CachedObject {

	private AbstractDanmakuField targetField;

	/// <summary>
	/// Gets or sets the target field.
	/// </summary>
	/// <value>The target field.</value>
	public AbstractDanmakuField TargetField {
		get {
			return targetField;
		}
		set {
			targetField = value;
		}
	}

	protected abstract bool IsFinished { get; }

	/// <summary>
	/// Gets the angle to player.
	/// </summary>
	/// <value>The angle to player.</value>
	protected float AngleToPlayer {
		get {
			return targetField.AngleTowardPlayer(Transform.position);
		}
	}

	/// <summary>
	/// The attack active.
	/// </summary>
	private bool attackActive;

	/// <summary>
	/// Raises the execution start event.
	/// </summary>
	protected virtual void OnExecutionStart() {
	}

	/// <summary>
	/// Mains the loop.
	/// </summary>
	/// <param name="dt">Dt.</param>
	protected abstract void MainLoop(float dt);

	/// <summary>
	/// Raises the execution finish event.
	/// </summary>
	protected virtual void OnExecutionFinish() {
	}

	/// <summary>
	/// Fires the linear bullet.
	/// </summary>
	/// <returns>The linear bullet.</returns>
	/// <param name="bulletType">Bullet type.</param>
	/// <param name="location">Location.</param>
	/// <param name="rotation">Rotation.</param>
	/// <param name="velocity">Velocity.</param>
	protected Projectile FireLinearBullet(ProjectilePrefab bulletType, 
	                                Vector2 location, 
	                                float rotation, 
	                                float velocity) {
		return FireCurvedBullet(bulletType, location, rotation, velocity, 0f);
	}

	/// <summary>
	/// Fires the curved bullet.
	/// </summary>
	/// <returns>The curved bullet.</returns>
	/// <param name="bulletType">Bullet type.</param>
	/// <param name="location">Location.</param>
	/// <param name="rotation">Rotation.</param>
	/// <param name="velocity">Velocity.</param>
	/// <param name="angularVelocity">Angular velocity.</param>
	protected Projectile FireCurvedBullet(ProjectilePrefab bulletType,
	                                      Vector2 location,
	                                      float rotation,
	                                      float velocity,
	                                      float angularVelocity) {
		Projectile bullet = targetField.SpawnProjectile (bulletType, location, rotation);
		bullet.Velocity = velocity;
		bullet.AngularVelocity = angularVelocity;
		return bullet;
	}

	/// <summary>
	/// Terminate this instance.
	/// </summary>
	protected void Terminate() {
		attackActive = false;
	}

	/// <summary>
	/// Fire this instance.
	/// </summary>
	public void Fire() {
		StartCoroutine (Execute ());
	}

	/// <summary>
	/// Execute this instance.
	/// </summary>
	private IEnumerator Execute() {
		attackActive = true;
		OnExecutionStart ();
		WaitForFixedUpdate wffu = new WaitForFixedUpdate ();
		while(!IsFinished && attackActive) {
			MainLoop(Time.fixedDeltaTime);
			yield return wffu;
		}
		OnExecutionFinish ();
	}
}
