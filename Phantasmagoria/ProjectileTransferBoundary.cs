using UnityEngine;
using System.Collections;

public class ProjectileTransferBoundary : ProjectileBoundary {

	[SerializeField]
	private AbstractDanmakuField field;
	public AbstractDanmakuField Field {
		get {
			return field;
		}
		set {
			field = value;
		}
	}

	[SerializeField]
	private AbstractDanmakuField targetField;
	public AbstractDanmakuField TargetField {
		get {
			return targetField;
		}
		set {
			targetField = value;
		}
	}

	protected override void ProcessProjectile (Projectile proj) {
		if (field != null && targetField != null) {
			Vector2 relativePos = field.FieldPoint(proj.Transform.position);
			proj.Transform.position = targetField.WorldPoint(relativePos);
		}
	}
}
