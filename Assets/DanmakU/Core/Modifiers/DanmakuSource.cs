// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections.Generic;
using UnityUtilLib;

namespace DanmakU {
	
	public class SourcePoint {
		public Vector2 Position;
		public DynamicFloat BaseRotation;
		
		public SourcePoint(Vector2 location, DynamicFloat rotation) {
			this.Position = location;
			this.BaseRotation = rotation;
		}
	}
	
	public abstract class DanmakuSource : DanmakuModifier {
		
		protected List<SourcePoint> sourcePoints;
		
		protected abstract void UpdateSourcePoints (Vector2 position, float rotation);
		
		public sealed override void Fire (Vector2 position, DynamicFloat rotation) {
			if (sourcePoints == null)
				sourcePoints = new List<SourcePoint> ();
			UpdateSourcePoints (position, rotation);
			for(int i = 0; i < sourcePoints.Count; i++) {
				FireSingle(sourcePoints[i].Position, sourcePoints[i].BaseRotation);
			}
		}
		
		void DrawGizmos() {
			if (sourcePoints == null) { 
				sourcePoints = new List<SourcePoint> ();
			}
			//			UpdatePoints(transform.position, transform.rotation.eulerAngles.z);
			for(int i = 0; i < sourcePoints.Count; i++) {
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(sourcePoints[i].Position, 1f);
				Gizmos.color = Color.red;
				Vector3 endRay = sourcePoints[i].Position + 5 * Util.OnUnitCircle(sourcePoints[i].BaseRotation + 90f).normalized;
				Gizmos.DrawLine(sourcePoints[i].Position, endRay);
			}
		}
	}
	
}