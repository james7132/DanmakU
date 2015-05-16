// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections.Generic;


namespace DanmakU.Modifiers {
	
	public class SourcePoint {
		public Vector2 Position;
		public DynamicFloat BaseRotation;
		
		public SourcePoint(Vector2 location, DynamicFloat rotation) {
			this.Position = location;
			this.BaseRotation = rotation;
		}
	}

	[System.Serializable]
	public abstract class DanmakuSource : DanmakuModifier {
		
		protected List<SourcePoint> SourcePoints;

		public DanmakuSource() {
			SourcePoints = new List<SourcePoint>();
		}
		
		protected abstract void UpdateSourcePoints (Vector2 position, float rotation);
		
		public sealed override void OnFire (Vector2 position, DynamicFloat rotation) {
			UpdateSourcePoints (position, rotation);
			for(int i = 0; i < SourcePoints.Count; i++) {
				FireSingle(SourcePoints[i].Position, SourcePoints[i].BaseRotation);
			}
		}
		
		void DrawGizmos() {
			//			UpdatePoints(transform.position, transform.rotation.eulerAngles.z);
			for(int i = 0; i < SourcePoints.Count; i++) {
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(SourcePoints[i].Position, 1f);
				Gizmos.color = Color.red;
				Vector3 endRay = SourcePoints[i].Position + 5 * Util.OnUnitCircle(SourcePoints[i].BaseRotation + 90f).normalized;
				Gizmos.DrawLine(SourcePoints[i].Position, endRay);
			}
		}
	}
	
}