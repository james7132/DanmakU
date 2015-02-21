using UnityEngine;
using System.Collections;
using UnityUtilLib;

public abstract class AbstractMovementPattern : CachedObject {
	
	/// <summary>
	/// The destroy on end.
	/// </summary>
	[SerializeField]
	private bool destroyOnEnd;
	public bool DestroyOnEnd {
		get {
			return destroyOnEnd;
		}
		set {
			destroyOnEnd = value;
		}
	}

	public void StartMovement() {
		StartCoroutine (MoveImpl ());
	}

	private IEnumerator MoveImpl() {
		yield return StartCoroutine(Move());
		if(destroyOnEnd) {
			Destroy (gameObject);
		}
	}

	protected abstract IEnumerator Move();
}
