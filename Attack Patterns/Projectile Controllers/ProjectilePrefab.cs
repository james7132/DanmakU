using UnityEngine;
using System.Collections;
using UnityUtilLib;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ProjectilePrefab : CachedObject {

	[SerializeField]
	private CircleCollider2D circleCollider;
	public CircleCollider2D CircleCollider {
		get {
			return circleCollider;
		}
	}

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	public SpriteRenderer SpriteRenderer {
		get {
			return spriteRenderer;
		}
	}

	public override void Awake()
	{
		base.Awake ();
		if(circleCollider == null)
			circleCollider = GetComponent<CircleCollider2D> ();
		if(spriteRenderer == null)
			spriteRenderer = GetComponent<SpriteRenderer> ();
#if UNITY_EDITOR
		if(circleCollider == null)
			Debug.Log("Need circle collider on projectile prefab");
#endif
	}
}
