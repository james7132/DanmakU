// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;
using DanmakU;

public class ParentPlayer : MonoBehaviour {

	[SerializeField]
	private Vector2 offset;

	// Use this for initialization
	void Start () {
		DanmakuField field = DanmakuField.FindClosest (this);
		transform.parent = field.player.transform;
		transform.localPosition = offset;
	}
}
