using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D.AttackPatterns {

	[AddComponentMenu("Danmaku 2D/Attack Patterns/Enemy Basic Attack")]
	public class EnemyBasicAttack : AttackPattern {
		
		[SerializeField]
		private FrameCounter fireDelay;

		[SerializeField]
		private float velocity;

		[SerializeField]
		public float angV;

		[SerializeField]
		private float currentDelay;

		[SerializeField]
		private float generalRange;

		[SerializeField]
		private ProjectilePrefab basicPrefab;

		protected override bool IsFinished {
			get {
				return false;
			}
		}
		
		protected override void MainLoop () {
			if (fireDelay.Tick()) {
				float angle = TargetField.AngleTowardPlayer(transform.position) + Random.Range(-generalRange, generalRange);
				FireCurvedBullet(basicPrefab, Transform.position, angle, velocity, angV, DanmakuField.CoordinateSystem.World);
			}
		}
	}
}