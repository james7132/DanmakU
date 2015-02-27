using UnityEngine;
using UnityUtilLib;
using System;
using System.Collections;

namespace Danmaku2D {
	public abstract class AbstractAttackPattern : CachedObject {

		private AbstractDanmakuField targetField;
		public AbstractDanmakuField TargetField {
			get {
				return targetField;
			}
			set {
				targetField = value;
			}
		}

		protected abstract bool IsFinished {
			get; 
		}

		protected float AngleToPlayer {
			get {
				return targetField.AngleTowardPlayer(Transform.position);
			}
		}

		private bool attackActive;

		protected virtual void OnExecutionStart() {
		}

		protected abstract void MainLoop(float dt);

		protected virtual void OnExecutionFinish() {
		}

		protected Projectile FireLinearBullet(ProjectilePrefab bulletType, 
		                                Vector2 location, 
		                                float rotation, 
		                                float velocity) {
			return FireCurvedBullet(bulletType, location, rotation, velocity, 0f);
		}

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

		protected void Terminate() {
			attackActive = false;
		}

		public void Fire() {
			StartCoroutine (Execute ());
		}

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
}