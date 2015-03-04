using UnityEngine;
using System.Collections;

/// <summary>
/// Screen boundary.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class ScreenBoundary : UnityUtilLib.CachedObject {

	/// <summary>
	/// The fixed points.
	/// </summary>
	public enum Edge { Top = 0, Bottom = 1, Left = 2, Right = 3}

	private static Vector2[] fixedPoints = new Vector2[] {
				new Vector2 (0.5f, 1f),
				new Vector2 (0.5f, 0f),
				new Vector2 (0f, 0.5f),
				new Vector2 (1f, 0.5f)
		};

	/// <summary>
	/// The reference camera.
	/// </summary>
	public Camera referenceCamera;

	/// <summary>
	/// The location.
	/// </summary>
	public Edge location;

	/// <summary>
	/// The camera size buffer ratio.
	/// </summary>
	public float cameraSizeBufferRatio;

	/// <summary>
	/// The camera size space ratio.
	/// </summary>
	public float cameraSizeSpaceRatio;

	/// <summary>
	/// The constantly update.
	/// </summary>
	public bool constantlyUpdate = true;

	/// <summary>
	/// The boundary.
	/// </summary>
	private BoxCollider2D boundary;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	public override void Awake () {
		base.Awake ();
		boundary = GetComponent<BoxCollider2D> ();
		UpdatePosition ();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () {
		if(constantlyUpdate) {
			UpdatePosition();
		}
	}

	/// <summary>
	/// Updates the position.
	/// </summary>
	private void UpdatePosition() {
		float cameraSize = referenceCamera.orthographicSize;
		Vector2 fixedPoint = fixedPoints [(int)location];
		Vector3 viewportPoint = new Vector3 (fixedPoint.x, fixedPoint.y, 0f);
		Vector2 screenSize = new Vector2 (1f, (float)((double)Screen.width / (double)Screen.height)) * cameraSize * 2.0f;
		Vector3 newPosition = referenceCamera.ViewportToWorldPoint (viewportPoint);
		float space = cameraSizeSpaceRatio * cameraSize;;
			
		Vector2 area = boundary.size;
		switch(location) {
			case Edge.Top:
			case Edge.Bottom:
				area.y = cameraSizeBufferRatio * cameraSize;
				area.x = screenSize.x + 2 * space;
				break;
			case Edge.Left:
			case Edge.Right:
				area.x = cameraSizeBufferRatio * cameraSize;
				area.y = screenSize.y + 2 * space;
				break;
		}
		boundary.size = area;
		
		Bounds oldBounds = boundary.bounds;
		switch(location) {
			case Edge.Top:
				newPosition.y += oldBounds.extents.y + space;
				break;
			case Edge.Bottom:
				newPosition.y -= oldBounds.extents.y + space;
				break;
			case Edge.Left:
				newPosition.x -= oldBounds.extents.x + space;
				break;
			case Edge.Right:
				newPosition.x += oldBounds.extents.x + space;
				break;
		}
		
		newPosition.z = Transform.position.z;
		Transform.position = newPosition;
	}
}
