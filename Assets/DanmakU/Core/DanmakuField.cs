// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

using System.Collections.Generic;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace DanmakU {

	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("DanmakU/Danmaku Field")]
	public class DanmakuField : MonoBehaviour {

		internal static List<DanmakuField> fields;

		/// <summary>
		/// Finds the closest DanmakuField to a GameObject's position.
		/// </summary>
		/// <returns>The closest DanmakuField to the GameObject.</returns>
		/// <param name="gameObject">the GameObject used to find the closest DanmakuField.</param>
		public static DanmakuField FindClosest(GameObject gameObject) {
			return FindClosest (gameObject.transform.position);
		}

		/// <summary>
		/// Finds the closest DanmakuField to a Component's GameObject's position.
		/// </summary>
		/// <returns>The closest DanmakuField to the Component and it's GameObject.</returns>
		/// <param name="gameObject">the Component used to find the closest DanmakuField.</param>
		public static DanmakuField FindClosest(Component component) {
			return FindClosest (component.transform.position);
		}

		/// <summary>
		/// Finds the closest DanmakuField to the point specified by a Transform's position.
		/// </summary>
		/// <returns>The closest DanmakuField to the Transform's position.</returns>
		/// <param name="gameObject">the Transform used to find the closest DanmakuField.</param>
		public static DanmakuField FindClosest(Transform transform) {
			return FindClosest (transform.position);
		}

		public static DanmakuField FindClosest(Vector2 position) {
			if (fields == null) {
				fields = new List<DanmakuField>();
				fields.AddRange(Object.FindObjectsOfType<DanmakuField>());
			}
			if (fields.Count == 0) {
				DanmakuField encompassing = new GameObject("Danmaku Field").AddComponent<DanmakuField>();
				encompassing.useClipBoundary = false;
				fields.Add(encompassing);
			}
			if (fields.Count == 1) {
				return fields[0];
			}
			DanmakuField closest = null;
			float minDist = float.MaxValue;
			for(int i = 0; i < fields.Count; i++) {
				DanmakuField field = fields[i];
				Vector2 diff = field.bounds.Center - position;
				float distance = diff.sqrMagnitude;
				if(distance < minDist) {
					closest = field;
					minDist = distance;
				}
			}
			return closest;
		}

		public enum CoordinateSystem { View, ViewRelative, Relative, World }

		[SerializeField]
		private bool useClipBoundary = true;
		public bool UseClipBoundary {
			get {
				return useClipBoundary;
			}
			set {
				useClipBoundary = value;
			}
		}

		[SerializeField]
		private float clipBoundary = 1f;

		public float ClipBoundary {
			get {
				return clipBoundary;
			}
			set {
				clipBoundary = value;
			}
		}

		[SerializeField]
		private Vector2 fieldSize = new Vector2(20f, 20f);

		public Vector2 FieldSize {
			get {
				return fieldSize;
			}
			set {
				fieldSize = value;
			}
		}

		private static readonly Vector2 infiniteSize = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

		public DanmakuField TargetField {
			get;
			set;
		}

		internal Bounds2D bounds;
		private Bounds2D movementBounds;

		[SerializeField]
		private Camera camera2D;
		public Camera Camera2D {
			get {
				return camera2D;
			}
			set {
				camera2D = value;
			}
		}

		private Transform camera2DTransform;

		[SerializeField]
		private List<Camera> otherCameras;
		public List<Camera> OtherCameras {
			get {
				return otherCameras;
			}
		}

		public float Camera2DRotation {
			get {
				if(camera2D == null) {
					return float.NaN;
				}
				if(camera2DTransform == null) {
					camera2DTransform = camera2D.transform;
				}
				return camera2DTransform.eulerAngles.z;
			}
			set {
				camera2DTransform.rotation = Quaternion.Euler(0f, 0f, value);
			}
		}

		public Bounds2D Bounds {
			get {
				return bounds;
			}
		}

		public Bounds2D MovementBounds {
			get {
				return movementBounds;
			}
		}

		public virtual void Awake () {
			if (fields == null) {
				fields = new List<DanmakuField>();
			}
			fields.Add (this);
			TargetField = this;
		}

		public virtual void Update() {
			if (camera2D != null) {
				camera2DTransform = camera2D.transform;
				camera2D.orthographic = true;
				movementBounds.Center = bounds.Center = (Vector2)camera2DTransform.position;
				float size = camera2D.orthographicSize;
				movementBounds.Extents = new Vector2 (camera2D.aspect * size, size);
				for(int i = 0; i < otherCameras.Count; i++) {
					if(otherCameras[i] != null)
						otherCameras[i].rect = camera2D.rect;
				}
			} else {
				camera2DTransform = null;
				movementBounds.Center = bounds.Center = (Vector2)transform.position;
				movementBounds.Extents = fieldSize * 0.5f;
			}
			if(useClipBoundary) {
				bounds.Extents = movementBounds.Extents + Vector2.one * clipBoundary * movementBounds.Extents.Max();
			} else {
				bounds.Extents = infiniteSize;
			}
		}

		void OnDestroy() {
			if (fields != null) {
				fields.Remove(this);
			}
		}

		public Vector2 WorldPoint(Vector2 point, CoordinateSystem coordSys = CoordinateSystem.View) {
			switch (coordSys) {
				case CoordinateSystem.World:
					return point;
				case CoordinateSystem.Relative:
					return movementBounds.Min + point;
				case CoordinateSystem.ViewRelative:
					return point.Hadamard2(movementBounds.Size);
				default:
				case CoordinateSystem.View:
					return movementBounds.Min + point.Hadamard2(movementBounds.Size);
			}
		}

		public Vector2 RelativePoint(Vector2 point, CoordinateSystem coordSys = CoordinateSystem.View) {
			switch (coordSys) {
				case CoordinateSystem.World:
					return point - movementBounds.Min;
				case CoordinateSystem.Relative:
					return point;
				default:
				case CoordinateSystem.View:
					Vector2 size = movementBounds.Size;
					Vector2 inverse = new Vector2(1 / size.x, 1 / size.y);
					return (point - movementBounds.Min).Hadamard2(inverse);
			}
		}

		public Vector2 ViewPoint(Vector2 point, CoordinateSystem coordSys = CoordinateSystem.World) {
			Vector2 size = bounds.Size;
			switch (coordSys) {
				default:
				case CoordinateSystem.World:
					Vector2 offset = point - movementBounds.Min;
					return new Vector2(offset.x / size.x, offset.y / size.y);
				case CoordinateSystem.Relative:
					return new Vector2(point.x / size.x, point.y / size.y);
				case CoordinateSystem.View:
					return point;
			}
		}
		
		public GameObject SpawnGameObject(GameObject prefab, Vector2 location, CoordinateSystem coordSys = CoordinateSystem.View) {
			if (TargetField == null)
				TargetField = this;
			Quaternion rotation = prefab.transform.rotation;
			return Instantiate (gameObject, TargetField.WorldPoint (location, coordSys), rotation) as GameObject;
		}

		public T SpawnObject<T>(T prefab, Vector2 location, CoordinateSystem coordSys = CoordinateSystem.View) where T : Component {
			if (TargetField == null)
				TargetField = this;
			Quaternion rotation = prefab.transform.rotation;
			T clone = Instantiate (prefab, TargetField.WorldPoint (location, coordSys), rotation) as T;
			if (clone is IDanmakuObject)
				(clone as IDanmakuObject).Field = this;
			return clone;
		}
		
		public Danmaku SpawnDanmaku(DanmakuPrefab bulletType, Vector2 location, DynamicFloat rotation, CoordinateSystem coordSys = CoordinateSystem.View) {
			if (TargetField == null)
				TargetField = this;
			Danmaku bullet = Danmaku.GetInactive (bulletType, TargetField.WorldPoint(location, coordSys), rotation);
			bullet.Field = TargetField;
			bullet.Activate ();
			return bullet;
		}

		public Danmaku FireLinear(DanmakuPrefab prefab, 
                                  Vector2 location, 
                                  DynamicFloat rotation, 
                                  DynamicFloat speed,
		                          CoordinateSystem coordSys = CoordinateSystem.View,
                                  DanmakuModifier modifier = null) {
			if (TargetField == null)
				TargetField = this;
			Vector2 position = TargetField.WorldPoint (location, coordSys);
			if (modifier == null) {
				Danmaku danmaku = Danmaku.GetInactive (prefab, position, rotation);
				danmaku.Field = TargetField;
				danmaku.Activate ();
				danmaku.Speed = speed;
				danmaku.AngularSpeed = 0f;
				return danmaku;
			} else {
				FireData temp = new FireData();
				temp.Prefab = prefab;
				temp.Speed = speed;
				temp.AngularSpeed = 0f;
				temp.Field = this;
				modifier.Initialize(temp);
				modifier.OnFire(position, rotation);
				return null;
			}
		}
		
		public Danmaku FireCurved (DanmakuPrefab prefab,
                                     Vector2 location,
                                     DynamicFloat rotation,
                                     DynamicFloat speed,
                                     DynamicFloat angularSpeed,
                                     CoordinateSystem coordSys = CoordinateSystem.View,
                                     DanmakuModifier modifier = null) {
			if (TargetField == null)
				TargetField = this;
			Vector2 position = TargetField.WorldPoint (location, coordSys);
			if (modifier == null) {
				Danmaku danmaku = Danmaku.GetInactive (prefab, position, rotation);
				danmaku.Field = this;
				danmaku.Activate ();
				danmaku.Speed = speed;
				danmaku.AngularSpeed = angularSpeed;
				return danmaku;
			} else {
				FireData temp = new FireData();
				temp.Prefab = prefab;
				temp.Speed = speed;
				temp.AngularSpeed = angularSpeed;
				temp.Field = this;
				modifier.Initialize(temp);
				modifier.OnFire(position, rotation);
				return null;
			}
		}

		public Danmaku Fire (FireData data) {
			if (TargetField == null)
				TargetField = this;
			DanmakuField old = data.Field;
			data.Field = TargetField;
			Danmaku danmaku = data.Fire ();
			data.Field = old;
			return danmaku;
		}

		public FireBuilder Fire(DanmakuPrefab prefab) {
			return new FireBuilder (prefab, this);
		}

		#if UNITY_EDITOR
		void OnDrawGizmos() {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube (bounds.Center, bounds.Size);
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube (movementBounds.Center, movementBounds.Size);
		}
		#endif
	}
}