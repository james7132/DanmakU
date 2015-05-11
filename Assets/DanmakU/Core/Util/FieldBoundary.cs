// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU {

	[ExecuteInEditMode]
	public sealed class FieldBoundary : MonoBehaviour {

		private enum Edge { Top = 0, Bottom = 1, Left = 2, Right = 3}
		
		private static Vector2[] fixedPoints = new Vector2[] {
			new Vector2 (0, 1f),
			new Vector2 (0, -1f),
			new Vector2 (-1f, 0f),
			new Vector2 (1f, 0f)
		};
		
		[SerializeField]
		private DanmakuField field;
		
		[SerializeField]
		private Edge location;
		
		[SerializeField]
		private float bufferRatio = 0.1f;

		[SerializeField]
		private float hangoverRatio = 0f;
		
		[SerializeField]
		private float spaceRatio = 0;
		
		private BoxCollider2D boundary;
		private Bounds2D oldBounds;
		private Bounds2D newBounds;
		
		void Awake () {
			boundary = GetComponent<BoxCollider2D> ();
			if (field == null) {
				print("No field provided, searching in ancestor GameObjects...");
				field = GetComponentInParent<DanmakuField>();
			}
			if (field == null) {
				Debug.LogError ("Field Boundary without a DanmakuField");
			} else {
				UpdatePosition ();
			}
		}
		
		void Update () {
			if (field != null && field.MovementBounds != oldBounds) {
				UpdatePosition ();
			}
		}

		void OnDrawGizmos() {
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube (boundary.bounds.center, boundary.bounds.size);
		}
		
		private void UpdatePosition() {
			oldBounds = field.MovementBounds;

			float size = oldBounds.Size.Max();
			Vector2 newPosition = (Vector2)oldBounds.Center + fixedPoints [(int)location].Hadamard2(oldBounds.Extents);
			float buffer = bufferRatio * size;
			float space = spaceRatio * size;
			float hangover = hangoverRatio * size;
			
			Vector2 area = boundary.size;
			switch(location) {
			case Edge.Top:
			case Edge.Bottom:
				area.y = buffer;
				area.x = oldBounds.Size.x + hangover;
				break;
			case Edge.Left:
			case Edge.Right:
				area.x = buffer;
				area.y = oldBounds.Size.y + hangover;
				break;
			}
			boundary.size = area;

			oldBounds = boundary.bounds;
			switch(location) {
			case Edge.Top:
				newPosition.y += oldBounds.Extents.y + space;
				break;
			case Edge.Bottom:
				newPosition.y -= oldBounds.Extents.y + space;
				break;
			case Edge.Left:
				newPosition.x -= oldBounds.Extents.x + space;
				break;
			case Edge.Right:
				newPosition.x += oldBounds.Extents.x + space;
				break;
			}

			transform.position = newPosition;
		}
	}
}