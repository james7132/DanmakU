// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections.Generic;
using UnityEngine;
using UnityUtilLib;
using UnityUtilLib.Pooling;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace DanmakU {
	
	/// <summary>
	/// A single projectile fired.
	/// The base object that represents a single bullet in a Danmaku game
	/// </summary>
	public sealed partial class Danmaku : IPooledObject, IPrefabed<DanmakuPrefab> {

		/// <summary>
		/// The standard number of bullets pre-spawned in if no Danmaku Game Controller is present
		/// </summary>
		internal const int standardStart = 1000;

		/// <summary>
		/// The standard number of bullets that is spawned used if a DanmakuGameController is not present
		/// </summary>
		internal const int standardSpawn = 100;

		/// <summary>
		/// A set of 2D vectors corresponding to unit circle coordinates
		/// </summary>
		internal static Vector2[] unitCircle;
		private static float invAngRes;
		private static int unitCircleMax;

		private static int[] collisionMask;
		private static DanmakuPool danmakuPool;

		/// <summary>
		/// A cached delta time value for procesing bullet updates.
		/// Static member accesses are slightly faster than member accesses or passing via parameters.
		/// Since it is a static value within each frame, it is best to cache it as a static variable.
		/// </summary>
		private static float dt;

		/// <summary>
		/// A map that matches colliders to respective collision handler scripts.
		/// Used to cache the results so that not every bullet collision triggers a GetComponents call.
		/// Cleared every frame: do not put permanent data in here.
		/// </summary>
		private static Dictionary<Collider2D, IDanmakuCollider[]> colliderMap;
		
		internal static void Setup(int initial = standardStart, int spawn = standardSpawn, float angRes = 0.1f) {
			colliderMap = new Dictionary<Collider2D, IDanmakuCollider[]> ();
			collisionMask = Util.CollisionLayers2D ();
			danmakuPool = new DanmakuPool (initial, spawn);
			invAngRes = 1f / angRes;
			unitCircleMax = Mathf.CeilToInt(360f / angRes);
			float angle = 90f;
			unitCircle = new Vector2[unitCircleMax];
			for (int i = 0; i < unitCircleMax; i++) {
				angle += angRes;
				unitCircle[i] = Util.OnUnitCircle(angle);
			}
		}

		internal static int Ang2Index(float angle) {
			float clamp = angle - 360 * Mathf.FloorToInt (angle / 360);
			int index = (int)(clamp * invAngRes);
			if (index >= unitCircleMax)
				index = unitCircleMax - 1;
			if (index < 0)
				index = 0;
			return index;
		}

		internal static Vector2 UnitCircle(float angle) {
			return unitCircle [Ang2Index(angle)];
		}

		internal static float Cos(float angle) {
			return unitCircle [Ang2Index (angle)].x;
		}

		internal static float Sin(float angle) {
			return unitCircle [Ang2Index (angle)].y;
		}

		internal static float Tan(float angle) {
			return Cos (angle) / Sin (angle);
		}

		/// <summary>
		/// Does a frame update on all currently active bullets.
		/// This should be called only once per frame. 
		/// </summary>
		internal static void UpdateAll() {
			colliderMap.Clear ();

			//caches the change in time since the last frame
			Danmaku.dt = Util.DeltaTime;
			Danmaku[] all = danmakuPool.all;
			for (int i = 0; i < all.Length; i++) {
				if(all[i] != null && all[i].is_active) {
					all[i].Update();
				}
			}
		}

		/// <summary>
		/// Deactivates all currently active bullets.
		/// </summary>
		public static void DeactivateAll() {
			Danmaku[] all = danmakuPool.all;
			for (int i = 0; i < all.Length; i++) {
				if(all[i] != null && all[i].is_active)
					all[i].DeactivateImmediate();
			}
		}

		public static void DeactivateInCircle(Vector2 center, float radius, int layerMask = ~0) {
			Danmaku[] all = danmakuPool.all;
			Danmaku target;
			float sqrRadius = radius * radius;
			for (int i = 0; i < all.Length; i++) {
				target = all[i];
				if((layerMask & (1 << target.layer)) != 0) {
					if(sqrRadius >= (target.Position - center).sqrMagnitude) {
						target.DeactivateImmediate();
					}
				}
			}
		}
		
		public static int TotalCount {
			get {
				return (danmakuPool != null) ? danmakuPool.totalCount : 0;
			}
		}
		
		public static int ActiveCount {
			get {
				return (danmakuPool != null) ? danmakuPool.totalCount : 0;
			}
		}
		
		public static Danmaku Get (DanmakuPrefab danmakuType, Vector2 position, DynamicFloat rotation, DanmakuField field) {
			if (danmakuPool == null) {
				new GameObject("Danmaku Game Controller").AddComponent<DanmakuGameController>();
			}
			Danmaku proj = danmakuPool.Get ();
			proj.MatchPrefab (danmakuType);
			proj.position.x = position.x;
			proj.position.y = position.y;
			proj.Rotation = rotation;
			proj.Field = field;
			return proj;
		}
		
		public static Danmaku Get(DanmakuField field, FireBuilder builder) {
			if (danmakuPool == null) {
				new GameObject("Danmaku Game Controller").AddComponent<DanmakuGameController>();
			}
			Danmaku proj = danmakuPool.Get ();
			proj.MatchPrefab (builder.Prefab);
			Vector2 position = field.WorldPoint (builder.Position, builder.CoordinateSystem);
			proj.position.x = position.x;
			proj.position.y = position.y;
			proj.Rotation = builder.Rotation;
			proj.Speed = builder.Velocity;
			proj.AngularSpeed = builder.AngularVelocity;
			proj.AddController (builder.Controller);
			proj.Damage = builder.Damage;
			if (builder.Group != null) 
				proj.AddToGroup (builder.Group);
			proj.Field = field;
			return proj;
		}
	}
}

