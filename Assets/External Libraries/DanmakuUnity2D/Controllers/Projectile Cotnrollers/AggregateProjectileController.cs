using UnityEngine;
using System.Collections.Generic;

namespace Danmaku2D.ProjectileControllers {

	public class AggregateProjectileController : ProjectileController, ICollection<IProjectileController> {

		private List<IProjectileController> children;

		public AggregateProjectileController(params IProjectileController[] children) {
			this.children = new List<IProjectileController> (children);
		}

		#region implemented abstract members of ProjectileController
		public override void UpdateProjectile (float dt) {
			for(int i = 0; i < children.Count; i++) {
				children[i].Projectile = Projectile;
				children[i].UpdateProjectile(dt);
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