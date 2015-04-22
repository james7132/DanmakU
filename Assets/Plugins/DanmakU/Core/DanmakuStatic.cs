// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
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

		private const int standardStart = 1000;
		private const int standardSpawn = 100;

		private const float twoPI = Mathf.PI * 2;
		private static Vector2[] unitCircle;
		private static float angleResolution;
		private static float invAngRes;
		private static int[] collisionMask;
		private static int unitCircleMax;
		private static DanmakuPool danmakuPool;
		private static float dt;
		
		internal static void Setup(int initial = standardStart, int spawn = standardSpawn, float angRes = 0.1f) {
			collisionMask = Util.CollisionLayers2D ();
			danmakuPool = new DanmakuPool (initial, spawn);
			angleResolution = angRes;
			invAngRes = 1f / angRes;
			unitCircleMax = Mathf.CeilToInt(360f / angleResolution);
			float angle;
			unitCircle = new Vector2[unitCircleMax];
			for (int i = 0; i < unitCircleMax; i++) {
				angle = i * angleResolution + 90f;
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
		
		internal static void UpdateAll() {
			Danmaku.dt = Util.DeltaTime;
			Danmaku[] all = danmakuPool.all;
			for (int i = 0; i < all.Length; i++) {
				if(all[i] != null && all[i].is_active) {
					all[i].Update();
				}
			}
		}
		
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
			float sqrRadius = radius * radius, sqrDRadius, DRadius;
			for (int i = 0; i < all.Length; i++) {
				target = all[i];
				if((layerMask & (1 << target.layer)) != 0) {
					DRadius = target.colliderRadius;
					sqrDRadius = DRadius * DRadius;
					if(sqrRadius + sqrDRadius >= (target.collisionCenter - center).sqrMagnitude) {
						target.DeactivateImmediate();
					}
				}
			}
		}

		public static void DirectDeactivateInCircle(Vector2 center, float radius, int layerMask = ~0) {
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

