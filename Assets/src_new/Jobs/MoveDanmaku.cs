using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

public struct MoveDanmaku : IJobParallelFor {

  [ReadOnly] public NativeArray<Vector2> CurrentPositions;
  [ReadOnly] public NativeArray<float> CurrentRotations;

  [WriteOnly] public NativeArray<Vector2> NewPositions;
  [WriteOnly] public NativeArray<float> NewRotations;

  [ReadOnly] public NativeArray<float> Speeds;
  [ReadOnly] public NativeArray<float> AngularSpeeds;

  public void Execute(int index) {
    var rotation = (CurrentRotations[index] + AngularSpeeds[index]) % (Mathf.PI * 2);

    NewPositions[index] = CurrentPositions[index] + (Speeds[index] * RotationUtil.ToUnitVector(rotation));
    NewRotations[index] = rotation;
  }

}
