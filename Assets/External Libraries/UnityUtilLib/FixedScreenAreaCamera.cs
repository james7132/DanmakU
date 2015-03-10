using UnityEngine;

namespace UnityUtilLib {

	/// <summary>
	/// Resolves aspect ratio issues when dealing with orthographic cameras that need to maintain a specific aspect ratio for proper game functionality
	/// Fixes aspect ratio of the camera even when the aspect ratio of the screen is changed.
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class FixedScreenAreaCamera : MonoBehaviour {

		private Camera fixedCamera;
		private float currentAspectRatio;
		private float offset;

		/// <summary>
		/// The anchor point the camera moves relative to when rescaling
		/// </summary>
		[SerializeField]
		private float anchorPoint = 0.5f;

		/// <summary>
		/// The native aspect ratio the camera is designed to be used with
		/// </summary>
		[SerializeField]
		private float nativeAspectRatio;

		/// <summary>
		/// The bounds of the camera is to be used with while the screen is in the native aspect ratio
		/// </summary>
		[SerializeField]
		private Rect nativeBounds;

		/// <summary>
		/// Called on instantiation
		/// Used for initialization
		/// </summary>
		void Awake() {
			fixedCamera = GetComponent<Camera> ();
			currentAspectRatio = (float)Screen.width / (float)Screen.height;
			offset = nativeBounds.x + 0.5f * nativeBounds.width - anchorPoint;
			Resize ();
		}

		/// <summary>
		/// Called every frame
		/// </summary>
		void Update() {
			float newAspectRatio = (float)Screen.width / (float)Screen.height;
			if (currentAspectRatio != newAspectRatio) {
				currentAspectRatio = newAspectRatio;
				Resize ();
			}
		}

		/// <summary>
		/// Resizes and moves the camera as per defined values
		/// </summary>
		private void Resize() {
			float changeRatio = nativeAspectRatio / currentAspectRatio;
			float targetWidth = changeRatio * nativeBounds.width;
			float center = anchorPoint + changeRatio * offset;
			Rect cameraRect = fixedCamera.rect;
			cameraRect.x = center - targetWidth / 2;
			cameraRect.width = targetWidth;
			fixedCamera.rect = cameraRect;
		}
	}
}