// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections.Generic;
using DanmakU.Modifiers;

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

	[System.Serializable]
	public class FireBuilder {
		private DanmakuPrefab prefab;
		public DanmakuPrefab Prefab {
			get {
				return prefab;
			}
			set {
				prefab = value;
			}
		}

		private Stack<DanmakuModifier> modifiers;
		
		public FireBuilder(DanmakuPrefab prefab) {

			this.prefab = prefab;
			modifiers = new Stack<DanmakuModifier>();
		}

		public FireBuilder AddModifier(DanmakuModifier modifier) {
			modifiers.Push(modifier);
			return this;
		}

		public FireBuilder WithController (IDanmakuController controller) {
			modifiers.Push(new AddControllersModifier(controller));
			return this;
		}

		public FireBuilder WithController (IEnumerable<IDanmakuController> controllers) {
			modifiers.Push(new AddControllersModifier(controllers));
			return this;
		}

		public FireBuilder WithController (DanmakuController controller) {
			modifiers.Push(new AddControllersModifier(controller));
			return this;
		}

		public FireBuilder WithController (IEnumerable<DanmakuController> controllers) {
			modifiers.Push(new AddControllersModifier(controllers));
			return this;
		}

		public FireBuilder WithoutControllers () {
			modifiers.Push(new ClearControllersModifier());
			return this;
		}

		public void Fire() {
			DanmakuModifier modifier = DanmakuModifier.Construct(modifiers);
			modifier.Initialize(new FireData() { Prefab = Prefab });
			modifier.OnFire(Vector2.zero, 0f);
		}

		public void Fire(FireData data) {
		}
		
	}
}

