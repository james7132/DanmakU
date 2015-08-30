// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections;
using UnityEngine;
using DanmakU;

public class BasicFireTest : MonoBehaviour {

    [SerializeField]
    private float delay;

	[SerializeField]
	private DanmakuPrefab prefab;

    private Task test;

    private void Start() {
        test = prefab.Infinite().From(this).InDirection(new DFloat(0f, 90f)).Delay(delay).Fire();
        Invoke("Test", 10);
    }

    void Test() {
        test.Stop();
    }

}