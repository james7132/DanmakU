// Copyright (C) 2015  James Liu
//	
//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.
//		
//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.
//			
//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>

using UnityEngine;
using UnityUtilLib;
using System.Collections.Generic;

namespace Danmaku2D {

	[RequireComponent(typeof(Collider2D))]
	public abstract class DanmakuPlayer : DanmakuTrigger, IPausable {

		public virtual DanmakuField Field {
			get;
			set;
		}

		public bool Paused {
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
			Field = DanmakuField.FindClosest (transform.position);
			Field.player = this;
			movementCollider = GetComponent<Collider2D> ();
		}

		void Update() {
			if(!Paused)
				NormalUpdate();
		}

		public virtual void NormalUpdate() {
			if (agent != null)
				agent.Update ();
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

		public virtual bool IsFocused {
			get;
			set;
		}

		private Collider2D movementCollider;

		[SerializeField]
		private float fireRate = 4.0f;
		private float fireDelay;

		public virtual void Move(float horizontalDirection, float verticalDirection) {
			Bounds2D fieldBounds = Field.MovementBounds;
			Bounds2D myBounds = new Bounds2D(movementCollider.bounds);
			float dt = Util.TargetDeltaTime;
			float movementSpeed = (IsFocused) ? focusMovementSpeed : normalMovementSpeed;
			Vector2 position = transform.position;
			Vector2 movementVector = movementSpeed * dt * new Vector2 (Util.Sign(horizontalDirection), Util.Sign(verticalDirection));
			position += movementVector;
			myBounds.Center += movementVector;
			Vector2 myMin = myBounds.Min;
			Vector2 myMax = myBounds.Max;
			Vector2 fMin = fieldBounds.Min;
			Vector2 fMax = fieldBounds.Max;
//			Debug.Log (myMin.ToString () + " " + fMin.ToString () + " " + fMax.ToString());
			if (myMin.x < fMin.x)
				position.x += fMin.x - myMin.x;
			else if (myMax.x > fMax.x)
				position.x += fMax.x - myMax.x;
			else if (myMin.y < fMin.y)
				position.y += fMin.y - myMin.y;
			else if (myMax.y > fMax.y)
				position.y += fMax.y - myMax.y;
			transform.position = position;
		}

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
					Trigger();
					fireDelay = 1f / fireRate;
				}
			}
		}
	}
}