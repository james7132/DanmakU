using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

namespace DanmakU {

internal struct MoveDanmaku : IJobParallelFor {

  public float DeltaTime;

  public NativeArray<Vector2> Positions;
  [ReadOnly] public NativeArray<Vector2> Directions;
  [ReadOnly] public NativeArray<float> Speeds;

  public MoveDanmaku(DanmakuPool pool) {
    DeltaTime = Time.deltaTime;
    Positions = pool.Positions;
    Directions = pool.Directions;
    Speeds = pool.Speeds;
  }

  public void Execute(int index) {
    Positions[index] += Speeds[index] * Directions[index] * DeltaTime;
  }

}

}