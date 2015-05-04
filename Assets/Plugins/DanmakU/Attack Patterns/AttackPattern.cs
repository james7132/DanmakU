// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityUtilLib;
using System.Collections;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace DanmakU {

	/// <summary>
	/// An abstract class that defines the basic functionality of a DanmakuUnity Attack Pattern.
	/// Derived classes of AbstractAttackPattern are used to define and control the various intricate patterns seen in danmaku games.
	/// </summary>
	public abstract class AttackPattern : CachedObject, IPausable, IDanmakuObject {
		#region IPausable implementation
		public bool Paused {
			get;
			set;
		}
		#endregion

		#region IDanmakuObject implementation
		/// <summary>
		/// The DanmakuField that all bullets fired by this pattern will end up within. <br>
		/// This MUST be set to a non-null value before firing any bullets.
		/// <see cref="DanmakuField"/>
		/// </summary>
		/// <value>The AttackPattern's target danmaku field</value>
		public virtual DanmakuField Field {
			get;
			set;
		}
		#endregion

		public bool Active;

		/// <summary>
		/// The Main Loop of the AttackPattern, called once every frame during the AttackPattern's execution
		/// </summary>
		protected abstract IEnumerator MainLoop();

		/// <summary>
		/// An overridable function that is called every time the AttackPattern starts its execution.
		/// Use this for setup of various execution related variables
		/// </summary>
		protected virtual void OnInitialize() {
			print("hello");
			return;
		}

		/// <summary>
		/// An overridable function that is called every time the AttackPattern finishes its execution.
		/// Use this for cleanup of various execution related variables
		/// </summary>
		protected virtual void OnFinalize() {
			return;
		}

		/// <summary>
		/// Starts the execution of this AttackPattern
		/// </summary>
		public virtual void Fire () {
			if (!Active) {
				StartTask (Execute ());
			} else {
				print("Tried Executing Already Running Attack Pattern");
			}
		}
		
		private IEnumerator Execute() {
			Active = true;
			OnInitialize ();
			yield return StartTask (MainLoop ());
			OnFinalize ();
			Active = false;
		}

		protected Danmaku SpawnDanmaku(DanmakuPrefab danmakuType,
		                                     Vector2 location,
		                                     DynamicFloat rotation,
		                                     DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View) {
			return Field.SpawnDanmaku (danmakuType, location, rotation, coordSys);
		}

		protected Danmaku FireLinear(DanmakuPrefab danmakuType, 
		                                      Vector2 location, 
		                             		  DynamicFloat rotation, 
		                             		  DynamicFloat velocity,
		                                      DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View,
		                                      DanmakuController controller = null,
		                                      DanmakuModifier modifier = null,
		                                      DanmakuGroup group = null) {
			return Field.FireLinear (danmakuType, location, rotation, velocity, coordSys, controller, modifier, group);
		}

		protected Danmaku FireCurved(DanmakuPrefab danmakuType,
	                                    Vector2 location,
	                                    DynamicFloat rotation,
	                                    DynamicFloat velocity,
	                                    DynamicFloat angularVelocity,
	                                    DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View,
	                                    DanmakuController controller = null,
	                                    DanmakuModifier modifier = null,
	                                    DanmakuGroup group = null) {
			return Field.FireCurved (danmakuType, location, rotation, velocity, angularVelocity, coordSys, controller, modifier, group);
		}

		protected Danmaku Fire(FireBuilder data) {
			return Field.Fire (data);
		}
	}
}