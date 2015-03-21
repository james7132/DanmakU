using UnityEngine;
using System.Collections.Generic;

namespace Danmaku2D.ProjectileControllers {

	public class AggregateProjectileController : IProjectileController, ICollection<IProjectileController> {

		private List<IProjectileController> children;

		public AggregateProjectileController(params IProjectileController[] children) {
			this.children = new List<IProjectileController> (children);
		}

		#region IProjectileController implementation

		public void UpdateProjectile (Projectile projectile, float dt) {
			for(int i = 0; i < children.Count; i++) {
				children[i].UpdateProjectile(projectile, dt);
			}
		}

		#endregion

		#region ICollection implementation

		public void Add (IProjectileController item) {
			children.Add (item);
		}

		public void Clear () {
			children.Clear ();
		}

		public bool Contains (IProjectileController item) {
			return children.Contains (item);
		}

		public void CopyTo (IProjectileController[] array, int arrayIndex) {
			children.CopyTo (array, arrayIndex);
		}

		public bool Remove (IProjectileController item) {
			return children.Remove (item);
		}

		public int Count {
			get {
				return children.Count;
			}
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		#endregion

		#region IEnumerable implementation

		public IEnumerator<IProjectileController> GetEnumerator () {
			return children.GetEnumerator ();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
			return children.GetEnumerator ();
		}

		#endregion
	}
}