using UnityEngine;
using UnityUtilLib;
using System.Collections.Generic;

namespace Danmaku2D {
	public abstract class AbstractAgentCharacter : AbstractPlayableCharacter {

		private AbstractPlayerAgent agent;
		public AbstractPlayerAgent Agent {
			get {
				return agent;
			}
			set {
				agent = value;
			}
		}

		public virtual void Initialize(AbstractPlayerAgent agent) {
			this.agent = agent;
			agent.Player = this;
		}

		public virtual void FixedUpdate() {
			agent.Update (Time.fixedDeltaTime);
		}
	}

	[RequireComponent(typeof(Collider2D))]
	public abstract class AbstractPlayableCharacter : CachedObject {

		private AbstractDanmakuField field;
		public AbstractDanmakuField Field { 
			get {
				return field;
			}
			set {
				field = value;
			}
		}

		[SerializeField]
		private float normalMovementSpeed = 5f;

		[SerializeField]
		private float focusMovementSpeed = 3f;

		private int livesRemaining;
		public int LivesRemaining {
			get {
				return livesRemaining;
			}
		}

		private bool firing = false;
		public virtual bool IsFiring {
			get { 
				return firing;
			}
			set {
				firing = value;
			}
		}

		[SerializeField]
		private float fireRate = 4.0f;
		private float fireDelay;

		private Vector2 forbiddenMovement = Vector3.zero;

		public int CanMoveHorizontal {
			get { return -(int)Util.Sign(forbiddenMovement.x); }
		}

		public int CanMoveVertical {
			get { return -(int)Util.Sign(forbiddenMovement.y); }
		}

		public virtual void Move(float horizontalDirection, float verticalDirection, bool focus, float dt = 1.0f) {
			float movementSpeed = (focus) ? focusMovementSpeed : normalMovementSpeed;
			Vector2 dir = new Vector2 (Util.Sign(horizontalDirection), Util.Sign(verticalDirection));
			Vector3 movementVector = movementSpeed * Vector3.one;
			movementVector.x *= (dir.x == Util.Sign(forbiddenMovement.x)) ? 0f : dir.x;
			movementVector.y *= (dir.y == Util.Sign(forbiddenMovement.y)) ? 0f : dir.y;
			movementVector.z = 0f;
			Transform.position += movementVector * dt;
		}

		public void AllowMovement(Vector2 direction) {
			if(Util.Sign(direction.x) == Util.Sign(forbiddenMovement.x)) {
				forbiddenMovement.x = 0;
			}
			if(Util.Sign(direction.y) == Util.Sign(forbiddenMovement.y)) {
				forbiddenMovement.y = 0;
			}
		}

		public void ForbidMovement(Vector2 direction) {
			if(direction.x != 0) {
				forbiddenMovement.x = direction.x;
			}
			if(direction.y != 0) {
				forbiddenMovement.y = direction.y;
			}
		}

		public abstract void Fire ();

		public virtual void Hit(Projectile proj) {
			livesRemaining--;
		}

		public void Reset(int maxLives) {
			livesRemaining = maxLives;
		}

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
}