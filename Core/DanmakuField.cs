// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityUtilLib;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace DanmakU {

	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("DanmakU/Danmaku Field")]
	public class DanmakuField : MonoBehaviour, IDanmakuObject {
		#region IDanmakuObject implementation
		DanmakuField IDanmakuObject.Field {
			get {
				return TargetField;
			}
			set {
				TargetField = value;
			}
		}
		#endregion

		internal static List<DanmakuField> fields;

		public static DanmakuField FindClosest(GameObject gameObject) {
			return FindClosest (gameObject.transform.position);
		}

		public static DanmakuField FindClosest(Component component) {
			return FindClosest (component.transform.position);
		}

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
				encompassing.UseClipBoundary = false;
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

		public bool UseClipBoundary = true;

		public float ClipBoundary = 1f;

		public Vector2 FieldSize = new Vector2(20f, 20f);

		private static readonly Vector2 infiniteSize = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

		[System.NonSerialized]
		public DanmakuField TargetField;

		private float currentAspectRatio;
		private float screenOffset;
		internal Bounds2D bounds;
		private Bounds2D movementBounds;

		[SerializeField]
		private Camera camera2D;
		private Transform camera2DTransform;

		[SerializeField]
		private Camera[] otherCameras;

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

		public float XSize {
			get { return bounds.Size.x; }
		}

		public float YSize {
			get { return bounds.Size.y; }
		}

		public Vector2 BottomLeft {
			get { return WorldPoint (new Vector2(0f, 0f)); }
		}

		public Vector2 BottomRight {
			get { return WorldPoint (new Vector2(1f, 0f)); }
		}

		public Vector2 TopLeft {
			get { return WorldPoint (new Vector2(0f, 1f)); }
		}

		public Vector2 TopRight {
			get { return WorldPoint (new Vector2(1f, 1f)); }
		}

		public Vector2 Center {
			get { return WorldPoint (new Vector2(0.5f, 0.5f)); }
		}

		public Vector2 Top {
			get { return WorldPoint (new Vector2 (0.5f, 1f)); }
		}

		public Vector2 Bottom {
			get { return WorldPoint (new Vector2 (0.5f, 0f));}
		}

		public Vector2 Right {
			get { return WorldPoint (new Vector2 (1f, 0.5f)); }
		}

		public Vector2 Left {
			get { return WorldPoint (new Vector2 (0f, 0.5f));}
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
				for(int i = 0; i < otherCameras.Length; i++) {
					if(otherCameras[i] != null)
						otherCameras[i].rect = camera2D.rect;
				}
			} else {
				camera2DTransform = null;
				movementBounds.Center = bounds.Center = (Vector2)transform.position;
				movementBounds.Extents = FieldSize * 0.5f;
			}
			if(UseClipBoundary) {
				bounds.Extents = movementBounds.Extents + Vector2.one * ClipBoundary * movementBounds.Extents.Max();
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

		public Component SpawnObject(Component prefab, Vector2 location, CoordinateSystem coordSys = CoordinateSystem.View) {
			if (TargetField == null)
				TargetField = this;
			Quaternion rotation = prefab.transform.rotation;
			return Instantiate (prefab, TargetField.WorldPoint (location, coordSys), rotation) as Component;
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
			Danmaku bullet = Danmaku.Get (bulletType, TargetField.WorldPoint(location, coordSys), rotation, this);
			bullet.Activate ();
			return bullet;
		}

		public Danmaku FireLinear(DanmakuPrefab bulletType, 
                                     Vector2 location, 
                                     DynamicFloat rotation, 
                                     DynamicFloat velocity,
		                             CoordinateSystem coordSys = CoordinateSystem.View,
		                             DanmakuController controller = null,
                                     DanmakuModifier modifier = null,
                                     DanmakuGroup group = null,
		                          Color? colorOverride = null) {
			if (TargetField == null)
				TargetField = this;
			Vector2 position = TargetField.WorldPoint (location, coordSys);
			if (modifier == null) {
				Danmaku danmaku = Danmaku.Get (bulletType, position, rotation, this);
				danmaku.Activate ();
				danmaku.Speed = velocity;
				if (group != null) {
					group.Add (danmaku);
				}
				return danmaku;
			} else {
				modifier.Initialize(bulletType, velocity, 0f, this,  null, group);
				modifier.Fire(position, rotation);
				return null;
			}
		}
		
		public Danmaku FireCurved(DanmakuPrefab bulletType,
                                     Vector2 location,
                                     DynamicFloat rotation,
                                     DynamicFloat velocity,
                                     DynamicFloat angularVelocity,
                                     CoordinateSystem coordSys = CoordinateSystem.View,
                             		 DanmakuController controller = null,
                                     DanmakuModifier modifier = null,
		                          DanmakuGroup group = null) {
			if (TargetField == null)
				TargetField = this;
			Vector2 position = TargetField.WorldPoint (location, coordSys);
			if (modifier == null) {
				Danmaku danmaku = Danmaku.Get (bulletType, position, rotation, this);
				danmaku.Activate ();
				danmaku.Speed = velocity;
				danmaku.AngularSpeed = angularVelocity;
				danmaku.AddController(controller);
				if (group != null) {
					group.Add (danmaku);
				}
				return danmaku;
			} else {
				modifier.Initialize(bulletType, velocity, angularVelocity, this, null, group);
				modifier.Fire(position, rotation);
				return null;
			}
		}
		
		public Danmaku Fire(FireBuilder data) {
			DanmakuModifier modifier = data.Modifier;
			if (modifier == null) {
				Danmaku danmaku = Danmaku.Get (this, data);
				danmaku.Activate ();
				return danmaku;
			} else {
				modifier.Initialize (data, this);
				if (TargetField == null)
					TargetField = this;
				modifier.Fire (TargetField.WorldPoint (data.Position, data.CoordinateSystem), data.Rotation);
				return null;
			}
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