//// Copyright (c) 2015 James Liu
////	
//// See the LISCENSE file for copying permission.
//
//using UnityEngine;
//using UnityEditor;
//
//using DanmakU;
//
///// <summary>
///// Custom editor scripts for various components of the DanmakU development kit
///// </summary>
//namespace DanmakU.Editor {
//
//	/// <summary>
//	/// Custom <a href="http://docs.unity3d.com/ScriptReference/Editor.html">Editor</a> for FieldMovementPattern.
//	/// </summary>
//	[CustomEditor(typeof(FieldMovementPattern))]
//	internal class FieldMovementPatternEditor : UnityEditor.Editor {
//
//		/// <summary>
//		/// Creates custom GUI useful for statistics/debug on the Scene View
//		/// Shows how many active Projectiles and how many
//		/// </summary>
//		public void OnSceneGUI() {
//			Texture2D curveTex = new Texture2D (1, 1);
//			curveTex.SetPixel (0, 0, Color.white);
//			FieldMovementPattern fmp = (FieldMovementPattern)target;
//			FieldMovementPattern.AtomicMovement[] movements = fmp.movements;
//			DanmakuField testField = Util.FindClosest<DanmakuField> (fmp.transform.position);
//			if(testField != null && movements != null) {
//				Vector3 currentLocation = fmp.transform.position;
//				for(int i = 0; i < movements.Length; i++) {
//					if(movements[i] != null) {
//						Vector3 nextLocation = movements[i].NextLocation(testField, currentLocation);
//						Vector3 control1 = movements[i].NextControlPoint1(testField, currentLocation);
//						Vector3 control2 = movements[i].NextControlPoint2(testField, currentLocation);
//						Handles.DrawDottedLine(currentLocation, control1, 10f);
//						Handles.DrawWireDisc(control1, Vector3.forward, 1);
//						Handles.DrawDottedLine(control1, control2, 10f);
//						Handles.DrawWireDisc(control2, Vector3.forward, 1f);
//						Handles.DrawDottedLine(control2, nextLocation, 10f);
//						Handles.DrawBezier(currentLocation, nextLocation, control1, control2, Handles.color, curveTex, 1f);
//						currentLocation = nextLocation;
//						Handles.DrawWireDisc(currentLocation, Vector3.forward, 1);
//					}
//				}
//			}
//		}
//	}
//}
