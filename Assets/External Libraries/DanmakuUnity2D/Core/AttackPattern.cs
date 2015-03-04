using UnityEngine;
using UnityUtilLib;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Danmaku2D {

	/// <summary>
	/// An abstract class that defines the basic functionality of a DanmakuUnity Attack Pattern.
	/// Derived classes of AbstractAttackPattern are used to define and control the various intricate patterns seen in danmaku games.
	/// </summary>
	public abstract class AttackPattern : PausableGameObject {

		private DanmakuField targetField;
		/// <summary>
		/// The DanmakuField that all bullets fired by this pattern will end up within.
		/// This MUST be set before firing any bullets.
		/// <see cref="DanmakuField"/>
		/// </summary>
		/// <value>The AttackPattern's target danmaku field</value>
		public DanmakuField TargetField {
			get {
				return targetField;
			}
			set {
				targetField = value;
			}
		}

		protected float AngleToPlayer(Vector2 position) {
			return targetField.AngleTowardPlayer(Transform.position);
		}

		protected abstract void MainLoop();

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Danmaku2D.AttackPattern+Execution"/> is active.
		/// </summary>
		/// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
		public bool Active {
			get;
			set;
		}
		
		/// <summary>
		/// Gets a value indicating whether this instance is finished.
		/// </summary>
		/// <value><c>true</c> if this instance is finished; otherwise, <c>false</c>.</value>
		protected abstract bool IsFinished { get; }
		
		protected virtual void OnExecutionStart() {
		}
		
		protected virtual void OnExecutionFinish() {
		}

		public virtual void Fire () {
			if (!Active) {
				StartCoroutine (Execute ());
			} else {
				Debug.Log("Tried Executing Already Running Attack Pattern");
			}
		}

		private IEnumerator Execute() {
			Active = true;
			OnExecutionStart ();
			while(!IsFinished && Active) {
				MainLoop();
				yield return UtilCoroutines.WaitForUnpause(this);
			}
			OnExecutionFinish ();
			Active = false;
		}

		protected Projectile SpawnProjectile(ProjectilePrefab bulletType,
		                                     Vector2 location,
		                                     float rotation,
		                                     DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View) {
			return targetField.SpawnProjectile (bulletType, location, rotation, coordSys);
		}

		protected LinearProjectile FireLinearBullet(ProjectilePrefab bulletType, 
		                                      Vector2 location, 
		                                      float rotation, 
		                                      float velocity,
		                                      DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View) {
			return targetField.FireLinearBullet (bulletType, location, rotation, velocity, coordSys);
		}
		
		protected CurvedProjectile FireCurvedBullet(ProjectilePrefab bulletType,
		                                      Vector2 location,
		                                      float rotation,
		                                      float velocity,
		                                      float angularVelocity,
		                                      DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View) {
			return targetField.FireCurvedBullet (bulletType, location, rotation, velocity, angularVelocity, coordSys);
		}

		protected void FireControlledBullet(ProjectilePrefab bulletType, 
		                                    Vector2 location, 
		                                    float rotation, 
		                                    IProjectileController controller,
		                                    DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View) {
			targetField.FireControlledBullet (bulletType, location, rotation, controller, coordSys);
		}
	}
}