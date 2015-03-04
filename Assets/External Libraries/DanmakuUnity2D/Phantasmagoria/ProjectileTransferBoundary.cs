using UnityEngine;
using System.Collections;

namespace Danmaku2D.Phantasmagoria {
	public class ProjectileTransferBoundary : ProjectileBoundary {

		[SerializeField]
		private DanmakuField field;
		public DanmakuField Field {
			get {
				return field;
			}
			set {
				field = value;
			}
		}

		[SerializeField]
		private DanmakuField targetField;
		public DanmakuField TargetField {
			get {
				return targetField;
			}
			set {
				targetField = value;
			}
		}

		protected override void ProcessProjectile (Projectile proj) {
			if (field != null && targetField != null) {
				Vector2 relativePos = field.ViewPoint(proj.Position);
				proj.Position = targetField.WorldPoint(relativePos);
			}
		}
	}
}