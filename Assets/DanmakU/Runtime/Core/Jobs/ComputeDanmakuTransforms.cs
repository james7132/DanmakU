using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

namespace DanmakU {

internal struct ComputeDanmakuTranforms : IJobParallelFor {

  [ReadOnly] public NativeArray<Vector2> Positions;
  [ReadOnly] public NativeArray<Vector2> Directions;
  [WriteOnly] public NativeArray<Matrix4x4> Transforms; 

  public ComputeDanmakuTranforms(DanmakuPool pool) {
    Positions = pool.Positions;
    Directions = pool.Directions;
    Transforms = pool.Transforms;
  }

  public void Execute(int index) {
    var position = Positions[index];
    var direction = Directions[index];

    var transform = new Matrix4x4();
    transform[0, 0] = direction.x;
    transform[0, 1] = -direction.y;
    transform[1, 0] = direction.y;
    transform[1, 1] = direction.x;
    transform[2, 2] = 1.0f;
    transform[3, 3] = 1;
    transform[0, 3] = position.x;
    transform[1, 3] = position.y;

    Transforms[index] = transform;
  }

}

}