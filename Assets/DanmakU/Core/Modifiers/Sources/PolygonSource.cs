// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Modifiers {

	[System.Serializable]
	public class PolygonSource : DanmakuSource {

		public enum RotationType { None, Normal, Tangential, Radial }
		
		[SerializeField, Show]
		private DynamicFloat size;
		public DynamicFloat Size {
			get {
				return size;
			}
			set {
				size = value;
			}
		}
		
		[SerializeField, Show]
		private DynamicInt edgeCount;
		public DynamicInt EdgeCount {
			get {
				return edgeCount;
			}
			set {
				edgeCount = value;
			}
		}
		
		[SerializeField, Show]
		private DynamicInt pointsPerEdge;
		public DynamicInt PointsPerEdge {
			get {
				return pointsPerEdge;
			}
			set {
				pointsPerEdge = value;
			}
		}
		
		[SerializeField, Show]
		private RotationType type;
		public RotationType Type {
			get {
				return type;
			}
			set {
				type = value;
			}
		}

		public PolygonSource(DynamicFloat size, 
		                     DynamicInt edgeCount, 
		                     DynamicInt pointsPerEdge, 
		                     RotationType type) {
			this.size = size;
			this.edgeCount = edgeCount;
			this.pointsPerEdge = pointsPerEdge;
			this.type = type;
		}

		#region implemented abstract members of DanmakuSource
		protected override void UpdateSourcePoints (Vector2 position, float rotation) {
			SourcePoints.Clear ();
			int edge = 0;
			float edgeRot = 90f + rotation;
			float edgeDelta = 360f / EdgeCount;
			Vector2 current = position + Size * Util.OnUnitCircle (edgeRot);
			Vector2 next = position + Size * Util.OnUnitCircle (edgeRot + edgeDelta);
			edgeRot += edgeDelta;
			while(edge <= edgeCount) {
				Vector2 diff = (next - current) / (pointsPerEdge);
				float rot = rotation;
				switch(type) {
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
					if(Type == RotationType.Radial) {
						rot = DanmakuUtil.AngleBetween2D(position, currentPos);
					}
					SourcePoints.Add(new SourcePoint(currentPos, rot));
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
