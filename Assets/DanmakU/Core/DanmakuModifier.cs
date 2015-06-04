// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections.Generic;

namespace DanmakU {

	[System.Serializable]
	public abstract class DanmakuModifier : IEnumerable<DanmakuModifier> {

		[SerializeField]
		private DanmakuModifier subModifier;
		private FireData data;

		protected DynamicFloat Speed {
			get {
				return data.Speed;
			}
			set {
				data.Speed = value;
				if(subModifier != null)
					subModifier.Speed = value;
			}
		}

		protected DynamicFloat AngularSpeed {
			get {
				return data.AngularSpeed;
			}
			set {
				data.AngularSpeed = value;
				if(subModifier != null)
					subModifier.AngularSpeed = value;
			}
		}

		protected DanmakuField Field {
			get {
				return data.Field;
			}
			set {
				data.Field = value;
				if (subModifier != null)
					subModifier.Field = value;
			}
		}

		protected DanmakuController Controller {
			get {
				return data.Controller;
			}
			set {
				data.Controller = value;
				if (subModifier != null)
					subModifier.Controller = value;
			}
		}

		protected DanmakuPrefab Prefab {
			get {
				return data.Prefab;
			}
			set {
				data.Prefab = value;
				if (subModifier != null)
					subModifier.Prefab = value;
			}
		}

		protected DanmakuGroup Group {
			get {
				return data.Group;
			}
			set {
				data.Group = value;
				if (subModifier != null)
					subModifier.Group = value;
			}
		}

		public DanmakuModifier SubModifier {
			get {
				return subModifier;
			}
			set {
				subModifier = value;
				if(subModifier != null)
					subModifier.Initialize(data);
			}
		}

		internal void Initialize(FireData data) {
			this.data = data;
			if (subModifier != null)
				subModifier.Initialize (data);
			OnInitialize ();
		}

		protected virtual void OnInitialize() {
		}

		public static DanmakuModifier Construct (IEnumerable<DanmakuModifier> enumerable) {
			if (enumerable == null)
				throw new System.ArgumentNullException ();
			if (enumerable is DanmakuModifier)
				return enumerable as DanmakuModifier;
			DanmakuModifier top = null;
			DanmakuModifier current = null;
			foreach (var next in enumerable) {
				if(next != null) {
					if(top == null)
						top = next;
					else
						current.subModifier = next;
					current = next;
				}
			}
			return top;
		}

		public void Insert (DanmakuModifier newModifier) {
			if (newModifier == null)
				throw new System.ArgumentNullException ();
			if (subModifier == null)
				subModifier = newModifier;
			else {
				newModifier.subModifier = subModifier;
				subModifier = newModifier;
			}
		}

		public void Append(DanmakuModifier newModifier) {
			DanmakuModifier parent = this;
			DanmakuModifier current = subModifier;
			while (current != null) {
				current = current.subModifier;
			}
			parent.SubModifier = newModifier;
		}

		protected void FireSingle(Vector2 position,
		                          DynamicFloat rotation) {
			if (SubModifier == null) {
				data.Position = position;
				data.Rotation = rotation;
				data.Fire();
			} else {
				SubModifier.OnFire (position, rotation);
			}
		}

		public void Fire(FireData data) {
			Initialize(data);
			OnFire(data.Position, data.Rotation);
		}

		public abstract void OnFire(Vector2 position, DynamicFloat rotation);

		#region IEnumerable implementation

		public IEnumerator<DanmakuModifier> GetEnumerator () {
			DanmakuModifier current = this;
			while (current != null) {
				yield return current;
				current = current.subModifier;
			}
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
			DanmakuModifier current = this;
			while (current != null) {
				yield return current;
				current = current.subModifier;
			}
		}

		#endregion
	}

}
