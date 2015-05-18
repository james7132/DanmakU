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

		private DanmakuField field;
		public DanmakuField Field {
			get {
				return field;
			}
			set {
				field = value;
			}
		}

		private Vector2 position;
		public Vector2 Position {
			get {
				return position;
			}
			set {
				position = value;
			}
		}

		private DynamicFloat rotation;
		public DynamicFloat Rotation {
			get {
				return rotation;
			}
			set {
				rotation = value;
			}
		}

		private DanmakuController controller;
		public event DanmakuController Controller {
			add {
				controller += value;
			}
			remove {
				controller -= value;
			}
		}

		private Stack<DanmakuModifier> modifiers;
		
		public FireBuilder(DanmakuPrefab prefab, DanmakuField field = null) {
			this.prefab = prefab;
			this.field = field;
			modifiers = new Stack<DanmakuModifier>();
		}

		public FireBuilder At (Vector2 position) {
			this.position = position;
			return this;
		}

		public FireBuilder At (Transform transform) {
			if(transform == null)
				throw new System.ArgumentNullException();

			position = transform.position;
			return this;
		}

		public FireBuilder At (GameObject gameObject) {
			if(gameObject == null)
				throw new System.ArgumentNullException();
			
			position = gameObject.transform.position;
			return this;
		}

		public FireBuilder At (Component component) {
			if(component == null)
				throw new System.ArgumentNullException();
			
			position = component.transform.position;
			return this;
		}

		public FireBuilder At (Vector2 position, 
		                       DanmakuField field, 
		                       DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View) {
			if(field == null)
				throw new System.ArgumentNullException();

			this.field = field;
			this.position = field.WorldPoint(position, coordSys);
			return this;
		}

		public FireBuilder Rotate(DynamicFloat rotation) {
			this.rotation = rotation;
			return this;
		}
		
		public FireBuilder Rotate (Transform transform) {
			if(transform == null)
				throw new System.ArgumentNullException();
			
			rotation = transform.Rotation2D();
			return this;
		}
		
		public FireBuilder Rotate (GameObject gameObject) {
			if(gameObject == null)
				throw new System.ArgumentNullException();
			
			rotation = gameObject.transform.Rotation2D();
			return this;
		}
		
		public FireBuilder Rotate (Component component) {
			if(component == null)
				throw new System.ArgumentNullException();
			
			rotation = component.transform.Rotation2D();
			return this;
		}

		public FireBuilder Transform (Transform transform) {
			return At (transform).Rotate (transform);
		}

		public FireBuilder Transform (GameObject gameObject) {
			if(gameObject == null)
				throw new System.ArgumentNullException();

			return Transform (gameObject.transform);
		}

		public FireBuilder Transform (Component component) {
			if(component == null)
				throw new System.ArgumentNullException();

			return Transform (component.transform);
		}

		public FireBuilder WithModifier(DanmakuModifier modifier) {
			modifiers.Push(modifier);
			return this;
		}

		public FireBuilder WithController (IDanmakuController controller) {
			this.controller += controller.Update;
			return this;
		}

		public FireBuilder WithController (IEnumerable<IDanmakuController> controllers) {
			var list = controllers as IList<IDanmakuController>;
			if(list != null) {
				int count = list.Count;
				for(int i = 0; i < count; i++) {
					IDanmakuController current = list[i];
					if(current != null)
						this.controller += current.Update;
				}
			} else {
				foreach(var controller in controllers) {
					if(controller != null)
						this.controller += controller.Update;
				}
			}
			return this;
		}

		public FireBuilder WithController (DanmakuController controller) {
			this.controller += controller;
			return this;
		}

		public FireBuilder WithController (IEnumerable<DanmakuController> controllers) {
			var list = controllers as IList<DanmakuController>;
			if(list != null) {
				int count = list.Count;
				for(int i = 0; i < count; i++) {
					DanmakuController current = list[i];
					if(current != null)
						this.controller += current;
				}
			} else {
				foreach(var controller in controllers) {
					if(controller != null)
						this.controller += controller;
				}
			}
			return this;
		}

		public FireBuilder WithoutControllers () {
			modifiers.Push(new ClearControllersModifier());
			return this;
		}

		public void Fire() {
			DanmakuModifier modifier = DanmakuModifier.Construct(modifiers);
			modifier.Initialize(new FireData() { 
				Prefab = prefab,
				Controller = controller
			});
			modifier.OnFire(position, rotation);
		}
	}
}

