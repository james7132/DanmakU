// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections;
using UnityEngine;
using Hourai;
using Hourai.DanmakU;

public class BasicFireTest : MonoBehaviour {

    [SerializeField]
    private float delay;

	[SerializeField]
	private DanmakuPrefab prefab;

    [SerializeField]
    private Gradient grad;

    [SerializeField]
    private float speed;

    [SerializeField]
    private int burstCount;

    [SerializeField]
    private float rotation;

    private Task test;
    private DanmakuGroup group;
    private DanmakuField field;

    private void Start() {
        TimeUtil.FrameRateIndependent = false;
        field = DanmakuField.FindClosest(this);
        group = DanmakuGroup.List();
        FireData data = prefab;
        group.Bind(data);
        if (field)
            field.Bind(data);
        
        Spiral(data.Infinite(Modifier.Rotate(rotation))).Execute();
        //Spiral(data.Infinite(Modifier.Rotate(-3f)).WithColor(Color.blue)).Execute();
        //InvokeRepeating("DAALL", 10, 10);
    }

    void DAALL()
    {
        Danmaku.DestroyAll();
    }

    IEnumerable Spiral(IEnumerable pos)
    {
        return pos  .WithSpeed(speed)
                    .Delay(delay)
                    .RadialBurst(burstCount);
    }

}