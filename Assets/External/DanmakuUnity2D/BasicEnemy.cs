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
using System.Collections;
using UnityUtilLib;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {

	/// <summary>
	/// A basic enemy that is usually used as cannon fodder in Danmaku games.
	/// </summary>
	[RequireComponent(typeof(MovementPattern))]
	public class BasicEnemy : Enemy {

		[SerializeField]
		private float maxHealth;
		private float currentHealth;

		/// <summary>
		/// Gets the current health. 
		/// </summary>
		/// <value>The current health.</value>
		public float CurrentHealth {
			get {
				return currentHealth;
			}
		}

		private MovementPattern movementPattern;

		private AttackPattern attackPattern;
		/// <summary>
		/// Gets the current attack pattern.
		/// </summary>
		/// <value>The current attack pattern.</value>
		public override AttackPattern CurrentAttackPattern {
			get {
				return attackPattern;
			}
		}

		public override void Awake() {
			base.Awake ();
			currentHealth = maxHealth;
			movementPattern = GetComponent<MovementPattern> ();
			movementPattern.DestroyOnEnd = true;
			attackPattern = GetComponent<AttackPattern> ();
			if (attackPattern != null) {
				attackPattern.TargetField = Field.TargetField;
				attackPattern.Fire();
			}
			movementPattern.StartMovement ();
		}

		protected override void Damage (float damage) {
			currentHealth -= damage;
			//print (currentHealth);
		}
		
		public override bool IsDead {
			get {
				return currentHealth <= 0;
			}
		}
	}
}
