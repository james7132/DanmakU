using UnityEngine;
using System.Collections;

namespace Danmaku2D.ProjectileControllers {

	public class ColorFadeProjectile : ProjectileControlBehavior {

		[SerializeField]
		private Color endColor;

		[SerializeField]
		private float startTime;

		[SerializeField]
		private float endTime;

		public override void UpdateProjectile (Projectile projectile, float dt) {
			float bulletTime = projectile.Time;
			Color startColor = SpriteRenderer.color;
			if (bulletTime < startTime)
				projectile.Color = startColor;
			else if (bulletTime > endTime)
				projectile.Color = endColor;
			else
				projectile.Color = Color.Lerp (startColor, endColor, (bulletTime - startTime) / (endTime - startTime));
		}

	}
}