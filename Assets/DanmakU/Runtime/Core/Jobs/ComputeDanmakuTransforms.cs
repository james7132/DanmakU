using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

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

  public unsafe void Execute(int index) {
    var position = Positions[index];
    var direction = Directions[index];

    var floatPtr = (float*)((Matrix4x4*)Transforms.GetUnsafePtr() + index);

    *floatPtr++ = direction.x;    // 0, 0
    *floatPtr++ = direction.y;    // 0, 1
    *floatPtr++ = 0f;             // 0, 2
    *floatPtr++ = 0f;             // 0, 3
    *floatPtr++ = -direction.y;   // 1, 0
    *floatPtr++ = direction.x;    // 1, 1
    *floatPtr++ = 0f;             // 1, 2
    *floatPtr++ = 0f;             // 1, 3
    *floatPtr++ = 0f;             // 2, 0
    *floatPtr++ = 0f;             // 2, 1
    *floatPtr++ = 1f;             // 2, 2
    *floatPtr++ = 0f;             // 2, 3
    *floatPtr++ = position.x;     // 3, 0
    *floatPtr++ = position.y;     // 3, 1
    *floatPtr++ = 0f;             // 3, 2
    *floatPtr = 1f;               // 3, 3
  }

}

}