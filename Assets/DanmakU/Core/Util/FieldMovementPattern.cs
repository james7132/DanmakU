//// Copyright (c) 2015 James Liu
////	
//// See the LISCENSE file for copying permission.
//
//using UnityEngine;
//
//using System.Collections;
//
///// <summary>
///// A development kit for quick development of 2D Danmaku games
///// </summary>
//namespace DanmakU {
//	public class FieldMovementPattern : MovementPattern {
//
//		[SerializeField]
//		private DanmakuField field;
//
//		[System.Serializable]
//		public class AtomicMovement {
//			[SerializeField]
//			private float time;
//			public float Time {
//				get {
//					return time;
//				}
//				set {
//					time = value;
//				}
//			}
//
//			[SerializeField]
//			private Vector2 targetLocation;
//			[SerializeField]
//			private Vector2 curveControlPoint1;
//			[SerializeField]
//			private Vector2 curveControlPoint2;
//
//			public Vector3 NextLocation(DanmakuField field, Vector3 startLocation) {
//				return Interpret (targetLocation, field, startLocation);
//			}
//
//			public Vector3 NextControlPoint1(DanmakuField field, Vector3 startLocation) {
//				return Interpret (curveControlPoint1, field, startLocation);
//			}
//
//			public Vector3 NextControlPoint2(DanmakuField field, Vector3 startLocation) {
//				return Interpret (curveControlPoint2, field, startLocation);
//			}
//
//			private Vector3 Interpret(Vector2 loc, DanmakuField field, Vector3 startLocation) {	
//				Vector3 nextLocation = loc;
//				if (field == null)
//					field = DanmakuField.FindClosest(startLocation);
//				return startLocation + (Vector3)field.WorldPoint(nextLocation, DanmakuField.CoordinateSystem.ViewRelative);
//			}
//		}
//
//		/// <summary>
//		/// The movements.
//		/// </summary>
//		public AtomicMovement[] movements;
//
//		void Awake () {
//			base.Awake ();
//			if (field == null) {
//				field = DanmakuField.FindClosest(this);
//			}
//		}
//
//		/// <summary>
//		/// Move this instance.
//		/// </summary>
//		protected override IEnumerator Move() {
//			for(int i = 0; i < movements.Length; i++) {
//				if(movements[i] != null) {
//					float totalTime = movements[i].Time;
//					float t = 0f;
//					Vector3 startLocation = transform.position;
//					Vector3 targetLocation = movements[i].NextLocation(field, startLocation);
//					Vector3 control1 = movements[i].NextControlPoint1(field, startLocation);
//					Vector3 control2 = movements[i].NextControlPoint2(field, startLocation);
//					Vector3 oldPosition;
//					float dt = Util.DeltaTime;
//					while(t < 1f) {
//						oldPosition = transform.position;
//						transform.position = Util.BerzierCurveVectorLerp(startLocation, targetLocation, control1, control2, t);
//						transform.rotation = DanmakuUtil.RotationBetween2D(oldPosition, transform.position);
//						yield return UtilCoroutines.WaitForUnpause(this);
//						t += dt / totalTime;
//					}
//				}
//			}
//		}
//	}
//}