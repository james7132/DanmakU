// Copyright (C) 2015  James Liu
//	
//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.
//		
//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.
//			
//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>

using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {

	[System.Serializable]
	public abstract class DanmakuModifier {

		[SerializeField]
		private DanmakuModifier subModifier;
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

		public DanmakuModifier SubModifier {
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
