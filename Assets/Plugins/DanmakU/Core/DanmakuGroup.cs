// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilLib;

namespace DanmakU {

	public sealed class DanmakuGroup : HashSet<Danmaku> {

		internal DanmakuController groupControllers;

		public int Damage {
			set {
				foreach(Danmaku danmaku in this) {
					danmaku.Damage = value;
				}
			}
		}

		public Color Color {
			set {
				foreach(Danmaku danmaku in this) {
					danmaku.Color = value;
				}
			}
		}

		public Vector2 Position {
			set {
				foreach(Danmaku danmaku in this) {
					danmaku.Position = value;
				}
			}
		}

		public DynamicFloat Rotation {
			set {
				foreach(Danmaku danmaku in this) {
					danmaku.Rotation = value.Value;
				}
			}
		}

		public string Tag {
			set {
				foreach(Danmaku danmaku in this) {
					danmaku.Tag = value;
				}
			}
		}

		public int Layer {
			set {
				foreach(Danmaku danmaku in this) {
					danmaku.Layer = value;
				}
			}
		}

		public bool BoundsCheck {
			set {
				foreach(Danmaku danmaku in this) {
					danmaku.BoundsCheck = value;
				}
			}
		}

		public bool CollisionCheck {
			set {
				foreach(Danmaku danmaku in this) {
					danmaku.CollisionCheck = value;
				}
			}
		}

		public void Rotate(DynamicFloat delta) {
			foreach(Danmaku danmaku in this) {
				danmaku.Rotate(delta);
			}
		}

		public void MatchPrefab(DanmakuPrefab prefab) {
			foreach (Danmaku danmaku in this) {
				danmaku.MatchPrefab(prefab);
			}
		}

		public void AddController(IDanmakuController controller) {
			groupControllers -= controller.UpdateDanmaku;
			foreach(Danmaku proj in this) {
				proj.AddController(controller.UpdateDanmaku);
			}
		}

		public void RemoveController(IDanmakuController controller) {
			groupControllers -= controller.UpdateDanmaku;
			foreach(Danmaku proj in this) {
				proj.RemoveController(controller.UpdateDanmaku);
			}
		}

		public void ClearControllers() {
			foreach(Danmaku danmaku in this) {
				danmaku.ClearControllers();
			}
		}

		public void ClearGroupControllers() {
			foreach(Danmaku danmaku in this) {
				danmaku.RemoveController(groupControllers);
			}
			groupControllers = null;
		}

		public new void Add (Danmaku item) {
			bool added = base.Add(item);
			if (added) {
				item.groups.Add (this);
				item.groupCountCache++;
				item.groupCheck = item.groups.Count > 0;
				item.AddController(groupControllers);
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
				item.RemoveController(groupControllers);
			}
			return success;
		}

	}
}

