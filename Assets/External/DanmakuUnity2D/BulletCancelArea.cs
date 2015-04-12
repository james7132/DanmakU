using UnityEngine;
using System.Collections;
using UnityUtilLib;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {
	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(DanmakuBoundary))]
	[RequireComponent(typeof(Collider2D))]
	public class BulletCancelArea : PausableGameObject {
		
		public void Run(float duration, float maxScale) {
			StartCoroutine (Execute (duration, maxScale));
		}

		/// <summary>
		/// Execute this instance.
		/// </summary>
		private IEnumerator Execute(float duration, float maxScale) {
			SpriteRenderer rend = GetComponent<SpriteRenderer> ();
			Vector3 maxScaleV = Vector3.one * maxScale;
			Vector3 startScale = transform.localScale;
			Color spriteColor = rend.color;
			Color targetColor = spriteColor;
			targetColor.a = 0f;
			float t = 0f;
			float dt = Util.TargetDeltaTime;
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