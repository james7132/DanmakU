using UnityEngine;
using System;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {
	public class FieldMovementPattern : AbstractMovementPattern {

		[SerializeField]
		private AbstractDanmakuField field;

		[Serializable]
		public class AtomicMovement {
			[SerializeField]
			private float time;
			public float Time {
				get {
					return time;
				}
				set {
					time = value;
				}
			}

			[SerializeField]
			private Vector2 targetLocation;
			[SerializeField]
			private Vector2 curveControlPoint1;
			[SerializeField]
			private Vector2 curveControlPoint2;

			public Vector3 NextLocation(AbstractDanmakuField field, Vector3 startLocation) {
				return Interpret (targetLocation, field, startLocation);
			}

			public Vector3 NextControlPoint1(AbstractDanmakuField field, Vector3 startLocation) {
				return Interpret (curveControlPoint1, field, startLocation);
			}

			public Vector3 NextControlPoint2(AbstractDanmakuField field, Vector3 startLocation) {
				return Interpret (curveControlPoint2, field, startLocation);
			}

			private Vector3 Interpret(Vector2 loc, AbstractDanmakuField field, Vector3 startLocation) {	
				Vector3 nextLocation = Util.To3D(loc);
				return startLocation + field.Relative2Absolute(nextLocation);
			}
		}

		/// <summary>
		/// The movements.
		/// </summary>
		public AtomicMovement[] movements;

		public override void Awake () {
			base.Awake ();
			if (field == null) {
				field = Util.FindClosest<AbstractDanmakuField>(Transform.position);
			}
		}

		/// <summary>
		/// Move this instance.
		/// </summary>
		protected override IEnumerator Move() {
			for(int i = 0; i < movements.Length; i++) {
				if(movements[i] != null) {
					float totalTime = movements[i].Time;
					float t = 0f;
					Vector3 startLocation = Transform.position;
					Vector3 targetLocation = movements[i].NextLocation(field, startLocation);
					Vector3 control1 = movements[i].NextControlPoint1(field, startLocation);
					Vector3 control2 = movements[i].NextControlPoint2(field, startLocation);
					Vector3 oldPosition;
					while(t < 1f) {
						oldPosition = Transform.position;
						Transform.position = Util.BerzierCurveVectorLerp(startLocation, targetLocation, control1, control2, t);
						Transform.rotation = Util.RotationBetween2D(oldPosition, Transform.position);
						yield return new WaitForFixedUpdate();
						t += Time.deltaTime / totalTime;
					}
				}
			}
		}
	}
}