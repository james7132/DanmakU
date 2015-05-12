// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;


namespace DanmakU.Modifiers {

	public class CircleSource : DanmakuSource {

		[Serialize, Show]
		public DynamicInt Count {
			get;
			set;
		}
		
		[Serialize, Show]
		public DynamicFloat Radius {
			get;
			set;
		}
		
		[Serialize, Show]
		public bool RaidalDirection {
			get;
			set;
		}

		#region implemented abstract members of ProjectileSource

		protected override void UpdateSourcePoints (Vector2 position, float rotation) {
			sourcePoints.Clear ();
			float delta = Util.TwoPI / Count;
			for (int i = 0; i < Count; i++) {
				float currentRotation = Mathf.Deg2Rad * rotation + i * delta;
				SourcePoint sourcePoint = new SourcePoint(position + Radius  * Util.OnUnitCircleRadians(currentRotation),
				                                          ((RaidalDirection) ? Mathf.Rad2Deg * currentRotation - 90f : rotation));
				sourcePoints.Add(sourcePoint);
			}
		}

		#endregion
	}
}