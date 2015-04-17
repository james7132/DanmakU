// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;
using UnityUtilLib;

/// <summary>
/// A set of scripts for commonly created Attack Patterns
/// </summary>
namespace DanmakU.AttackPatterns {

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
		
		protected override IEnumerator MainLoop () {
			while (true) {
//				yield return WaitForFrames(fireDelay.MaxCount);
				float angle = Field.AngleTowardPlayer(transform.position) + Random.Range(-generalRange, generalRange);
				FireCurved(basicPrefab, transform.position, angle, velocity, angV, DanmakuField.CoordinateSystem.World);
			}
		}
	}
}