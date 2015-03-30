using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D.ProjectileControllers {

	public class DelayedAngleChange : ProjectileControlBehavior {

		private enum RotationMode { Absolute, Relative, Player }

		[SerializeField]
		private RotationMode rotationMode;

		[SerializeField]
		private float delay;

		[SerializeField]
		private DynamicFloat angle;

		#region implemented abstract members of ProjectileControlBehavior

		public override void UpdateProjectile (Danmaku projectile, float dt) {
			float time = projectile.Time;
			if(time >= delay && time - dt <= delay) {
				float baseAngle = angle.Value;
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
				projectile.Rotation = baseAngle;
			}
		}

		#endregion
		
	}
}