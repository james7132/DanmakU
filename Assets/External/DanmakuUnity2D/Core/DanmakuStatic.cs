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
	public sealed partial class Danmaku : IPooledObject, IColorable, IPrefabed<DanmakuPrefab> {

		private const float twoPI = Mathf.PI * 2;
		private static Vector2[] unitCircle;
		private static float angleResolution;
		private static float invAngRes;
		private static int[] collisionMask;
		private static int unitCircleMax;
		private static ProjectilePool projectilePool;

		private static float oldDt;
		private static float dt;
		private static bool dtChanged;
		
		internal static void Setup(int initial, int spawn, float angRes) {
			collisionMask = Util.CollisionLayers2D ();
			projectilePool = new ProjectilePool (initial, spawn);
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
//			int index = Ang2Index(angle);
//			if(index > unitCircleMax || angle < 0)
//				Debug.Log(index);

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
			dtChanged = oldDt != dt;
			Danmaku[] all = projectilePool.all;
			for (int i = 0; i < all.Length; i++) {
				if(all[i] != null && all[i].is_active) {
					all[i].Update();
				}
			}
			oldDt = dt;
		}
		
		public static void DeactivateAll() {
			Danmaku[] all = projectilePool.all;
			for (int i = 0; i < all.Length; i++) {
				if(all[i] != null && all[i].is_active)
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
		
		public static Danmaku Get (DanmakuPrefab danmakuType, Vector2 position, DynamicFloat rotation, DanmakuField field) {
			Danmaku proj = projectilePool.Get ();
			proj.MatchPrefab (danmakuType);
			proj.PositionImmediate = position;
			proj.Rotation = rotation;
			proj.field = field;
			proj.bounds = field.bounds;
			return proj;
		}
		
		public static Danmaku Get(DanmakuField field, FireBuilder builder) {
			Danmaku proj = projectilePool.Get ();
			proj.MatchPrefab (builder.Prefab);
			proj.PositionImmediate = field.WorldPoint (builder.Position, builder.CoordinateSystem);
			proj.Rotation = builder.Rotation;
			proj.Speed = builder.Velocity;
			proj.AngularVelocity = builder.AngularVelocity;
			proj.AddController (builder.Controller);
			proj.Damage = builder.Damage;
			if (builder.Group != null) 
				proj.AddToGroup (builder.Group);
			proj.field = field;
			proj.bounds = field.bounds;
			return proj;
		}
	}
}

