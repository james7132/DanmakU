// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityUtilLib;

namespace DanmakU {

	[System.Serializable]
	public class FireBuilder : IClonable<FireBuilder> {
		public DanmakuPrefab Prefab = null;
		public Vector2 Position = Vector2.zero;
		public DynamicFloat Rotation;
		public DynamicFloat Velocity;
		public DynamicFloat AngularVelocity;
		public DanmakuController Controller = null;
		public DanmakuField.CoordinateSystem CoordinateSystem = DanmakuField.CoordinateSystem.View;
		public DanmakuGroup Group;
		public int Damage;
		public DanmakuModifier Modifier;
		public Color ColorOverride;
		
		public FireBuilder() {
		}
		
		public void ResetColor() {
			if (Prefab != null)
				throw new System.InvalidOperationException ("Cannot reset color without a Danmaku Prefab");
			ColorOverride = Prefab.Color;
		}
		
		public FireBuilder(DanmakuPrefab prefab) {
			this.Prefab = prefab;
		}
		
		public void Copy(FireBuilder other) {
			Prefab = other.Prefab;
			Position = other.Position;
			Rotation = other.Rotation;
			Velocity = other.Velocity;
			AngularVelocity = other.AngularVelocity;
			Controller = other.Controller;
			CoordinateSystem = other.CoordinateSystem;
			Group = other.Group;
			Damage = other.Damage;
			Modifier = other.Modifier;
		}
		
		#region IClonable implementation
		public FireBuilder Clone () {
			FireBuilder copy = new FireBuilder ();
			copy.Copy (this);
			return copy;
		}
		#endregion
	}
}

