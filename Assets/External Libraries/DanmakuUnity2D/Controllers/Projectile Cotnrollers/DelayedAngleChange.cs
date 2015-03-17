using UnityEngine;
using System.Collections;

namespace Danmaku2D.ProjectileControllers {

	public class DelayedAngleChange : ProjectileControlBehavior {

		private enum RotationMode { Absolute, Relative, Player }

		[SerializeField]
		private RotationMode rotationMode;

		[SerializeField]
		private float delay;

		[SerializeField]
		private float angle;

		[SerializeField]
		private float range;

		#region implemented abstract members of ProjectileControlBehavior

		public override void UpdateProjectile (Projectile projectile, float dt) {
			float time = projectile.Time;
			if(time >= delay && time - dt <= delay) {
				float baseAngle = angle;
				switch(rotationMode) {
					case RotationMode.Relative:
						baseAngle += projectile.Rotation;
						break;
					case RotationMode.Player:
						baseAngle += projectile.Field.AngleTowardPlayer(projectile.Position);
						break;
					case RotationMode.Absolute:
						break;
				}
				projectile.Rotation = Random.Range(baseAngle - range, baseAngle + range);
			}
		}

		#endregion
		
	}
}