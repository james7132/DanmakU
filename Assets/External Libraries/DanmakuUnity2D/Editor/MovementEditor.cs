using System;
using UnityEngine;
using UnityEditor;
using UnityUtilLib;
using Danmaku2D;

[CustomEditor(typeof(FieldMovementPattern))]
public class MovementEditor : Editor {

	void OnSceneGUI() {
		
		Texture2D curveTex = new Texture2D (1, 1);
		curveTex.SetPixel (0, 0, Color.white);
		FieldMovementPattern fmp = (FieldMovementPattern)target;
		FieldMovementPattern.AtomicMovement[] movements = fmp.movements;
		DanmakuField testField = Util.FindClosest<DanmakuField> (fmp.Transform.position);
		if(testField != null) {
			Vector3 currentLocation = fmp.Transform.position;
			for(int i = 0; i < movements.Length; i++) {
				if(movements[i] != null) {
					Vector3 nextLocation = movements[i].NextLocation(testField, currentLocation);
					Vector3 control1 = movements[i].NextControlPoint1(testField, currentLocation);
					Vector3 control2 = movements[i].NextControlPoint2(testField, currentLocation);
					Handles.DrawDottedLine(currentLocation, control1, 10f);
					Handles.DrawWireDisc(control1, Vector3.forward, 1);
					Handles.DrawDottedLine(control1, control2, 10f);
					Handles.DrawWireDisc(control2, Vector3.forward, 1f);
					Handles.DrawDottedLine(control2, nextLocation, 10f);
					Handles.DrawBezier(currentLocation, nextLocation, control1, control2, Handles.color, curveTex, 1f);
					currentLocation = nextLocation;
					Handles.DrawWireDisc(currentLocation, Vector3.forward, 1);
				}
			}
		}
	}
}
