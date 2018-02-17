using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

public struct MoveDanmaku : IJobParallelFor {

  public float DeltaTime;

  public NativeArray<float> Times;

  public NativeArray<Vector2> Positions;
  public NativeArray<float> Rotations;

  [ReadOnly] public NativeArray<float> Speeds;
  [ReadOnly] public NativeArray<float> AngularSpeeds;

  [WriteOnly] public NativeArray<Matrix4x4> Transforms;

  public void Execute(int index) {
    var rotation = (Rotations[index] + AngularSpeeds[index] * DeltaTime) % (Mathf.PI * 2);
    var position = Positions[index] + (Speeds[index] * RotationUtil.ToUnitVector(rotation) * DeltaTime);

    var rotationQuat = Quaternion.Euler(0, 0, rotation * Mathf.Rad2Deg);

    Times[index] += DeltaTime;
    Transforms[index] = Matrix4x4.TRS(position, rotationQuat, Vector3.one);
    Positions[index] = position;
    Rotations[index] = rotation;
  }

}
