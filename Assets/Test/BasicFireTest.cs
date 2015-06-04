// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using DanmakU;

public class BasicFireTest : MonoBehaviour {

	[SerializeField]
	private FrameCounter delay;

	[SerializeField]
	private DanmakuPrefab prefab;

	void Update() {
		if(delay.Tick()) {
			Danmaku.Fire(prefab);
		}
	}

}
