using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Danmaku2D {
	[RequireComponent(typeof(Collider2D))]
	public class ProjectileBoundary : MonoBehaviour {

		[SerializeField]
		private string tagFilter;

		private List<string> validTags;

		void Awake() {
			if(tagFilter == null)
				tagFilter = "";
			validTags = new List<string> ();
			validTags.AddRange(tagFilter.Split ('|'));
		}

		void OnProjectileCollision(Projectile proj) {
			if(proj != null) {
				ProcessProjectile(proj);
				proj.Deactivate();
			}
		}

		protected virtual void ProcessProjectile(Projectile proj) {
			proj.Deactivate();
		}
	}
}