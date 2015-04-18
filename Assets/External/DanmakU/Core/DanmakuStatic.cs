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
	public sealed partial class Danmaku : IPooledObject, IColorable, IPrefabed<DanmakuPrefab> {

		private const float twoPI = Mathf.PI * 2;
		private static Vector2[] unitCircle;
		private static float angleResolution;
		private static float invAngRes;
		private static int[] collisionMask;
		private static int unitCircleMax;
		private static DanmakuPool danmakuPool;
		private static float dt;
		
		internal static void Setup(int initial, int spawn, float angRes) {
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
			dt = Util.DeltaTime;
			Danmaku[] all = danmakuPool.all;
			int activeCount = danmakuPool.activeCount;
			for (int i = 0; i < all.Length; i++) {
				if(i > activeCount) {
					break;
				}

				// Note: the reason why I am using a for loop here is because the CIL compiler ignores bounds checking
				// if and only if used in the form of "for(int x = 0, x < array.Length; x++)".
				// It actually increases performance signifigantly

				all[i].Update();
			}
		}
		
		public static void DeactivateAll() {
			danmakuPool.activeCount = 0;
		}

		public static void DeactivateInCircle(Vector2 center, float radius, int layerMask = ~0) {
			Danmaku[] all = danmakuPool.all;
			Danmaku target;
			int activeCount = danmakuPool.activeCount;
			float sqrRadius = radius * radius, sqrDRadius, DRadius;
			for (int i = 0; i < all.Length; i++) {
				if(i > activeCount) {
					break;
				}
				
				// Note: the reason why I am using a for loop here is because the CIL compiler ignores bounds checking
				// if and only if used in the form of "for(int x = 0, x < array.Length; x++)".
				// It actually increases performance signifigantly
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
			int activeCount = danmakuPool.activeCount;
			float sqrRadius = radius * radius;
			for (int i = 0; i < all.Length; i++) {
				if(i > activeCount) {
					break;
				}
				
				// Note: the reason why I am using a for loop here is because the CIL compiler ignores bounds checking
				// if and only if used in the form of "for(int x = 0, x < array.Length; x++)".
				// It actually increases performance signifigantly
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
			Danmaku proj = danmakuPool.Get ();
			proj.MatchPrefab (danmakuType);
			proj.PositionImmediate = position;
			proj.Rotation = rotation;
			proj.Field = field;
			proj.bounds = field.bounds;
			return proj;
		}
		
		public static Danmaku Get(DanmakuField field, FireBuilder builder) {
			Danmaku proj = danmakuPool.Get ();
			proj.MatchPrefab (builder.Prefab);
			proj.PositionImmediate = field.WorldPoint (builder.Position, builder.CoordinateSystem);
			proj.Rotation = builder.Rotation;
			proj.Speed = builder.Velocity;
			proj.AngularSpeed = builder.AngularVelocity;
			proj.AddController (builder.Controller);
			proj.Damage = builder.Damage;
			if (builder.Group != null) 
				proj.AddToGroup (builder.Group);
			proj.Field = field;
			proj.bounds = field.bounds;
			return proj;
		}
	}
}

