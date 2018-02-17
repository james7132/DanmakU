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
    var direction = RotationUtil.ToUnitVector(rotation);
    var position = Positions[index] + (Speeds[index] * direction * DeltaTime);

    var transform = new Matrix4x4();
    transform[0, 0] = direction.x;
    transform[0, 1] = -direction.y;
    transform[1, 0] = direction.y;
    transform[1, 1] = direction.x;
    transform[2, 2] = 1.0f;
    transform[3, 3] = 1;
    transform[0, 3] = position.x;
    transform[1, 3] = position.y;

    Times[index] += DeltaTime;
    Transforms[index] = transform;
    Positions[index] = position;
    Rotations[index] = rotation;
  }

}
