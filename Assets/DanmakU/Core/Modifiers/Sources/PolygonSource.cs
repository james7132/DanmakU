// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Modifiers {

	[System.Serializable]
	public class PolygonSource : DanmakuSource {

		public enum RotationType { None, Normal, Tangential, Radial }

		[Serialize, Show]
		public DynamicFloat Size {
			get;
			set;
		}
		
		[Serialize, Show]
		public DynamicInt EdgeCount {
			get;
			set;
		}

		[Serialize, Show]
		public DynamicInt PointsPerEdge {
			get;
			set;
		}

		[Serialize, Show]
		public RotationType Rotation {
			get;
			set;
		}

		[Serialize, Show]
		private float rotationOffset;

		#region implemented abstract members of DanmakuSource
		protected override void UpdateSourcePoints (Vector2 position, float rotation) {
			sourcePoints.Clear ();
			int edge = 0;
			float edgeRot = 90f + rotation;
			float edgeDelta = 360f / EdgeCount;
			Vector2 current = position + Size * Util.OnUnitCircle (edgeRot);
			Vector2 next = position + Size * Util.OnUnitCircle (edgeRot + edgeDelta);
			edgeRot += edgeDelta;
			while(edge <= EdgeCount) {
				Vector2 diff = (next - current) / (PointsPerEdge);
				float rot = rotation;
				switch(Rotation) {
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
				for(int i = 0; i < PointsPerEdge; i++) {
					Vector2 currentPos = current + i * diff;
					if(Rotation == RotationType.Radial) {
						rot = DanmakuUtil.AngleBetween2D(position, currentPos);
					}
					sourcePoints.Add(new SourcePoint(currentPos, rot));
				}
				edge++;
				edgeRot += edgeDelta;
				current = next;
				next = position + Size * Util.OnUnitCircle(edgeRot);
			}
		}
		#endregion
	}
}
