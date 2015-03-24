using UnityEngine;
using System.Collections;

namespace Danmaku2D.NoScript {

	public class StreamEmitter : ProjectileEmitter {

		public ProjectilePrefab prefab;
		public float rotationOffset;
		public ProjectileControlBehavior controller;

		#region implemented abstract members of ProjectileEmitter
		protected override void FireFromSource (SourcePoint source) {
			TargetField.FireControlledProjectile (prefab,
			                                     source.Location,
			                                     source.BaseRotation + rotationOffset,
			                                     controller,
			                                     DanmakuField.CoordinateSystem.World);
		}
		#endregion
		
	}
}