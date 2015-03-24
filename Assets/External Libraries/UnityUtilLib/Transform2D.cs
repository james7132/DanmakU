using UnityEngine;
using System.Collections;

namespace UnityUtilLib {

	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public sealed class Transform2D : CachedObject {

		[SerializeField]
		private int editMode;

		public Vector2 Position {
			get {
				return transform.position;
			}
			set {
				transform.position = value;
			}
		}

		public Vector2 LocalPosition {
			get {
				return transform.localPosition;
			}
			set {
				transform.localPosition = value;
			}
		}

		public Vector2 LossyScale {
			get {
				return transform.lossyScale;
			}
		}

		public Vector2 LocalScale {
			get {
				return transform.localScale;
			}
			set {
				transform.localScale = value;
			}
		}

		public float Rotation {
			get {
				return transform.eulerAngles.z;
			}
			set {
				transform.rotation = Quaternion.Euler(0f, 0f, value);
			}
		}

		public float LocalRotation {
			get {
				return transform.localEulerAngles.z;
			}
			set {
				transform.localEulerAngles = new Vector3(0f, 0f, value);
			}
		}

		public override void Awake () {
			base.Awake ();
			transform.hideFlags = HideFlags.HideInInspector;
		}

		public void OnDestroy() {
			transform.hideFlags = HideFlags.None;
		}

	}
}
