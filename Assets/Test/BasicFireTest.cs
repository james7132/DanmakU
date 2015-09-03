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

    [SerializeField]
    private Gradient grad;

    private Task test;
    private DanmakuGroup group;

    private void Start() {
        group = DanmakuGroup.List();
        FireData data = prefab;
        group.Bind(data);
        test = data.Infinite()
                    //.From(gameObject.Descendants())
                    //.WithSpeed(2)
                    //.Towards(Vector2.zero)
                    .Delay(delay)
                    .Circle(5,1)
                    .Execute();
        Invoke("Test", 10);
    }

    void Test() {
        Debug.Log(group.Count);
        group.Color(grad);
        test.Stop();
    }

}