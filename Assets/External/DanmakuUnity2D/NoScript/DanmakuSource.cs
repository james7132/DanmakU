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
using System.Collections.Generic;
using UnityUtilLib;

namespace Danmaku2D {
	
	public class SourcePoint {
		public Vector2 Position;
		public DynamicFloat BaseRotation;

		public SourcePoint(Vector2 location, DynamicFloat rotation) {
			this.Position = location;
			this.BaseRotation = rotation;
		}
	}

	public abstract class DanmakuSource : DanmakuModifier, IDanmakuNode {

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