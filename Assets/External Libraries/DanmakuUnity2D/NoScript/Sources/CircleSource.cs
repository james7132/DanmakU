using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D.NoScript {

	internal class CircleSource : ProjectileSource {

		#pragma warning disable 0649
		public int count;
		public float radius;
		public bool raidalDirection;
		#pragma warning restore 0649

		private int oldCount;

		#region implemented abstract members of ProjectileSource

		protected override void UpdateSourcePoints () {
			if (sourcePoints == null || count != oldCount) {
				sourcePoints = new SourcePoint[count];
				oldCount = count;
			}
			float delta = Util.TwoPI / count;
			float rotation = transform.rotation.eulerAngles.z;
			for (int i = 0; i < count; i++) {
				float currentRotation = Util.Degree2Rad * rotation + i * delta;
				sourcePoints[i].Location = ((Vector2)transform.position) + radius  * Util.OnUnitCircleRadians(currentRotation);
				sourcePoints[i].BaseRotation = (raidalDirection) ? Util.Rad2Degree * currentRotation + 90f : rotation;
			}
		}

		#endregion
	}
}