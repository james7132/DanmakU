// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections;
using UnityEngine;
using Hourai.DanmakU;
using Hourai.DanmakU.Modifiers;

public class BasicFireTest : MonoBehaviour {

    [SerializeField]
    private float delay;

	[SerializeField]
	private DanmakuPrefab prefab;

    private Task test;

    private void Start() {

        test = prefab.Infinite()
                    .From(gameObject.Descendants())
                    .WithSpeed(2)
                    .Delay(delay)
                    .RadialBurst(360, 10)
                    .Execute();
        Invoke("Test", 10);
    }

    void Test() {
        test.Stop();
    }

}