using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

namespace DanmakU {

internal struct MoveDanmaku : IJobParallelFor {

  public Bounds Bounds;
  public float DeltaTime;

  public NativeArray<Vector2> Positions;
  public NativeArray<float> Rotations;
  public NativeArray<float> Times;
  [ReadOnly] public NativeArray<float> Speeds;
  [ReadOnly] public NativeArray<float> AngularSpeeds;
  [WriteOnly] public NativeArray<Matrix4x4> Transforms; 

  public MoveDanmaku(DanmakuPool pool) {
    Bounds = DanmakuManager.Instance.Bounds;
    DeltaTime = Time.deltaTime;
    Positions = pool.Positions;
    Rotations = pool.Rotations;
    Times = pool.Times;
    Speeds = pool.Speeds;
    AngularSpeeds = pool.AngularSpeeds;
    Transforms = pool.Transforms;
  }

  public void Execute(int index) {
    var rotation = Rotations[index] + AngularSpeeds[index] * DeltaTime;
    var direction = RotationUtiliity.ToUnitVector(rotation);
    var position = Positions[index] + Speeds[index] * direction * DeltaTime;

    var transform = new Matrix4x4();
    transform[0, 0] = direction.x;
    transform[0, 1] = -direction.y;
    transform[1, 0] = direction.y;
    transform[1, 1] = direction.x;
    transform[2, 2] = 1.0f;
    transform[3, 3] = 1;
    transform[0, 3] = position.x;
    transform[1, 3] = position.y;

    if (Bounds.Contains(position)) {
      Times[index] += DeltaTime;
    } else {
      Times[index] = float.MinValue;
    }
    Positions[index] = position;
    Rotations[index] = rotation;
    Transforms[index] = transform;
  }

}
}