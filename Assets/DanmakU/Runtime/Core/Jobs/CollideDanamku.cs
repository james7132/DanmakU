using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

namespace DanmakU {

internal struct CollideDanamku : IJobParallelFor {

  Bounds2D Bounds;
  [ReadOnly] NativeArray<Vector2> Positions;
  [WriteOnly] NativeArray<int> Collisions;

  public CollideDanamku(DanmakuPool pool) {
    var radius = pool.ColliderRadius;
    Bounds = new Bounds2D(Vector2.zero, new Vector2(radius, radius));
    Positions = pool.Positions;
    Collisions = pool.CollisionMasks;
  }

  public void Execute(int index) {
    Bounds.Center = Positions[index];
    Collisions[index] = DanmakuCollider.TestCollisions(Bounds);
  }

}

}