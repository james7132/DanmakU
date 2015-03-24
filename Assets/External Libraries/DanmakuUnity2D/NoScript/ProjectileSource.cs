using UnityEngine;
using System.Collections.Generic;
using UnityUtilLib;

namespace Danmaku2D {
	
	public struct SourcePoint {
		public Vector2 Location;
		public float BaseRotation;
		
		public SourcePoint(Vector2 location, float rotation) {
			this.Location = location;
			this.BaseRotation = rotation;
		}
	}

	public abstract class ProjectileSource : CachedObject {

		[System.NonSerialized]
		public DanmakuField TargetField;

		protected SourcePoint[] sourcePoints;

		protected abstract void UpdateSourcePoints ();

		public override void Awake () {
			base.Awake ();
			TargetField = Util.FindClosest<DanmakuField> (transform.position);
		}

		public SourcePoint[] SourcePoints {
			get {
				UpdateSourcePoints();
				return sourcePoints;
			}
		}

		void OnDrawGizmos() {
			UpdateSourcePoints ();
			for(int i = 0; i < sourcePoints.Length; i++) {
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(sourcePoints[i].Location, 1f);
				Gizmos.color = Color.red;
				Vector3 endRay = sourcePoints[i].Location + 5 * Util.OnUnitCircle(sourcePoints[i].BaseRotation + 90f).normalized;
				Gizmos.DrawLine(sourcePoints[i].Location, endRay);
			}
		}
	}
	
}