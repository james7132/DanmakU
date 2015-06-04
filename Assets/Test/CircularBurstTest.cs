// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using DanmakU;
using DanmakU.Modifiers;

public class CircularBurstTest : MonoBehaviour {
	
	[SerializeField]
	private FrameCounter delay;
	
	[SerializeField]
	private DanmakuPrefab prefab;
	
	[SerializeField]
	private CircularBurstModifier burst;
	
	void Update() {
		if(delay.Tick()) {
			Danmaku.ConstructFire(prefab).WithModifier(burst).Fire();
		}
	}
	
}
