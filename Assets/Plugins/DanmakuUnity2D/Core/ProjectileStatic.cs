using System;
using UnityEngine;
using UnityUtilLib;
using UnityUtilLib.Pooling;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {
	
	/// <summary>
	/// A single projectile fired.
	/// The base object that represents a single bullet in a Danmaku game
	/// </summary>
	public sealed partial class Projectile : IPooledObject, IColorable, IPrefabed<ProjectilePrefab> {

		private const float twoPI = Mathf.PI * 2;
		private static Vector2[] unitCircle;
		private static float angleResolution;
		private static float invAngRes;
		private static int[] collisionMask;
		private static ProjectilePool projectilePool;
		private static float dt;
		
		internal static void Setup(int initial, int spawn, float angRes) {
			collisionMask = Util.CollisionLayers2D ();
			projectilePool = new ProjectilePool (initial, spawn);
			angleResolution = angRes;
			invAngRes = 1f / angRes;
			int count = Mathf.CeilToInt(360f / angleResolution);
			float angle;
			unitCircle = new Vector2[count];
			for (int i = 0; i < count; i++) {
				angle = i * angleResolution + 90f;
				unitCircle[i] = Util.OnUnitCircle(angle);
			}
		}

		internal static int Ang2Index(float angle) {
			float clamp = angle - 360 * Mathf.FloorToInt (angle / 360);
			return (int)(clamp * invAngRes);
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
			dt = Util.TargetDeltaTime;
			Projectile[] all = projectilePool.all;
			int totalCount = projectilePool.totalCount;
			for (int i = 0; i < totalCount; i++) {
				if(all[i].is_active) {
					all[i].Update();
				}
			}
		}
		
		public static void DeactivateAll() {
			Projectile[] all = projectilePool.all;
			int totalCount = projectilePool.totalCount;
			for (int i = 0; i < totalCount; i++) {
				if(all[i].is_active)
					all[i].DeactivateImmediate();
			}
		}
		
		public static int TotalCount {
			get {
				return (projectilePool != null) ? projectilePool.totalCount : 0;
			}
		}
		
		public static int ActiveCount {
			get {
				return (projectilePool != null) ? projectilePool.totalCount : 0;
			}
		}
		
		public static Projectile Get (ProjectilePrefab projectileType, Vector2 position, float rotation, DanmakuField field) {
			Projectile proj = projectilePool.Get ();
			proj.MatchPrefab (projectileType);
			proj.PositionImmediate = position;
			proj.Rotation = rotation;
			proj.field = field;
			proj.bounds = field.bounds;
			return proj;
		}
		
		public static Projectile Get(DanmakuField field, FireBuilder builder) {
			Projectile proj = projectilePool.Get ();
			proj.MatchPrefab (builder.Prefab);
			proj.PositionImmediate = field.WorldPoint (builder.Position, builder.CoordinateSystem);
			proj.Rotation = builder.Rotation;
			proj.Velocity = builder.Velocity;
			proj.AngularVelocity = builder.AngularVelocity;
			proj.AddController (builder.Controller);
			if (builder.Group != null) 
				proj.AddToGroup (builder.Group);
			proj.field = field;
			proj.bounds = field.bounds;
			return proj;
		}
	}
}

