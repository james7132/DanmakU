// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilLib;

namespace DanmakU {

	public sealed class DanmakuGroup : HashSet<Danmaku> {

		#region Position Functions
		
		public void SetPosition(Vector2 position) {
			foreach(Danmaku danmaku in this) {
				danmaku.Position = position;
			}
		}
		
		public void SetPosition (Vector2[] positions) {
			if (positions == null)
				throw new System.ArgumentNullException ();
			int max = positions.Length;
			foreach (Danmaku danmaku in this) {
				danmaku.Position = positions[Random.Range(0, max)];
			}
		}
		
		public void SetPosition (Rect area) {
			foreach (Danmaku danmaku in this) {
				danmaku.Position = area.RandomPoint();
			}
		}
		
		public void Translate (Vector2 deltaPos) {
			foreach(Danmaku danmaku in this) {
				danmaku.Position += deltaPos;
			}
		}
		
		#endregion

		#region Rotation Functions
		
		public void SetRotation(DynamicFloat rotation) {
			foreach(Danmaku danmaku in this) {
				danmaku.Rotation = rotation.Value;
			}
		}
		
		public void SetRotation (DynamicFloat[] rotations) {
			if (rotations == null)
				throw new System.ArgumentNullException ();
			int max = rotations.Length;
			foreach (Danmaku danmaku in this) {
				danmaku.Rotation = rotations[Random.Range(0, max)];
			}
		}
		
		public void Rotate(DynamicFloat delta) {
			foreach(Danmaku danmaku in this) {
				danmaku.Rotation += delta;
			}
		}
		
		#endregion

		#region Speed Functions
		
		public void SetSpeed(DynamicFloat velocity) {
			foreach (Danmaku danmaku in this) {
				danmaku.Speed = velocity.Value;
			}
		}
		
		public void SetSpeed (DynamicFloat[] speeds) {
			if (speeds == null)
				throw new System.ArgumentNullException ();
			int max = speeds.Length;
			foreach (Danmaku danmaku in this) {
				danmaku.Speed = speeds[Random.Range(0, max)];
			}
		}
		
		public void Accelerate (DynamicFloat deltaSpeed) {
			foreach (Danmaku danmaku in this) {
				danmaku.Speed += deltaSpeed.Value;
			}
		}
		
		#endregion
		
		#region Angular Speed Functions
		
		public void SetAngularSpeed(DynamicFloat angularSpeed) {
			foreach (Danmaku danmaku in this) {
				danmaku.AngularSpeed = angularSpeed.Value;
			}
		}
		
		public void SetAngularSpeed (DynamicFloat[] angularSpeeds) {
			if (angularSpeeds == null)
				throw new System.ArgumentNullException ();
			int max = angularSpeeds.Length;
			foreach (Danmaku danmaku in this) {
				danmaku.AngularSpeed = angularSpeeds[Random.Range(0, max)];
			}
		}
		
		public void AngularAccelerate (DynamicFloat deltaSpeed) {
			foreach (Danmaku danmaku in this) {
				danmaku.Speed += deltaSpeed.Value;
			}
		}
		
		#endregion

		#region Damage Functions
		
		public void SetDamage (DynamicInt damage) {
			foreach(Danmaku danmaku in this) {
				danmaku.Damage = damage.Value;
			}
		}
		
		public void SetDamage (DynamicInt[] damages) {
			if (damages == null)
				throw new System.ArgumentNullException ();
			int max = damages.Length;
			foreach (Danmaku danmaku in this) {
				danmaku.Damage = damages[Random.Range(0, max)].Value;
			}
		}
		
		#endregion

		#region Color Functions

		public void SetColor(Color color) {
			foreach(Danmaku danmaku in this) {
				danmaku.Color = color;
			}
		}

		public void SetColor(Color[] colors) {
			if (colors == null)
				throw new System.ArgumentNullException ();
			int max = colors.Length;
			foreach (Danmaku danmaku in this) {
				danmaku.Color = colors[Random.Range(0, max)];
			}
		}

		public void SetColor(Gradient colors) {
			if (colors == null)
				throw new System.ArgumentNullException ();
			foreach (Danmaku danmaku in this) {
				danmaku.Color = colors.Evaluate(Random.value);
			}
		}

		#endregion
		
		#region Controller Functions
		
		public void AddController(IDanmakuController controller) {
			foreach(Danmaku proj in this) {
				proj.AddController(controller.UpdateDanmaku);
			}
		}
		
		public void RemoveController(IDanmakuController controller) {
			foreach(Danmaku proj in this) {
				proj.RemoveController(controller.UpdateDanmaku);
			}
		}
		
		public void ClearControllers() {
			foreach(Danmaku danmaku in this) {
				danmaku.ClearControllers();
			}
		}
		
		#endregion

		#region Misc Functions

		public void SetTag(string tag) {
			foreach(Danmaku danmaku in this) {
				danmaku.Tag = tag;
			}
		}

		public void SetLayer(int layer) {
			foreach(Danmaku danmaku in this) {
				danmaku.Layer = layer;
			}
		}

		public void SetBoundsCheck(bool boundsCheck) {
			foreach(Danmaku danmaku in this) {
				danmaku.BoundsCheck = boundsCheck;
			}
		}

		public void SetCollisionCheck (bool collisionCheck) {
			foreach (Danmaku danmaku in this) {
				danmaku.CollisionCheck = collisionCheck;
			}
		}

		public void MatchPrefab(DanmakuPrefab prefab) {
			foreach (Danmaku danmaku in this) {
				danmaku.MatchPrefab(prefab);
			}
		}

		#endregion

		#region Collection Functions

		public new void Add (Danmaku item) {
			bool added = base.Add(item);
			if (added) {
				item.groups.Add (this);
				item.groupCountCache++;
				item.groupCheck = item.groups.Count > 0;
			}
		}

		public new void Clear () {
			foreach(Danmaku proj in this) {
				proj.RemoveFromGroup(this);
			}
			base.Clear ();
		}

		public new bool Remove (Danmaku item) {
			bool success = false;
			success = base.Remove(item);
			if (success) {
				item.groups.Remove (this);
				item.groupCountCache--;
				item.groupCheck = item.groups.Count > 0;
			}
			return success;
		}

		#endregion
	
	}
}

