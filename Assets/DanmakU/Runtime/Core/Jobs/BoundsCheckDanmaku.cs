using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace DanmakU {

public struct BoundsCheckDanmaku : IJob {

  public Bounds Bounds;
  [ReadOnly] public NativeArray<Vector2> Position;
  [WriteOnly] public NativeArray<int> Indexes;

  public unsafe void Execute() {
    var length = Position.Length;
    var positions = (Vector2*)Position.GetUnsafeReadOnlyPtr();
    var indexes = (int*)Indexes.GetUnsafePtr();
    int count = 0;
    for (var i = 0; i < length; i++) {
      if (Bounds.Contains(positions[i])) continue;
      indexes[count++] = i;
      indexes[0] = count;
    }
  }

}

}