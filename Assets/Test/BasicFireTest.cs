// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections;
using UnityEngine;
using Hourai.DanmakU;

public class BasicFireTest : MonoBehaviour {

    [SerializeField]
    private float delay;

	[SerializeField]
	private DanmakuPrefab prefab;

    [SerializeField]
    private Gradient grad;

    private Task test;
    private DanmakuGroup group;

    private void Start() {
        group = DanmakuGroup.List();
        FireData data = prefab;
        group.Bind(data);
        test = data.Infinite(Modifier.Rotate(2.5f))
                    .From(gameObject.Descendants())
                    .WithSpeed(2)
                    .Delay(delay)
                    .RadialBurst(100)
                    .Execute();
        Invoke("Test", 10);
    }

    void Test() {
        Debug.Log(group.Count);
        group.Color(grad);
        test.Stop();
    }

}