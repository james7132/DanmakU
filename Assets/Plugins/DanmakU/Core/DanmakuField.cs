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
	public sealed class DanmakuField : MonoBehaviour, IDanmakuObject {
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
			if(fields == null)
				return null;
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

		internal Dictionary<Collider2D, IDanmakuCollider[]> colliderMap;

		public enum CoordinateSystem { View, ViewRelative, Relative, World }

		public bool UseClipBoundary = true;

		public float ClipBoundary = 1f;

		public Vector2 FieldSize = new Vector2(20f, 20f);

		private static readonly Vector2 infiniteSize = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

		[System.NonSerialized]
		public DanmakuField TargetField;

		internal DanmakuPlayer player;
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

		public DanmakuPlayer Player {
			get {
				return player;
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

		void Awake () {
			if (fields == null) {
				fields = new List<DanmakuField>();
			}
			fields.Add (this);
			TargetField = this;

			colliderMap = new Dictionary<Collider2D, IDanmakuCollider[]> ();
		}

		void Update() {
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

			#if UNITY_EDITOR
			if(Application.isPlaying) {
			#endif

				colliderMap.Clear();

			#if UNITY_EDITOR
			}
			#endif
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

		public float AngleTowardPlayer(Vector2 startLocation, CoordinateSystem coordinateSystem = CoordinateSystem.World) {
			return Util.AngleBetween2D (startLocation, Player.transform.position);
		}

		/// <summary>
		/// Spawns the player with the given controller
		/// </summary>
		/// <param name="character">Character prefab, defines character behavior and attack patterns.</param>
		/// <param name="controller">Controller for the player, allows for a user to manually control it or let an AI take over.</param>
		public DanmakuPlayer SpawnPlayer(DanmakuPlayer playerCharacter, Vector2 position, CoordinateSystem coordSys = CoordinateSystem.View) {
			Vector2 spawnPos = WorldPoint(position, coordSys);
			player =  Object.Instantiate(playerCharacter);
			player.transform.position = spawnPos;
			if(player != null) {
				player.Reset (5);
				player.transform.parent = transform;
				player.Field = this;
			}
			return player;
		}

		public Enemy SpawnEnemy(Enemy prefab, Vector2 location, CoordinateSystem coordSys = CoordinateSystem.View) {
			Quaternion rotation = prefab.transform.rotation;
			Enemy enemy = Instantiate(prefab, WorldPoint(location, coordSys), rotation) as Enemy;
			enemy.Field = this;
			return enemy;
		}
		
		public GameObject SpawnGameObject(GameObject gameObject, Vector2 location, CoordinateSystem coordSys = CoordinateSystem.View) {
			Quaternion rotation = gameObject.transform.rotation;
			return Instantiate (gameObject, WorldPoint (location, coordSys), rotation) as GameObject;
		}

		public Component SpawnObject(Component prefab, Vector2 location, CoordinateSystem coordSys = CoordinateSystem.View) {
			Quaternion rotation = prefab.transform.rotation;
			return Instantiate (prefab, WorldPoint (location, coordSys), rotation) as Component;
		}

		public T SpawnObject<T>(T prefab, Vector2 location, CoordinateSystem coordSys = CoordinateSystem.View) where T : Component {
			Quaternion rotation = prefab.transform.rotation;
			return Instantiate (prefab, WorldPoint (location, coordSys), rotation) as T;
		}
		
		/// <summary>
		/// Spawns a projectile in the field.
		/// 
		/// If absoluteWorldCoord is set to false, location specifies a relative position in the field. 0.0 = left/bottom, 1.0 = right/top. Values greater than 1 or less than 0 spawn
		/// outside of of the camera view.
		/// </summary>
		/// <param name="prefab">Prefab for the spawned projectile, describes the visuals, size, and hitbox characteristics of the prefab.</param>
		/// <param name="location">The location within the field to spawn the projectile.</param>
		/// <param name="rotation">Rotation.</param>
		/// <param name="absoluteWorldCoord">If set to <c>true</c>, <c>location</c> is in absolute world coordinates relative to the bottom right corner of the game plane.</param>
		public Danmaku SpawnDanmaku(DanmakuPrefab bulletType, Vector2 location, DynamicFloat rotation, CoordinateSystem coordSys = CoordinateSystem.View) {
			Danmaku bullet = Danmaku.Get (bulletType, WorldPoint(location, coordSys), rotation, this);
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
			Vector2 position = WorldPoint (location, coordSys);
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
			Vector2 position = WorldPoint (location, coordSys);
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
				modifier.Fire (WorldPoint (data.Position, data.CoordinateSystem), data.Rotation);
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