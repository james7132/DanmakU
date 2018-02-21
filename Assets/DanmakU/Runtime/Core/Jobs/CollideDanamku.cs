using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

namespace DanmakU {

internal struct CollideDanamku : IJobParallelFor {

  public float Radius;

  [ReadOnly] public NativeArray<Vector2> Positions;
  [WriteOnly] public NativeArray<int> Collisions;

  public void Execute(int index) {
    var danmakuBounds = new Bounds2D(Positions[index], new Vector2(Radius, Radius));
    Collisions[index] = DanmakuCollider.TestCollisions(danmakuBounds);
  }

}

}