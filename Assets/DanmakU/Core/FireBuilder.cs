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
	[System.Serializable]
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

	public class FireBuilder {

		private FireData data;

		public DanmakuPrefab Prefab {
			get {
				return data.Prefab;
			}
			set {
				data.Prefab = value;
			}
		}

		public DanmakuField Field {
			get {
				return data.Field;
			}
			set {
				data.Field = value;
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

		public event DanmakuController Controller {
			add {
				data.Controller += value;
			}
			remove {
				data.Controller -= value;
			}
		}

		public float Speed {
			get {
				return data.Speed;
			}
			set {
				data.Speed = value;
			}
		}

		public float AngularSpeed {
			get {
				return data.AngularSpeed;
			}
			set {
				data.AngularSpeed = value;
			}
		}

		private List<DanmakuModifier> modifiers;

		private Transform positionSource;
		private Transform rotationSource;

		private bool targeted;
		private Vector2? targetPosition;
		private Transform targetObject;
		
		protected internal FireBuilder(DanmakuPrefab prefab, DanmakuField field = null) {
			data = new FireData() { 
				Prefab = prefab,
				Field = field
			};
			modifiers = new List<DanmakuModifier>();
		}

		#region Position Functions

		public FireBuilder From (Vector2 position) {
			this.position = position;
			positionSource = null;
			return this;
		}

		public FireBuilder From (Transform transform) {
			if(transform == null)
				throw new System.ArgumentNullException();

			positionSource  = transform;
			return this;
		}

		public FireBuilder From (GameObject gameObject) {
			if(gameObject == null)
				throw new System.ArgumentNullException();

			positionSource = gameObject.transform;
			return this;
		}

		public FireBuilder From (Component component) {
			if(component == null)
				throw new System.ArgumentNullException();

			positionSource = component.transform;
			return this;
		}

		public FireBuilder From (Vector2 position, 
		                       DanmakuField field, 
		                       DanmakuField.CoordinateSystem coordSys = DanmakuField.CoordinateSystem.View) {
			if(field == null)
				throw new System.ArgumentNullException();

			this.Field = field;
			this.position = field.WorldPoint(position, coordSys);
			return this;
		}

		#endregion

		public FireBuilder Facing (Vector2 direction) {
			targeted = false;
			targetPosition = null;
			targetObject = null;
			Rotation = Mathf.Rad2Deg * Mathf.Atan2 (direction.y, direction.x);
			return this;
		}

		public FireBuilder Towards (Vector2 position) {
			targeted = true;
			targetPosition = position;
			return this;
		}

		public FireBuilder Towards (Transform transform) {
			if (transform == null)
				throw new System.ArgumentNullException ();

			targeted = true;
			targetPosition = null;
			targetObject = transform;
			return this;
		}

		public FireBuilder Towards (Component component) {
			if (component == null)
				throw new System.ArgumentNullException ();
			
			targeted = true;
			targetPosition = null;
			targetObject = component.transform;
			return this;
		}

		public FireBuilder Towards (GameObject gameObject) {
			if (gameObject == null)
				throw new System.ArgumentNullException ();
			
			targeted = true;
			targetPosition = null;
			targetObject = gameObject.transform;
			return this;
		}

		public FireBuilder WithRotation (DynamicFloat rotation) {
			this.rotation = rotation;
			return this;
		}
		
		public FireBuilder WithRotation (Transform transform) {
			if(transform == null)
				throw new System.ArgumentNullException();
			
			rotationSource = transform;
			return this;
		}
		
		public FireBuilder WithRotation (GameObject gameObject) {
			if(gameObject == null)
				throw new System.ArgumentNullException();
			
			rotationSource = gameObject.transform;
			return this;
		}
		
		public FireBuilder WithRotation (Component component) {
			if(component == null)
				throw new System.ArgumentNullException();
			
			rotationSource = component.transform;
			return this;
		}

		public FireBuilder Transform (Transform transform) {
			return From (transform).WithRotation (transform);
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
			modifiers.Add(modifier);
			return this;
		}

		public FireBuilder WithController (IDanmakuController controller) {
			this.Controller += controller.Update;
			return this;
		}

		public FireBuilder WithController (IEnumerable<IDanmakuController> controllers) {
			var list = controllers as IList<IDanmakuController>;
			if(list != null) {
				int count = list.Count;
				for(int i = 0; i < count; i++) {
					IDanmakuController current = list[i];
					if(current != null)
						this.Controller += current.Update;
				}
			} else {
				foreach(var controller in controllers) {
					if(controller != null)
						this.Controller += controller.Update;
				}
			}
			return this;
		}

		public FireBuilder WithController (DanmakuController controller) {
			this.Controller += controller;
			return this;
		}

		public FireBuilder WithController (IEnumerable<DanmakuController> controllers) {
			var list = controllers as IList<DanmakuController>;
			if(list != null) {
				int count = list.Count;
				for(int i = 0; i < count; i++) {
					DanmakuController current = list[i];
					if(current != null)
						this.Controller += current;
				}
			} else {
				foreach(var controller in controllers) {
					if(controller != null)
						this.Controller += controller;
				}
			}
			return this;
		}

		public FireBuilder WithoutControllers () {
			data.Controller = null;
			return this;
		}

		public void Fire () {
			DanmakuModifier modifier = DanmakuModifier.Construct(modifiers);
			Vector2 actualPosition = position;
			DynamicFloat actualRotation = rotation;

			if (positionSource != null)
				actualPosition = positionSource.position;

			if(rotationSource != null)
				actualRotation = rotationSource.Rotation2D();

			if (targeted) {
				if(targetPosition != null)
					actualRotation += DanmakuUtil.AngleBetween2D(position, (Vector2)targetPosition);
				else if (targetObject != null)
					actualRotation += DanmakuUtil.AngleBetween2D(position, targetObject.position);
			}

			modifier.Initialize (data);
			modifier.OnFire(actualPosition, actualRotation);
		}
	}
}

