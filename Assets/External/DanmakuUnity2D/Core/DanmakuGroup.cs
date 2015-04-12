using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D {

	public sealed class DanmakuGroup : ICollection<Danmaku> {

		internal HashSet<Danmaku> group;

		internal DanmakuController groupControllers;

		public DanmakuGroup() {
			group = new HashSet<Danmaku> ();
		}

		public int Damage {
			set {
				foreach(Danmaku danmaku in group) {
					danmaku.Damage = value;
				}
			}
		}

		public Sprite Sprite {
			set {
				foreach(Danmaku danmaku in group) {
					danmaku.Sprite = value;
				}
			}
		}

		public Color Color {
			set {
				foreach(Danmaku danmaku in group) {
					danmaku.Color = value;
				}
			}
		}

		public Vector2 Position {
			set {
				foreach(Danmaku danmaku in group) {
					danmaku.Position = value;
				}
			}
		}

		public Vector2 PositionImmediate {
			set {
				foreach(Danmaku danmaku in group) {
					danmaku.PositionImmediate = value;
				}
			}
		}

		public DynamicFloat Rotation {
			set {
				foreach(Danmaku danmaku in group) {
					danmaku.Rotation = value.Value;
				}
			}
		}

		public string Tag {
			set {
				foreach(Danmaku danmaku in group) {
					danmaku.Tag = value;
				}
			}
		}

		public int Layer {
			set {
				foreach(Danmaku danmaku in group) {
					danmaku.Layer = value;
				}
			}
		}

		public bool BoundsCheck {
			set {
				foreach(Danmaku danmaku in group) {
					danmaku.BoundsCheck = value;
				}
			}
		}

		public bool CollisionCheck {
			set {
				foreach(Danmaku danmaku in group) {
					danmaku.CollisionCheck = value;
				}
			}
		}

		public void Rotate(DynamicFloat delta) {
			foreach(Danmaku danmaku in group) {
				danmaku.Rotate(delta);
			}
		}

		public void MatchPrefab(DanmakuPrefab prefab) {
			foreach (Danmaku danmaku in group) {
				danmaku.MatchPrefab(prefab);
			}
		}

		public void AddToGroup(DanmakuGroup otherGroup) {
			if(this == otherGroup)
				return;
			foreach(Danmaku danmaku in group) {
				danmaku.AddToGroup(otherGroup);
			}
		}

		public void RemoveFromGroup(DanmakuGroup otherGroup) {
			if (this == otherGroup) {
				Clear();
				return;
			}
			foreach(Danmaku danmaku in group) {
				danmaku.RemoveFromGroup(otherGroup);
			}
		}

		public void AddController(IDanmakuController controller) {
			groupControllers -= controller.UpdateDanmaku;
			foreach(Danmaku proj in group) {
				proj.AddController(controller.UpdateDanmaku);
			}
		}

		public void RemoveController(IDanmakuController controller) {
			groupControllers -= controller.UpdateDanmaku;
			foreach(Danmaku proj in group) {
				proj.RemoveController(controller.UpdateDanmaku);
			}
		}

		public void ClearControllers() {
			foreach(Danmaku danmaku in group) {
				danmaku.ClearControllers();
			}
		}

		public void ClearGroupControllers() {
			foreach(Danmaku danmaku in group) {
				danmaku.RemoveController(groupControllers);
			}
			groupControllers = null;
		}

		public Dictionary<Danmaku, T> AddComponent<T>() where T : Component {
			var pairs = new Dictionary<Danmaku, T> ();
			foreach (Danmaku danmaku in group) {
				pairs[danmaku] = danmaku.AddComponent<T>();
			}
			return pairs;
		}

		#region ICollection implementation

		public void Add (Danmaku item) {
			bool added = group.Add(item);
			if (added) {
				item.groups.Add (this);
				item.groupCountCache++;
				item.groupCheck = item.groups.Count > 0;
				item.AddController(groupControllers);
			}
		}

		public void Clear () {
			foreach(Danmaku proj in group) {
				proj.RemoveFromGroup(this);
			}
		}

		public bool Contains (Danmaku item) {
			return group.Contains (item);
		}

		public void CopyTo (Danmaku[] array, int arrayIndex) {
			group.CopyTo (array, arrayIndex);
		}

		public bool Remove (Danmaku item) {
			bool success = false;
			success = group.Remove(item);
			if (success) {
				item.groups.Remove (this);
				item.groupCountCache--;
				item.groupCheck = item.groups.Count > 0;
				item.RemoveController(groupControllers);
			}
			return success;
		}

		public int Count {
			get {
				return group.Count;
			}
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		#endregion

		#region IEnumerable implementation

		public IEnumerator<Danmaku> GetEnumerator () {
			return group.GetEnumerator ();
		}

		#endregion

		#region IEnumerable implementation

		IEnumerator IEnumerable.GetEnumerator () {
			return group.GetEnumerator ();
		}

		#endregion




	}
}

