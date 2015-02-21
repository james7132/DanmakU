using UnityEngine;
using System.Collections;
using UnityUtilLib;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(ProjectileTransferBoundary))]
[RequireComponent(typeof(Collider2D))]
public class BulletTransferArea : CachedObject {

	public void Run(float duration, float maxScale, AbstractDanmakuField origin, AbstractDanmakuField target) {
		ProjectileTransferBoundary ptb = GetComponent<ProjectileTransferBoundary> ();
		ptb.Field = origin;
		ptb.TargetField = target;
		StartCoroutine (Execute (duration, maxScale));
	}
	
	/// <summary>
	/// Execute this instance.
	/// </summary>
	private IEnumerator Execute(float duration, float maxScale) {
		SpriteRenderer rend = GetComponent<SpriteRenderer> ();
		Vector3 maxScaleV = Vector3.one * maxScale;
		Vector3 startScale = Transform.localScale;
		Color spriteColor = rend.color;
		Color targetColor = spriteColor;
		targetColor.a = 0f;
		float t = 0;
		while (t < 1f) {
			Transform.localScale = Vector3.Lerp(startScale, maxScaleV, t);
			rend.color = Color.Lerp(spriteColor, targetColor, t);
			yield return new WaitForFixedUpdate();
			t += Time.fixedDeltaTime / duration;
		}
		Destroy (GameObject);
	}

}