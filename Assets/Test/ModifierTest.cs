// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using DanmakU;
using DanmakU.Modifiers;

public class ModifierTest : MonoBehaviour {
	
	[SerializeField]
	private FrameCounter delay;
	
	[SerializeField]
	private DanmakuPrefab prefab;
	
	[SerializeField]
	private CircularBurstModifier circleBurst;

	[SerializeField]
	private LinearBurstModifier lineBurst;
	
	void Update() {
		if(delay.Tick()) {
			Danmaku.ConstructFire(prefab)
				   .WithModifier(circleBurst)
				   .WithModifier(lineBurst)
				   .Fire();
		}
	}
	
}
