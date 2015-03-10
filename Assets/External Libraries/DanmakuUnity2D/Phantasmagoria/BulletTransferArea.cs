using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D.Phantasmagoria {

	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(ProjectileTransferBoundary))]
	[RequireComponent(typeof(Collider2D))]
	public class BulletTransferArea : PausableGameObject {

		public void Run(float duration, float maxScale, DanmakuField origin, DanmakuField target) {
			ProjectileTransferBoundary ptb = GetComponent<ProjectileTransferBoundary> ();
			ptb.Field = origin;
			ptb.TargetField = target;
			StartCoroutine (Execute (duration, maxScale));
		}

		private IEnumerator Execute(float duration, float maxScale) {
			SpriteRenderer rend = GetComponent<SpriteRenderer> ();
			Vector3 maxScaleV = Vector3.one * maxScale;
			Vector3 startScale = Transform.localScale;
			Color spriteColor = rend.color;
			Color targetColor = spriteColor;
			targetColor.a = 0f;
			float dt = Util.TargetDeltaTime;
			float t = 0;
			while (t < 1f) {
				Transform.localScale = Vector3.Lerp(startScale, maxScaleV, t);
				rend.color = Color.Lerp(spriteColor, targetColor, t);
				yield return UtilCoroutines.AbstractProjectileController(this);
				t += dt / duration;
			}
			Destroy (GameObject);
		}
	}
}