using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace DanmakU {

internal struct MoveDanmaku : IJobParallelFor {

  public float DeltaTime;

  public NativeArray<Vector2> Positions;
  public NativeArray<float> Rotations;
  [ReadOnly] public NativeArray<float> Speeds;
  [ReadOnly] public NativeArray<float> AngularSpeeds;

  public MoveDanmaku(DanmakuPool pool) {
    DeltaTime = Time.deltaTime;
    Positions = pool.Positions;
    Rotations = pool.Rotations;
    Speeds = pool.Speeds;
    AngularSpeeds = pool.AngularSpeeds;
  }

  public unsafe void Execute(int index) {
    var positionPtr = (float*)Positions.GetUnsafePtr() + (index * 2);
    var rotationPtr = (float*)Rotations.GetUnsafePtr() + index;
    var speed = Speeds[index];
    var rotation = *rotationPtr + AngularSpeeds[index] * DeltaTime;
    *rotationPtr = rotation;
    *positionPtr++ += speed * Mathf.Cos(rotation)* DeltaTime;
    *positionPtr += speed * Mathf.Sin(rotation) * DeltaTime;
  }

}

}