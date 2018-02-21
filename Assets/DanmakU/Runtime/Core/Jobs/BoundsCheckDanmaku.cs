using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

namespace DanmakU {

internal struct BoundsCheckDanmaku : IJobParallelFor {

  public float DeltaTime;
  public Bounds2D Bounds;
  [ReadOnly] public NativeArray<Vector2> Positions;
  public NativeArray<float> Times;

  public BoundsCheckDanmaku(DanmakuPool pool) {
    DeltaTime = Time.deltaTime;
    Bounds = DanmakuManager.Instance.Bounds;
    Times = pool.Times;
    Positions = pool.Positions;
  }

  public void Execute(int index) {
    if (Bounds.Contains(Positions[index])) {
      Times[index] += DeltaTime;
    } else {
      Times[index] = float.MinValue;
    }
  }

}

}