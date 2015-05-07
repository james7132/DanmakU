using UnityEngine;
using UnityUtilLib;
using System.Collections.Generic;

namespace DanmakU {

	public static class DanmakuCollectionExtensions {

		#region Position Functions

		/// <summary>
		/// Moves all of the bullets in the group to a single position.
		/// </summary>
		/// <param name="position">The position to move to, in world coordinates.</param>
		public static void SetPosition(this IEnumerable<Danmaku> danmakus, Vector2 position) {
			foreach(var danmaku in danmakus) {
				danmaku.Position = position;
			}
		}

		/// <summary>
		/// Moves all of the bullets to random positions.
		/// Positions are randomly chosen from a provided array.
		/// </summary>
		/// <param name="positions">Positions.</param>
		public static void SetPosition (this IEnumerable<Danmaku> danmakus, Vector2[] positions) {
			if (positions == null)
				throw new System.ArgumentNullException ();
			int max = positions.Length;
			foreach (var danmaku in danmakus) {
				danmaku.Position = positions[Random.Range(0, max)];
			}
		}
		
		public static void SetPosition (this IEnumerable<Danmaku> danmakus, Rect area) {
			foreach (var danmaku in danmakus) {
				danmaku.Position = area.RandomPoint();
			}
		}
		
		public static void Translate (this IEnumerable<Danmaku> danmakus, Vector2 deltaPos) {
			foreach(var danmaku in danmakus) {
				danmaku.Position += deltaPos;
			}
		}

		#endregion

		#region Rotation Functions
		
		public static void SetRotation(this IEnumerable<Danmaku> danmakus, DynamicFloat rotation) {
			foreach(var danmaku in danmakus) {
				danmaku.Rotation = rotation.Value;
			}
		}
		
		public static void SetRotation (this IEnumerable<Danmaku> danmakus, DynamicFloat[] rotations) {
			if (rotations == null)
				throw new System.ArgumentNullException ();
			int max = rotations.Length;
			foreach (var danmaku in danmakus) {
				danmaku.Rotation = rotations[Random.Range(0, max)];
			}
		}
		
		public static void Rotate(this IEnumerable<Danmaku> danmakus, DynamicFloat delta) {
			foreach(var danmaku in danmakus) {
				danmaku.Rotation += delta;
			}
		}
		
		#endregion
		
		#region Speed Functions
		
		public static void SetSpeed(this IEnumerable<Danmaku> danmakus, DynamicFloat velocity) {
			foreach (var danmaku in danmakus) {
				danmaku.Speed = velocity.Value;
			}
		}
		
		public static void SetSpeed (this IEnumerable<Danmaku> danmakus, DynamicFloat[] speeds) {
			if (speeds == null)
				throw new System.ArgumentNullException ();
			int max = speeds.Length;
			foreach (var danmaku in danmakus) {
				danmaku.Speed = speeds[Random.Range(0, max)];
			}
		}
		
		public static void Accelerate (this IEnumerable<Danmaku> danmakus, DynamicFloat deltaSpeed) {
			foreach (var danmaku in danmakus) {
				danmaku.Speed += deltaSpeed.Value;
			}
		}
		
		#endregion
		
		#region Angular Speed Functions
		
		public static void SetAngularSpeed(this IEnumerable<Danmaku> danmakus, DynamicFloat angularSpeed) {
			foreach (var danmaku in danmakus) {
				danmaku.AngularSpeed = angularSpeed.Value;
			}
		}
		
		public static void SetAngularSpeed (this IEnumerable<Danmaku> danmakus, DynamicFloat[] angularSpeeds) {
			if (angularSpeeds == null)
				throw new System.ArgumentNullException ();
			int max = angularSpeeds.Length;
			foreach (var danmaku in danmakus) {
				danmaku.AngularSpeed = angularSpeeds[Random.Range(0, max)];
			}
		}
		
		public static void AngularAccelerate (this IEnumerable<Danmaku> danmakus, DynamicFloat deltaSpeed) {
			foreach (var danmaku in danmakus) {
				danmaku.Speed += deltaSpeed.Value;
			}
		}
		
		#endregion
		
		#region Damage Functions
		
		public static void SetDamage (this IEnumerable<Danmaku> danmakus, DynamicInt damage) {
			foreach(var danmaku in danmakus) {
				danmaku.Damage = damage.Value;
			}
		}
		
		public static void SetDamage (this IEnumerable<Danmaku> danmakus, DynamicInt[] damages) {
			if (damages == null)
				throw new System.ArgumentNullException ();
			int max = damages.Length;
			foreach (var danmaku in danmakus) {
				danmaku.Damage = damages[Random.Range(0, max)].Value;
			}
		}
		
		#endregion
		
		#region Color Functions
		
		public static void SetColor(this IEnumerable<Danmaku> danmakus, Color color) {
			foreach(var danmaku in danmakus) {
				danmaku.Color = color;
			}
		}
		
		public static void SetColor(this IEnumerable<Danmaku> danmakus, Color[] colors) {
			if (colors == null)
				throw new System.ArgumentNullException ();
			int max = colors.Length;
			foreach (var danmaku in danmakus) {
				danmaku.Color = colors[Random.Range(0, max)];
			}
		}
		
		public static void SetColor(this IEnumerable<Danmaku> danmakus, Gradient colors) {
			if (colors == null)
				throw new System.ArgumentNullException ();
			foreach (var danmaku in danmakus) {
				danmaku.Color = colors.Evaluate(Random.value);
			}
		}
		
		#endregion
		
		#region Controller Functions
		
		public static void AddController(this IEnumerable<Danmaku> danmakus, IDanmakuController controller) {
			foreach(var danmaku in danmakus) {
				danmaku.AddController(controller.UpdateDanmaku);
			}
		}
		
		public static void RemoveController(this IEnumerable<Danmaku> danmakus, IDanmakuController controller) {
			foreach(var danmaku in danmakus) {
				danmaku.RemoveController(controller.UpdateDanmaku);
			}
		}
		
		public static void ClearControllers(this IEnumerable<Danmaku> danmakus) {
			foreach(var danmaku in danmakus) {
				danmaku.ClearControllers();
			}
		}
		
		#endregion

		#region Misc Functions
		
		public static void SetTag(this IEnumerable<Danmaku> danmakus, string tag) {
			foreach(var danmaku in danmakus) {
				danmaku.Tag = tag;
			}
		}
		
		public static void SetLayer(this IEnumerable<Danmaku> danmakus, int layer) {
			foreach(var danmaku in danmakus) {
				danmaku.Layer = layer;
			}
		}
		
		public static void SetBoundsCheck(this IEnumerable<Danmaku> danmakus, bool boundsCheck) {
			foreach(var danmaku in danmakus) {
				danmaku.BoundsCheck = boundsCheck;
			}
		}
		
		public static void SetCollisionCheck (this IEnumerable<Danmaku> danmakus, bool collisionCheck) {
			foreach (var danmaku in danmakus) {
				danmaku.CollisionCheck = collisionCheck;
			}
		}
		
		public static void MatchPrefab(this IEnumerable<Danmaku> danmakus, DanmakuPrefab prefab) {
			foreach (var danmaku in danmakus) {
				danmaku.MatchPrefab(prefab);
			}
		}
		
		#endregion

	}
}

