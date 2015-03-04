using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {
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

		protected IEnumerator LinearMove(Vector3 end, float time) {
			float t = 0;
			Vector3 start = Transform.position;
			float dt = Util.TargetDeltaTime;
			while (t <= 1f) {
				Transform.position = Vector3.Lerp(start, end, t);
				yield return UtilCoroutines.WaitForUnpause(this);
				t += time / dt;
			}
		}

		protected IEnumerator BerzierMove(Vector3 end, Vector3 controlPoint1, Vector3 controlPoint2, float time) {
			float t = 0;
			Vector3 start = Transform.position;
			float dt = Util.TargetDeltaTime;
			while (t <= 1f) {
				Transform.position = Vector3.Lerp(start, end, t);
				yield return UtilCoroutines.WaitForUnpause(this);
				t += time / dt;
			}
		}
	}
}