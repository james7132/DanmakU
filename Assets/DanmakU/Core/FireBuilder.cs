// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityUtilLib;

namespace DanmakU {

	/// <summary>
	/// A container class for passing information about firing a single bullet.
	/// For creating more complex firing. See FireBuilder.
	/// </summary>
	public sealed class FireData {

		public DanmakuPrefab Prefab;
		public DanmakuField Field;
		public Vector2 Position = Vector2.zero;
		public DynamicFloat Rotation = 0f;
		public DynamicFloat Speed = 0f;
		public DynamicFloat AngularSpeed = 0f;
		public DanmakuController Controller;
		public DynamicInt Damage = 0;
		public DanmakuGroup Group;

		public Danmaku Fire() {
			Danmaku danmaku = Danmaku.GetInactive(this);
			danmaku.Field = Field;
			danmaku.Activate ();
			return danmaku;
		}

	}

	public abstract class AbstractFireBuilder {

		public abstract void AddModifier (DanmakuModifier modifier);

	}

	[System.Serializable]
	public class FireBuilder {
		public DanmakuPrefab Prefab;
		public Vector2 Position;
		public DynamicFloat Rotation;
		public DynamicFloat Velocity;
		public DynamicFloat AngularVelocity;
		public DanmakuController Controller;
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
		
	}
}

