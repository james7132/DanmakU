using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Danmaku2D {

	public sealed class ProjectileGroup : ICollection<Projectile> {

		internal HashSet<Projectile> group;

		internal HashSet<IProjectileController> controllers;

		public ProjectileGroup() {
			group = new HashSet<Projectile> ();
			controllers = new HashSet<IProjectileController> ();
		}

		public void AddController(IProjectileController controller) {
			if (controllers.Add (controller)) {
				foreach(Projectile proj in group) {
					proj.AddController(controller);
				}
			}
		}

		public void RemoveController(IProjectileController controller) {
			if (controllers.Remove (controller)) {
				foreach(Projectile proj in group) {
					proj.RemoveController(controller);
				}
			}
		}

		public void ClearControllers() {
			foreach (IProjectileController controller in controllers) {
				foreach(Projectile proj in group) {
					proj.RemoveController(controller);
				}
			}
			controllers.Clear ();
		}

		public bool UsesController(IProjectileController controller) {
			return controllers.Contains (controller);
		}

		public int ControllerCount {
			get {
				return controllers.Count;
			}
		}

		#region ICollection implementation

		public void Add (Projectile item) {
			bool added = group.Add(item);
			if (added) {
				item.groups.Add (this);
				item.groupCountCache++;
				item.groupCheck = item.groups.Count > 0;
				foreach(IProjectileController controller in controllers) {
					item.AddController(controller);
				}
			}
		}

		public void Clear () {
			foreach(Projectile proj in group) {
				proj.RemoveFromGroup(this);
			}
		}

		public bool Contains (Projectile item) {
			return group.Contains (item);
		}

		public void CopyTo (Projectile[] array, int arrayIndex) {
			group.CopyTo (array, arrayIndex);
		}

		public bool Remove (Projectile item) {
			bool success = false;
			success = group.Remove(item);
			if (success) {
				item.groups.Remove (this);
				item.groupCountCache--;
				item.groupCheck = item.groups.Count > 0;
				foreach(IProjectileController controller in controllers) {
					item.RemoveController(controller);
				}
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

		public IEnumerator<Projectile> GetEnumerator () {
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

