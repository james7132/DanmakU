using UnityEngine;
using UnityUtilLib;
using System.Collections;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {

	/// <summary>
	/// An abstract class that defines the basic functionality of a DanmakuUnity Attack Pattern.
	/// Derived classes of AbstractAttackPattern are used to define and control the various intricate patterns seen in danmaku games.
	/// </summary>
	public abstract class AttackPattern : CachedObject, IPausable {
		#region IPausable implementation
		public bool Paused {
			get;
			set;
		}
		#endregion

		/// <summary>
		/// The DanmakuField that all bullets fired by this pattern will end up within. <br>
		/// This MUST be set to a non-null value before firing any bullets.
		/// <see cref="DanmakuField"/>
		/// </summary>
		/// <value>The AttackPattern's target danmaku field</value>
		public DanmakuField TargetField;

		/// <summary>
		/// Helper method to quickly get the angle needed to directly fire at the player in the AttacKPattern's target field
		/// </summary>
		/// <returns>The angle needed to fire directly toward the player.</returns>
		/// <param name="position">The position to evaluate the angle to the player from.</param>
		/// <param name="coordSys">The cordinate system used to evaluate the true location of the source location</param>
		protected float AngleToPlayer(Vector2 position, DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.World) {
			return TargetField.AngleTowardPlayer(transform.position, coordSys);
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
		protected virtual void OnInitialize() {
		}

		/// <summary>
		/// An overridable function that is called every time the AttackPattern finishes its execution.
		/// Use this for cleanup of various execution related variables
		/// </summary>
		protected virtual void OnFinalize() {
		}

		/// <summary>
		/// Starts the execution of this AttackPattern
		/// </summary>
		public virtual void Fire () {
			if (!Active) {
				StartCoroutine (Execute ());
			} else {
				print("Tried Executing Already Running Attack Pattern");
			}
		}
		
		private IEnumerator Execute() {
			Active = true;
			OnInitialize ();
			while(!IsFinished && Active) {
				MainLoop();
				yield return UtilCoroutines.WaitForUnpause(this);
			}
			OnFinalize ();
			Active = false;
		}

		protected Danmaku SpawnProjectile(DanmakuPrefab projectileType,
		                                     Vector2 location,
		                                     DynamicFloat rotation,
		                                     DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View) {
			return TargetField.SpawnProjectile (projectileType, location, rotation, coordSys);
		}

		protected Danmaku FireLinear(DanmakuPrefab projectileType, 
		                                      Vector2 location, 
		                             		  DynamicFloat rotation, 
		                             		  DynamicFloat velocity,
		                                      DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View,
		                                      DanmakuController controller = null,
		                                      FireModifier modifier = null,
		                                      DanmakuGroup group = null) {
			return TargetField.FireLinear (projectileType, location, rotation, velocity, coordSys, controller, modifier, group);
		}

		protected Danmaku FireCurved(DanmakuPrefab projectileType,
	                                    Vector2 location,
	                                    DynamicFloat rotation,
	                                    DynamicFloat velocity,
	                                    DynamicFloat angularVelocity,
	                                    DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View,
	                                    DanmakuController controller = null,
	                                    FireModifier modifier = null,
	                                    DanmakuGroup group = null) {
			return TargetField.FireCurved (projectileType, location, rotation, velocity, angularVelocity, coordSys, controller, modifier, group);
		}

		protected Danmaku Fire(FireBuilder data) {
			return TargetField.Fire (data);
		}
	}
}