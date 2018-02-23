using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

namespace DanmakU {

internal struct BoundsCheckDanmaku : IJobParallelFor {

  public float DeltaTime;
  [ReadOnly] public NativeArray<Vector2> Positions;
  public NativeArray<float> Times;

  Vector2 min;
  Vector2 max;

  public BoundsCheckDanmaku(DanmakuPool pool) {
    DeltaTime = Time.deltaTime;
    Times = pool.Times;
    Positions = pool.Positions;

    var bounds = DanmakuManager.Instance.Bounds;
    min = bounds.Min;
    max = bounds.Max;
  }

  public void Execute(int index) {
    var position = Positions[index];
    if (position.x >= min.x && position.x <= max.x &&
        position.y >= min.y && position.y <= max.y) {
      Times[index] += DeltaTime;
    } else {
      Times[index] = float.MinValue;
    }
  }

}

}