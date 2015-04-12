using UnityEngine;
using System.Collections;
using Danmaku2D;

public class ParentPlayer : MonoBehaviour {

	[SerializeField]
	private Vector2 offset;

	// Use this for initialization
	void Start () {
		DanmakuField field = DanmakuField.FindClosest (transform.position);
		transform.parent = field.player.transform;
		transform.localPosition = offset;
	}
}
