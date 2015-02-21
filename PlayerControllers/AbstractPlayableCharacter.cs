using UnityEngine;
using UnityUtilLib;
using System.Collections.Generic;

public abstract class AbstractAgentCharacter : AbstractPlayableCharacter {

	private AbstractPlayerAgent agent;
	/// <summary>
	/// Gets or sets the agent.
	/// </summary>
	/// <value>The agent.</value>
	public AbstractPlayerAgent Agent {
		get {
			return agent;
		}
		set {
			agent = value;
		}
	}

	/// <summary>
	/// Initialize the specified playerField and targetField.
	/// </summary>
	/// <param name="playerField">Player field.</param>
	/// <param name="targetField">Target field.</param>
	public virtual void Initialize(AbstractPlayerAgent agent) {
		this.agent = agent;
		agent.Player = this;
	}

	public virtual void FixedUpdate() {
		agent.Update (Time.fixedDeltaTime);
	}
}

/// <summary>
/// Avatar.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public abstract class AbstractPlayableCharacter : CachedObject {

	private AbstractDanmakuField field;
	/// <summary>
	/// Gets the field controller.
	/// </summary>
	/// <value>The field controller.</value>
	public AbstractDanmakuField Field { 
		get {
			return field;
		}
		set {
			field = value;
		}
	}

	/// <summary>
	/// The normal movement speed.
	/// </summary>
	[SerializeField]
	private float normalMovementSpeed = 5f;

	/// <summary>
	/// The focus movement speed.
	/// </summary>
	[SerializeField]
	private float focusMovementSpeed = 3f;

	private int livesRemaining;
	/// <summary>
	/// Gets the lives remaining.
	/// </summary>
	/// <value>The lives remaining.</value>
	public int LivesRemaining {
		get {
			return livesRemaining;
		}
	}

	private bool firing = false;
	/// <summary>
	/// Gets a value indicating whether this instance is firing.
	/// </summary>
	/// <value><c>true</c> if this instance is firing; otherwise, <c>false</c>.</value>
	public virtual bool IsFiring {
		get { 
			return firing;
		}
		set {
			firing = value;
		}
	}

	/// <summary>
	/// The fire rate.
	/// </summary>
	[SerializeField]
	private float fireRate = 4.0f;
	private float fireDelay;

	private Vector2 forbiddenMovement = Vector3.zero;
	/// <summary>
	/// Gets a value indicating whether this instance can move horizontal.
	/// </summary>
	/// <value><c>true</c> if this instance can move horizontal; otherwise, <c>false</c>.</value>
	public int CanMoveHorizontal {
		get { return -(int)Util.Sign(forbiddenMovement.x); }
	}

	/// <summary>
	/// Gets a value indicating whether this instance can move vertical.
	/// </summary>
	/// <value><c>true</c> if this instance can move vertical; otherwise, <c>false</c>.</value>
	public int CanMoveVertical {
		get { return -(int)Util.Sign(forbiddenMovement.y); }
	}

	/// <summary>
	/// Move the specified horizontalDirection, verticalDirection, focus and dt.
	/// </summary>
	/// <param name="horizontalDirection">Horizontal direction.</param>
	/// <param name="verticalDirection">Vertical direction.</param>
	/// <param name="focus">If set to <c>true</c> focus.</param>
	/// <param name="dt">Dt.</param>
	public virtual void Move(float horizontalDirection, float verticalDirection, bool focus, float dt = 1.0f) {
		float movementSpeed = (focus) ? focusMovementSpeed : normalMovementSpeed;
		Vector2 dir = new Vector2 (Util.Sign(horizontalDirection), Util.Sign(verticalDirection));
		Vector3 movementVector = movementSpeed * Vector3.one;
		movementVector.x *= (dir.x == Util.Sign(forbiddenMovement.x)) ? 0f : dir.x;
		movementVector.y *= (dir.y == Util.Sign(forbiddenMovement.y)) ? 0f : dir.y;
		movementVector.z = 0f;
		Transform.position += movementVector * dt;
	}

	/// <summary>
	/// Allows the movement.
	/// </summary>
	/// <param name="direction">Direction.</param>
	public void AllowMovement(Vector2 direction) {
		if(Util.Sign(direction.x) == Util.Sign(forbiddenMovement.x)) {
			forbiddenMovement.x = 0;
		}
		if(Util.Sign(direction.y) == Util.Sign(forbiddenMovement.y)) {
			forbiddenMovement.y = 0;
		}
	}

	/// <summary>
	/// Forbids the movement.
	/// </summary>
	/// <param name="direction">Direction.</param>
	public void ForbidMovement(Vector2 direction) {
		if(direction.x != 0) {
			forbiddenMovement.x = direction.x;
		}
		if(direction.y != 0) {
			forbiddenMovement.y = direction.y;
		}
	}

	/// <summary>
	/// Fire this instance.
	/// </summary>
	public abstract void Fire ();

	/// <summary>
	/// Hit this instance.
	/// </summary>
	public virtual void Hit(Projectile proj) {
		livesRemaining--;
	}

	/// <summary>
	/// Reset the specified maxLives.
	/// </summary>
	/// <param name="maxLives">Max lives.</param>
	public void Reset(int maxLives) {
		livesRemaining = maxLives;
	}

	/// <summary>
	/// Graze this instance.
	/// </summary>
	public virtual void Graze (Projectile proj) {
	}

	public void FireCheck(float dt) {
		if(IsFiring) {
			fireDelay -= dt;
			if(fireDelay < 0f) {
				Fire ();
				fireDelay = 1f / fireRate;
			}
		}
	}
}
