using UnityEngine;
using System.Collections;

public class ProjectileGroup {
	private Camera targetCamera;
	private Material projectileMaterial;
	private int layer = 0;

	private Vector2 boxOffset;
	private Vector2 boxSize;

	private Vector2 circleOffset;
	private float circleRadius;

	public Camera TargetCamera {
		get {
			return targetCamera;
		}
		set {
			targetCamera = value;
		}
	}

	public Material ProjectileMaterial {
		get {
			return projectileMaterial;
		}
		set {
			projectileMaterial = value;
		}
	}

	public int Layer {
		get {
			return layer;
		}
		set {
			layer = value;
		}
	}

	public Vector2 BoxOffset {
		get {
			return boxOffset;
		}
		set {
			boxOffset = value;
		}
	}

	public Vector2 BoxSize {
		get {
			return boxSize;
		}
		set {
			boxSize = value;
		}
	}

	public Vector2 CircleOffset {
		get {
			return circleOffset;
		}
		set {
			circleOffset = value;
		}
	}

	public float CircleRadius {
		get {
			return circleRadius;
		}
		set {
			circleRadius = value;
		}
	}
}
