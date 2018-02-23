using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace DanmakU {

internal struct MoveDanmaku : IJobParallelFor {

  public float DeltaTime;

  public NativeArray<Vector2> Positions;
  public NativeArray<float> Rotations;
  [WriteOnly] public NativeArray<Vector2> Directions;
  [ReadOnly] public NativeArray<float> Speeds;
  [ReadOnly] public NativeArray<float> AngularSpeeds;

  public MoveDanmaku(DanmakuPool pool) {
    DeltaTime = Time.deltaTime;
    Positions = pool.Positions;
    Rotations = pool.Rotations;
    Directions = pool.Directions;
    Speeds = pool.Speeds;
    AngularSpeeds = pool.AngularSpeeds;
  }

  public unsafe void Execute(int index) {
    var positionPtr = (float*)Positions.GetUnsafePtr() + (index * 2);
    var directionPtr = (float*)Directions.GetUnsafePtr() + (index * 2);
    var rotationPtr = (float*)Rotations.GetUnsafePtr() + index;
    var speed = Speeds[index];
    var rotation = *rotationPtr + AngularSpeeds[index] * DeltaTime;
    var dirX = Mathf.Cos(rotation);
    var dirY = Mathf.Sin(rotation);
    *rotationPtr = rotation;
    *positionPtr++ += speed * dirX * DeltaTime;
    *positionPtr += speed * dirY * DeltaTime;
    *directionPtr++ = dirX;
    *directionPtr = dirY;
  }

}

}