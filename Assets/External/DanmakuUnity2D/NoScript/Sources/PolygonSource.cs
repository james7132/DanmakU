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
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D.NoScript {
	
	[AddComponentMenu("Danmaku 2D/Sources/Polygon Source")]
	public class PolygonSource : DanmakuSource {

		private enum RotationType { None, Normal, Tangential, Radial }

		[SerializeField]
		private float size;

		[SerializeField]
		private int edgeCount;

		[SerializeField]
		private int pointsPerEdge;

		[SerializeField]
		private RotationType rotationType;

		[SerializeField]
		private float rotationOffset;

		#region implemented abstract members of DanmakuSource
		protected override void UpdateSourcePoints (Vector2 position, float rotation) {
			sourcePoints.Clear ();
			int edge = 0;
			float edgeRot = 90f;
			float edgeDelta = 360f / edgeCount;
			Vector2 current = position + size * Util.OnUnitCircle (edgeRot);
			Vector2 next = position + size * Util.OnUnitCircle (edgeRot + edgeDelta);
			edgeRot += edgeDelta;
			while(edge <= edgeCount) {
				Vector2 diff = (next - current) / (pointsPerEdge);
				float rot = rotation;
				switch(rotationType) {
					case RotationType.None:
					case RotationType.Radial:
						break;
					case RotationType.Tangential:
						rot = edgeRot - 0.5f * edgeDelta;
						break;
					case RotationType.Normal:
						rot = edgeRot - 90f - 0.5f * edgeDelta;
						break;
				}
				for(int i = 0; i < pointsPerEdge; i++) {
					Vector2 currentPos = current + i * diff;
					if(rotationType == RotationType.Radial) {
						rot = Util.AngleBetween2D(position, currentPos);
					}
					sourcePoints.Add(new SourcePoint(currentPos, rot));
				}
				edge++;
				edgeRot += edgeDelta;
				current = next;
				next = position + size * Util.OnUnitCircle(edgeRot);
			}
		}
		#endregion
	}
}
