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
/// A set of scripts for commonly created Attack Patterns
/// </summary>
namespace Danmaku2D.AttackPatterns {

	/// <summary>
	/// A basic enemy attack pattern. It fires a set of bullets in a fixed pattern at a fixed interval.
	/// </summary>
	[AddComponentMenu("Danmaku 2D/Attack Patterns/Enemy Basic Attack")]
	public class EnemyBasicAttack : AttackPattern {
		
		[SerializeField]
		private FrameCounter fireDelay;

		[SerializeField]
		private DynamicFloat velocity;

		[SerializeField]
		public DynamicFloat angV;

		[SerializeField]
		private DynamicFloat currentDelay;

		[SerializeField]
		private DynamicFloat generalRange;

		[SerializeField]
		private DanmakuPrefab basicPrefab;

		protected override bool IsFinished {
			get {
				return false;
			}
		}
		
		protected override void MainLoop () {
			if (fireDelay.Tick()) {
				float angle = TargetField.AngleTowardPlayer(transform.position) + Random.Range(-generalRange, generalRange);
				FireCurved(basicPrefab, transform.position, angle, velocity, angV, DanmakuField.CoordinateSystem.World);
			}
		}
	}
}