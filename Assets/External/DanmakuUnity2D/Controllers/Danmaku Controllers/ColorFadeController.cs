using UnityEngine;
using System.Collections;

namespace Danmaku2D.DanmakuControllers {

	[System.Serializable]
	public class ColorFadeController : IDanmakuController {

		[SerializeField]
		private Color endColor;

		[SerializeField]
		private float startTime;

		[SerializeField]
		private float endTime;

		public void UpdateDanmaku (Danmaku projectile, float dt) {
			float bulletTime = projectile.Time;
			Color startColor = projectile.Prefab.cachedColor;
//			Debug.Log (bulletTime);
			if (bulletTime < startTime)
				projectile.Color = startColor;
			else if (bulletTime > endTime)
				projectile.Color = endColor;
			else {
				if(endTime <= startTime)
					projectile.color = endColor;
				else
					projectile.Color = Color.Lerp (startColor, endColor, (bulletTime - startTime) / (endTime - startTime));
			}
		}
	}

	namespace Wrapper {

		[AddComponentMenu("Danmaku 2D/Controllers/Color Fade Controller")]
		internal class ColorFadeController : ControllerWrapperBehavior<Danmaku2D.DanmakuControllers.ColorFadeController> {
		}
	}
}