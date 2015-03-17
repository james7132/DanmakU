using UnityEngine;
using System.Collections;

namespace Danmaku2D.Phantasmagoria {
	public class ProjectileTransferBoundary : ProjectileBoundary {

		[SerializeField]
		private PhantasmagoriaField field;
		public PhantasmagoriaField Field {
			get {
				return field;
			}
			set {
				field = value;
			}
		}

		protected override void ProcessProjectile (Projectile proj) {
			if (field != null) {
				field.Transfer(proj);
			}
		}
	}
}