using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;
using System.Runtime.CompilerServices;

public class DanmakuPool : IDisposable {

  const int kBatchSize = 32;

  public int ActiveCount;

  public NativeArray<Vector2> Positions;
  public NativeArray<float> Rotations;

  public NativeArray<float> Speeds;
  public NativeArray<float> AngularSpeeds;

  readonly Stack<int> Deactivated;
  JobHandle? UpdateHandle;

  public DanmakuPool(int poolSize) {
    ActiveCount = 0;
    Deactivated = new Stack<int>(poolSize);

    Positions = new NativeArray<Vector2>(poolSize, Allocator.Persistent);
    Rotations = new NativeArray<float>(poolSize, Allocator.Persistent);

    Speeds = new NativeArray<float>(poolSize, Allocator.Persistent);
    AngularSpeeds = new NativeArray<float>(poolSize, Allocator.Persistent);
  }

  public void StartUpdate() {
    while (Deactivated.Count > 0) {
      DestroyInternal(Deactivated.Pop());
    }

    if (ActiveCount <= 0) {
      UpdateHandle = null;
      return;
    }

    using (var positions = new NativeArray<Vector2>(Positions, Allocator.TempJob))
    using (var rotations = new NativeArray<float>(Rotations, Allocator.TempJob)) {
      new MoveDanmaku {
        CurrentPositions = positions,
        CurrentRotations = rotations,

        NewPositions = Positions,
        NewRotations = Rotations,

        Speeds = Speeds,
        AngularSpeeds = AngularSpeeds,
      }.Schedule(ActiveCount, kBatchSize).Complete();
    }
  }

  void DestroyInternal(int index) {
    ActiveCount--;
    Swap(ref Positions, index, ActiveCount);
    Swap(ref Rotations, index, ActiveCount);
    Swap(ref Speeds, index, ActiveCount);
    Swap(ref AngularSpeeds, index, ActiveCount);
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
  }

  internal void Destroy(Danmaku deactivate) => Deactivated.Push(deactivate.Index);

}
