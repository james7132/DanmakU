using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

namespace DanmakU {

internal struct RotateDanmaku : IJobParallelFor {

  public float DeltaTime;

  public NativeArray<float> Rotations;
  [ReadOnly] public NativeArray<float> AngularSpeeds;
  [WriteOnly] public NativeArray<Vector2> Directions;

  public RotateDanmaku(DanmakuPool pool) {
    DeltaTime = Time.deltaTime;
    Rotations = pool.Rotations;
    AngularSpeeds = pool.AngularSpeeds;
    Directions = pool.Directions;
  }

  public void Execute(int index) {
    var rotation = Rotations[index] + AngularSpeeds[index] * DeltaTime;
    Directions[index] = RotationUtiliity.ToUnitVector(rotation);
    Rotations[index] = rotation;
  }

}

}