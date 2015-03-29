using UnityEngine;
using System.Collections;
using UnityUtilLib;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {

	/// <summary>
	/// An abstract class to define a movement pattern through world space for the GameObject, to which instances are attached, to move through.
	/// </summary>
	public abstract class MovementPattern : PausableGameObject {

		[SerializeField]
		private bool destroyOnEnd;

		/// <summary>
		/// Whether or not the GameObject this MovementPattern is destroyed at the end of the movement or not.
		/// </summary>
		/// <value><c>true</c> if it is to be destroyed; otherwise, <c>false</c>. <see cref="UnityEngine.Object.Destroy"/> </value>
		public bool DestroyOnEnd {
			get {
				return destroyOnEnd;
			}
			set {
				destroyOnEnd = value;
			}
		}

		/// <summary>
		/// Starts the movement followining the pattern defined by this script.
		/// </summary>
		public void StartMovement() {
			StartCoroutine (MoveImpl ());
		}

		private IEnumerator MoveImpl() {
			yield return StartCoroutine(Move());
			if(destroyOnEnd) {
				Destroy (gameObject);
			}
		}

		/// <summary>
		/// The actual movement coroutine. 
		/// </summary>
		protected abstract IEnumerator Move();

		/// <summary>
		/// A Coroutine used to move the GameObject the instance is attached to moved to the given position in absolute world coordinates within a given time.
		/// Movement is done in a straight line and a constant velocity.
		/// </summary>
		/// <returns>the coroutine IEnumerator</returns>
		/// <param name="end">the end position of the movement</param>
		/// <param name="time">the amount of time the move should take</param>
		protected IEnumerator LinearMove(Vector3 end, float time) {
			float t = 0;
			Vector3 start = transform.position;
			float dt = Util.TargetDeltaTime;
			while (t <= 1f) {
				transform.position = Vector3.Lerp(start, end, t);
				yield return UtilCoroutines.WaitForUnpause(this);
				t += time / dt;
			}
		}

		/// <summary>
		/// A Coroutine used to move the GameObject the instance is attached to moved to the given position in absolute world coordinates within a given time.
		/// Movement is done along a cubic Berzier curve and a constant velocity.
		/// </summary>
		/// <returns>the coroutine IEnumerator</returns>
		/// <param name="end">the end position of the movement</param>
		/// <param name="controlPoint1">the first control point for the curve</param>
		/// <param name="controlPoint2">the second control point for the curve</param>
		/// <param name="time">the amount of time the move should take</param>
		protected IEnumerator BerzierMove(Vector3 end, Vector3 controlPoint1, Vector3 controlPoint2, float time) {
			float t = 0;
			Vector3 start = transform.position;
			float dt = Util.TargetDeltaTime;
			while (t <= 1f) {
				transform.position = Vector3.Lerp(start, end, t);
				yield return UtilCoroutines.WaitForUnpause(this);
				t += time / dt;
			}
		}
	}
}