using UnityEngine;
using UnityUtilLib;
using System.Collections.Generic;

namespace Danmaku2D {

	[RequireComponent(typeof(Collider2D))]
	public abstract class DanmakuPlayer : PausableGameObject {

		public virtual DanmakuField Field {
			get;
			set;
		}
		
		private PlayerAgent agent;
		public PlayerAgent Agent {
			get {
				return agent;
			}
			set {
				agent = value;
				agent.Player = this;
			}
		}

		public override void Awake () {
			base.Awake ();
			Field = Util.FindClosest<DanmakuField> (transform.position);
			Field.player = this;
		}
		
		public override void NormalUpdate () {
			base.NormalUpdate ();
			if(agent != null)
				agent.Update();
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

		public virtual bool IsFiring {
			get;
			set;
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

		public virtual void Move(float horizontalDirection, float verticalDirection, bool focus) {
			float dt = Util.TargetDeltaTime;
			float movementSpeed = (focus) ? focusMovementSpeed : normalMovementSpeed;
			Vector2 dir = new Vector2 (Util.Sign(horizontalDirection), Util.Sign(verticalDirection));
			Vector3 movementVector = movementSpeed * Vector3.one;
			movementVector.x *= (dir.x == Util.Sign(forbiddenMovement.x)) ? 0f : dir.x;
			movementVector.y *= (dir.y == Util.Sign(forbiddenMovement.y)) ? 0f : dir.y;
			movementVector.z = 0f;
			transform.position += movementVector * dt;
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

		public virtual void Hit(Danmaku proj) {
			livesRemaining--;
		}

		public void Reset(int maxLives) {
			livesRemaining = maxLives;
		}

		public virtual void Graze (Danmaku proj) {
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