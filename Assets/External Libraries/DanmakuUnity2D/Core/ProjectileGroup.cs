using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Danmaku2D {

	public sealed class ProjectileGroup : ICollection<Projectile> {

		internal HashSet<Projectile> group;

		public IProjectileController Controller;

		public ProjectileGroup() {
			group = new HashSet<Projectile> ();
		}

		#region ICollection implementation

		public void Add (Projectile item) {
			bool added = group.Add(item);
			if (added) {
				item.groups.Add (this);
				item.groupCountCache++;
				item.groupCheck = item.groups.Count > 0;
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

