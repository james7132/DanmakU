using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {

	[System.Serializable]
	public abstract class FireModifier {

		private FireModifier subModifier;
		private FireBuilder builder;

		protected DynamicFloat Velocity {
			get {
				return builder.Velocity;
			}
			set {
				builder.Velocity = value;
				if(subModifier != null) {
					subModifier.Velocity = value;
				}
			}
		}

		protected DynamicFloat AngularVelocity {
			get {
				return builder.AngularVelocity;
			}
			set {
				builder.AngularVelocity = value;
				if(subModifier != null) {
					subModifier.AngularVelocity = value;
				}
			}
		}

		protected DanmakuField TargetField {
			get;
			private set;
		}

		protected DanmakuController Controller {
			get {
				return builder.Controller;
			}
		}

		protected DanmakuPrefab BulletType {
			get {
				return builder.Prefab;
			}
		}

		protected DanmakuGroup Group {
			get {
				return builder.Group;
			}
		}

		public FireModifier SubModifier {
			get {
				return subModifier;
			}
			set {
				subModifier = value;
				if(subModifier == null)
					subModifier.Initialize(BulletType, Velocity, AngularVelocity, TargetField, Controller, Group);
			}
		}

		internal void Initialize(DanmakuPrefab prefab,
		                         float velocity,
		                         float angularVelocity,
				                 DanmakuField field,
				                 DanmakuController controller,
		                         DanmakuGroup group) {
			TargetField = field;
			builder = new FireBuilder (prefab);
			builder.Velocity = velocity;
			builder.AngularVelocity = angularVelocity;
			builder.Controller = controller;
			builder.Group = group;
			builder.CoordinateSystem = DanmakuField.CoordinateSystem.World;
			if (subModifier != null)
				subModifier.Initialize (builder, field);
			OnInitialize ();
		}

		internal void Initialize (FireBuilder builder, DanmakuField field) {
			TargetField = field;
			this.builder = builder.Clone ();
			this.builder.CoordinateSystem = DanmakuField.CoordinateSystem.World;
			this.builder.Modifier = null;
			if (subModifier != null)
				subModifier.Initialize (builder, field);
			OnInitialize ();
		}

		protected virtual void OnInitialize() {
		}

		protected void FireSingle(Vector2 position,
		                          DynamicFloat rotation) {
			if (SubModifier == null) {
				builder.Position = position;
				builder.Rotation = rotation;
				TargetField.Fire(builder);
			} else {
				SubModifier.Fire (position, rotation);
			}
		}

		public abstract void Fire(Vector2 position, DynamicFloat rotation);
	}

}
