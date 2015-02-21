using UnityEngine;
using System;
using System.Collections;
using UnityUtilLib;

/// <summary>
/// Field movement pattern.
/// </summary>
public class FieldMovementPattern : AbstractMovementPattern {

	/// <summary>
	/// The field.
	/// </summary>
	[SerializeField]
	private AbstractDanmakuField field;

	/// <summary>
	/// The test start point.
	/// </summary>
	private Vector2 testStartPoint;

	/// <summary>
	/// The test invert y.
	/// </summary>
	private bool testInvertY;

	/// <summary>
	/// The test invert x.
	/// </summary>
	private bool testInvertX;

	//TODO: Document Comment
	[Serializable]
	public class AtomicMovement {

		/// <summary>
		/// The time.
		/// </summary>
		public float time;

		/// <summary>
		/// The target location.
		/// </summary>
		public Vector2 targetLocation;

		/// <summary>
		/// The curve control point1.
		/// </summary>
		public Vector2 curveControlPoint1;

		/// <summary>
		/// The curve control point2.
		/// </summary>
		public Vector2 curveControlPoint2;

		/// <summary>
		/// Nexts the location.
		/// </summary>
		/// <returns>The location.</returns>
		/// <param name="field">Field.</param>
		/// <param name="startLocation">Start location.</param>
		public Vector3 NextLocation(AbstractDanmakuField field, Vector3 startLocation) {
			return Interpret (targetLocation, field, startLocation);
		}

		/// <summary>
		/// Nexts the control point1.
		/// </summary>
		/// <returns>The control point1.</returns>
		/// <param name="field">Field.</param>
		/// <param name="startLocation">Start location.</param>
		public Vector3 NextControlPoint1(AbstractDanmakuField field, Vector3 startLocation) {
			return Interpret (curveControlPoint1, field, startLocation);
		}

		/// <summary>
		/// Nexts the control point2.
		/// </summary>
		/// <returns>The control point2.</returns>
		/// <param name="field">Field.</param>
		/// <param name="startLocation">Start location.</param>
		public Vector3 NextControlPoint2(AbstractDanmakuField field, Vector3 startLocation) {
			return Interpret (curveControlPoint2, field, startLocation);
		}

		/// <summary>
		/// Interpret the specified loc, field and startLocation.
		/// </summary>
		/// <param name="loc">Location.</param>
		/// <param name="field">Field.</param>
		/// <param name="startLocation">Start location.</param>
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
				float totalTime = movements[i].time;
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
