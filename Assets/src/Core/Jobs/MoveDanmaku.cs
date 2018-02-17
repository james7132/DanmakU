using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

public struct MoveDanmaku : IJobParallelFor {

  public float DeltaTime;

  public NativeArray<float> Times;

  [ReadOnly] public NativeArray<Vector2> CurrentPositions;
  [ReadOnly] public NativeArray<float> CurrentRotations;

  [WriteOnly] public NativeArray<Vector2> NewPositions;
  [WriteOnly] public NativeArray<float> NewRotations;

  [ReadOnly] public NativeArray<float> Speeds;
  [ReadOnly] public NativeArray<float> AngularSpeeds;

  [WriteOnly] public NativeArray<Matrix4x4> Transforms;

  public void Execute(int index) {
    var rotation = (CurrentRotations[index] + AngularSpeeds[index] * DeltaTime) % (Mathf.PI * 2);
    var position = CurrentPositions[index] + (Speeds[index] * RotationUtil.ToUnitVector(rotation) * DeltaTime);

    var rotationQuat = Quaternion.Euler(0, 0, rotation * Mathf.Rad2Deg);

    Times[index] += DeltaTime;
    Transforms[index] = Matrix4x4.TRS(position, rotationQuat, Vector3.one);
    NewPositions[index] = position;
    NewRotations[index] = rotation;
  }

}
