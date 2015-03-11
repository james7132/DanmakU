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
		/// The DanmakuField that all bullets fired by this pattern will end up within. <br>
		/// This MUST be set to a non-null value before firing any bullets.
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

		/// <summary>
		/// Helper method to quickly get the angle needed to directly fire at the player in the AttacKPattern's target field
		/// </summary>
		/// <returns>The angle needed to fire directly toward the player.</returns>
		/// <param name="position">The position to evaluate the angle to the player from.</param>
		/// <param name="coordSys">The cordinate system used to evaluate the true location of the source location</param>
		protected float AngleToPlayer(Vector2 position, DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.World) {
			return targetField.AngleTowardPlayer(Transform.position, coordSys);
		}

		/// <summary>
		/// The Main Loop of the AttackPattern, called once every frame during the AttackPattern's execution
		/// </summary>
		protected abstract void MainLoop();

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Danmaku2D.AttackPattern"/> is active.
		/// Setting this to false on a currently executing AttackPattern will terminate its execution immediately.
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

		/// <summary>
		/// An overridable function that is called every time the AttackPattern starts its execution.
		/// Use this for setup of various execution related variables
		/// </summary>
		protected virtual void OnExecutionStart() {
		}
		/// <summary>
		/// An overridable function that is called every time the AttackPattern finishes its execution.
		/// Use this for cleanup of various execution related variables
		/// </summary>
		protected virtual void OnExecutionFinish() {
		}

		/// <summary>
		/// Starts the execution of this AttackPattern
		/// </summary>
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
				yield return UtilCoroutines.AbstractProjectileController(this);
			}
			OnExecutionFinish ();
			Active = false;
		}

		/// <summary>
		/// Helper method for subclasses to quickly spawn projectiles.
		/// </summary>
		/// <returns>The projectile spawned with the given parameters</returns>
		/// <param name="bulletType">The defining characteristics behind this projectile</param>
		/// <param name="location">The location the bullet is to be spawned at. Expected value varies with the provided CoordinateSystem</param>
		/// <param name="rotation">The rotation the bullet is to be spawned with.</param>
		/// <param name="coordSys">The Coordinate system the location is to be spawned using</param>
		protected Projectile SpawnProjectile(ProjectilePrefab bulletType,
		                                     Vector2 location,
		                                     float rotation,
		                                     DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View) {
			return targetField.SpawnProjectile (bulletType, location, rotation, coordSys);
		}

		/// <summary>
		/// Helper method for subclasses to quickly firing straight moving bullets.
		/// </summary>
		/// <returns>The projectile spawned with the given parameters</returns>
		/// <param name="bulletType">The defining characteristics behind this projectile</param>
		/// <param name="location">The location the bullet is to be fired from. Expected value varies with the provided CoordinateSystem</param>
		/// <param name="rotation">The rotation the bullet is to be fired with.</param>
		/// <param name="coordSys">The Coordinate system the location is to be spawned using</param>
		protected LinearProjectile FireLinearBullet(ProjectilePrefab bulletType, 
		                                      Vector2 location, 
		                                      float rotation, 
		                                      float velocity,
		                                      DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View) {
			return targetField.FireLinearBullet (bulletType, location, rotation, velocity, coordSys);
		}

		/// <summary>
		/// Helper method for subclasses to quickly firing bullet that move along curved paths.
		/// </summary>
		/// <returns>The projectile spawned with the given parameters</returns>
		/// <param name="bulletType">The defining characteristics behind this projectile</param>
		/// <param name="location">The location the bullet is to be fired from. Expected value varies with the provided CoordinateSystem</param>
		/// <param name="rotation">The rotation the bullet is to be fired with.</param>
		/// <param name="coordSys">The Coordinate system the location is to be spawned using</param>
		protected CurvedProjectile FireCurvedBullet(ProjectilePrefab bulletType,
		                                      Vector2 location,
		                                      float rotation,
		                                      float velocity,
		                                      float angularVelocity,
		                                      DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View) {
			return targetField.FireCurvedBullet (bulletType, location, rotation, velocity, angularVelocity, coordSys);
		}

		/// <summary>
		/// Helper method for subclasses to quickly firing bullets with custom behavior.
		/// </summary>
		/// <returns>The projectile spawned with the given parameters</returns>
		/// <param name="bulletType">The defining characteristics behind this projectile</param>
		/// <param name="location">The location the bullet is to be fired from. Expected value varies with the provided CoordinateSystem</param>
		/// <param name="rotation">The rotation the bullet is to be fired with.</param>
		/// <param name="coordSys">The Coordinate system the location is to be spawned using</param>
		protected void FireControlledBullet(ProjectilePrefab bulletType, 
		                                    Vector2 location, 
		                                    float rotation, 
		                                    IProjectileController controller,
		                                    DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View) {
			targetField.FireControlledBullet (bulletType, location, rotation, controller, coordSys);
		}
	}
}