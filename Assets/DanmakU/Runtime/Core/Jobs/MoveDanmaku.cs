using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace DanmakU {

internal struct MoveDanmaku : IJobBatchedFor {

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

  public unsafe void Execute(int start, int end) {
    var positionPtr = (Vector2*)Positions.GetUnsafePtr() + start;
    var rotationPtr = (float*)Rotations.GetUnsafePtr() + start;
    var speedPtr = (float*)Speeds.GetUnsafeReadOnlyPtr() + start;
    var angularSpeedPtr = (float*)AngularSpeeds.GetUnsafeReadOnlyPtr() + start;
    var rotationEnd = rotationPtr + (end - start);
    while (rotationPtr < rotationEnd) {
      var speed = *speedPtr++;
      var rotation = *rotationPtr + *angularSpeedPtr++ * DeltaTime;
      *rotationPtr = rotation;
      positionPtr->x += speed * Mathf.Cos(rotation) * DeltaTime;
      positionPtr->y += speed * Mathf.Sin(rotation) * DeltaTime;
      rotationPtr++;
      positionPtr++;
    }
  }

}

}