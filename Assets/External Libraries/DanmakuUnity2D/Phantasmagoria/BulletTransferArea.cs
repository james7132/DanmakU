using UnityEngine;
using UnityUtilLib;
using System.Collections;

namespace Danmaku2D.Phantasmagoria {

	[DisallowMultipleComponent]
	[RequireComponent(typeof(ProjectileTransferBoundary))]
	[RequireComponent(typeof(Collider2D))]
	public class BulletTransferArea : PausableGameObject {

		public void Run(float duration, float maxScale, DanmakuField origin) {
			ProjectileTransferBoundary ptb = GetComponent<ProjectileTransferBoundary> ();
			ptb.Field = origin;
			StartCoroutine (Execute (duration, maxScale));
		}

		private IEnumerator Execute(float duration, float maxScale) {
			SpriteRenderer rend = GetComponent<SpriteRenderer> ();
			Vector3 maxScaleV = Vector3.one * maxScale;
			Vector3 startScale = transform.localScale;
			Color spriteColor = rend.color;
			Color targetColor = spriteColor;
			targetColor.a = 0f;
			float dt = Util.TargetDeltaTime;
			float t = 0;
			while (t < 1f) {
				transform.localScale = Vector3.Lerp(startScale, maxScaleV, t);
				rend.color = Color.Lerp(spriteColor, targetColor, t);
				yield return UtilCoroutines.WaitForUnpause(this);
				t += dt / duration;
			}
			Destroy (gameObject);
		}
	}
}