using UnityEngine;
using System.Collections;

namespace Danmaku2D.NoScript {

	internal sealed class BurstEmitter : ProjectileEmitter {

		#pragma warning disable 0649
		public ProjectilePrefab prefab;
		public float rotationOffset = 0f;
		public float rotationRange = 360f;
		public int count = 5;
		public int depth = 1;
		public ProjectileControlBehavior controller;
		#pragma warning restore 0649

		private IProjectileController BurstController(int depth) {
			return controller;
		}

		#region implemented abstract members of ProjectileEmitter

		protected override void FireFromSource (SourcePoint source) {
			TargetField.SpawnBurst (prefab,
			                       source.Location,
			                       source.BaseRotation + rotationOffset,
			                       rotationRange,
			                       count,
			                       null,
			                       depth,
			                       BurstController,
			                       DanmakuField.CoordinateSystem.World);
		}

		#endregion




	}
}