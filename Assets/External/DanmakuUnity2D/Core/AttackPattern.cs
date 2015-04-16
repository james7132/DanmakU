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
using System.Collections;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {

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

		/// <summary>
		/// Helper method to quickly get the angle needed to directly fire at the player in the AttacKPattern's target field
		/// </summary>
		/// <returns>The angle needed to fire directly toward the player.</returns>
		/// <param name="position">The position to evaluate the angle to the player from.</param>
		/// <param name="coordSys">The cordinate system used to evaluate the true location of the source location</param>
		protected float AngleToPlayer(Vector2 position, DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.World) {
			return Field.AngleTowardPlayer(transform.position, coordSys);
		}

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