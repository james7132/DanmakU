using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;
using System.Runtime.CompilerServices;

public class DanmakuPool : IDisposable {

  public int ActiveCount;

  public NativeArray<Vector2> Positions;
  public NativeArray<float> Rotations;

  public NativeArray<float> Speeds;
  public NativeArray<float> AngularSpeeds;

  public NativeArray<Color> Colors;

  public NativeArray<Matrix4x4> Transforms;

  readonly Stack<int> Deactivated;

  public DanmakuPool(int poolSize) {
    ActiveCount = 0;
    Deactivated = new Stack<int>(poolSize);

    Positions = new NativeArray<Vector2>(poolSize, Allocator.Persistent);
    Rotations = new NativeArray<float>(poolSize, Allocator.Persistent);

    Speeds = new NativeArray<float>(poolSize, Allocator.Persistent);
    AngularSpeeds = new NativeArray<float>(poolSize, Allocator.Persistent);

    Transforms = new NativeArray<Matrix4x4>(poolSize, Allocator.Persistent);
  }

  public UpdateContext Update(JobHandle dependency = default(JobHandle)) {
    while (Deactivated.Count > 0) {
      DestroyInternal(Deactivated.Pop());
    }

    if (ActiveCount <= 0) {
      UpdateHandle = null;
      return;
    }

    return new DanmakuPoolUpdateContext(this, dependency);
  }

  void DestroyInternal(int index) {
    ActiveCount--;
    Swap(ref Positions, index, ActiveCount);
    Swap(ref Rotations, index, ActiveCount);
    Swap(ref Speeds, index, ActiveCount);
    Swap(ref AngularSpeeds, index, ActiveCount);
    Swap(ref Colors, index, ActiveCount);
  }

  public Danmaku Get() => new Danmaku(this, ActiveCount++);

  public void Get(Danmaku[] danmaku, int count) {
    for (var i = 0; i < count; i++) {
      danmaku[i] = new Danmaku(this, ActiveCount + i);
    }
    ActiveCount += count;
  }

  void Swap<T>(ref NativeArray<T> array, int a, int b) where T : struct {
    T temp = array[a];
    array[a] = array[b];
    array[b] = temp;
  }

  public void Dispose() {
    Positions.Dispose();
    Rotations.Dispose();

    Speeds.Dispose();
    AngularSpeeds.Dispose();

    Colors.Dispose();

    Transforms.Dispose();
  }

  internal void Destroy(Danmaku deactivate) => Deactivated.Push(deactivate.Index);

  internal struct UpdateContext : IDisposable {

    const int kBatchSize = 32;

    public readonly NativeArray<Vector2> OldPositions;
    public readonly NativeArray<float> OldRotations;
    public readonly JobHandle UpdateJobHandle;

    internal DanmakuPoolUpdateContext(DanmakuPool pool, JobHandle dependency) {
      OldPositions = new NativeArray<Vector2>(pool.Positions, Allocator.TempJob));
      OldRotations = new NativeArray<float>(pool.Rotations, Allocator.TempJob));

      UpdateJobHandle = new MoveDanmaku {
        CurrentPositions = OldPositions,
        CurrentRotations = OldRotations,

        NewPositions = pool.Positions,
        NewRotations = pool.Rotations,

        Speeds = pool.Speeds,
        AngularSpeeds = pool.AngularSpeeds,

        Transforms = pool.Transforms;
      }.Schedule(pool.ActiveCount, kBatchSize, dependency);
    }

    public void Dispose() {
      UpdateJobHandle.Complete();
      OldPositions.Dispose();
      OldRotations.Dispose();
    }

  }
}
