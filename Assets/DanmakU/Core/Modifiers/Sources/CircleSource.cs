// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;


namespace DanmakU.NoScript {

	public class CircleSource : DanmakuSource {

		#pragma warning disable 0649
		public int count;
		public float radius;
		public bool raidalDirection;
		#pragma warning restore 0649

		#region implemented abstract members of ProjectileSource

		protected override void UpdateSourcePoints (Vector2 position, float rotation) {
			sourcePoints.Clear ();
			float delta = Util.TwoPI / count;
			for (int i = 0; i < count; i++) {
				float currentRotation = Mathf.Deg2Rad * rotation + i * delta;
				SourcePoint sourcePoint = new SourcePoint(position + radius  * Util.OnUnitCircleRadians(currentRotation),
				                                          ((raidalDirection) ? Mathf.Rad2Deg * currentRotation - 90f : rotation));
				sourcePoints.Add(sourcePoint);
			}
		}

		#endregion
	}
}