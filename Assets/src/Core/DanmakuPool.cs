using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace DanmakU {

public class DanmakuPool : IEnumerable<Danmaku>, IDisposable {

  const int kBatchSize = 32;

  public int ActiveCount { get; private set; }

  public float ColliderRadius;

  internal NativeArray<float> Times;

  public NativeArray<Vector2> Positions;
  public NativeArray<float> Rotations;

  public NativeArray<float> Speeds;
  public NativeArray<float> AngularSpeeds;

  public NativeArray<Vector4> Colors;

  internal NativeArray<Matrix4x4> Transforms;
  internal NativeArray<Vector2> OldPositions;
  internal NativeArray<int> CollisionMasks;

  readonly Stack<int> Deactivated;

  public DanmakuPool(int poolSize) {
    ActiveCount = 0;
    Deactivated = new Stack<int>(poolSize);

    Times = new NativeArray<float>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
    
    Positions = new NativeArray<Vector2>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
    Rotations = new NativeArray<float>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

    Speeds = new NativeArray<float>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
    AngularSpeeds = new NativeArray<float>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

    Colors = new NativeArray<Vector4>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

    Transforms = new NativeArray<Matrix4x4>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

    OldPositions = new NativeArray<Vector2>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
    CollisionMasks = new NativeArray<int>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
  }

  public JobHandle Update(JobHandle dependency = default(JobHandle)) {
    var bounds = DanmakuManager.Instance.Bounds;
    for (var i = 0; i < ActiveCount; i++) {
      if (bounds.Contains(Positions[i])) continue;
      Deactivated.Push(i);
    }

    while (Deactivated.Count > 0) {
      DestroyInternal(Deactivated.Pop());
    }

    if (ActiveCount <= 0) return dependency;

    OldPositions.CopyFrom(Positions);
    var updateHandle = new MoveDanmaku {
      DeltaTime = Time.deltaTime,

      Times = Times,

      Positions = Positions,
      Rotations = Rotations,

      Speeds = Speeds,
      AngularSpeeds = AngularSpeeds,

      Transforms = Transforms
    }.Schedule(ActiveCount, kBatchSize, dependency);
    if (DanmakuCollider.ColliderCount > 0) {
      updateHandle = new CollideDanamku {
        Radius = ColliderRadius,
        Positions = Positions,
        Collisions = CollisionMasks
      }.Schedule(ActiveCount, kBatchSize, updateHandle);
    }
    return updateHandle;
  }

  /// <summary>
  /// Retrieves a new Danmaku from the pool.
  /// </summary>
  /// <remarks>
  /// The Danmaku's data is not cleared upon retrieval. There may be
  /// invalid or imporperly initialized values.
  /// </remarks>
  /// <returns></returns>
  public Danmaku Get() {
    Times[ActiveCount] = 0f;
    return new Danmaku(this, ActiveCount++);
  } 

  /// <summary>
  /// Retrieves a batch of new Danmaku from the pool.
  /// </summary>
  /// <param name="danmaku">an array of danmaku to write the values to.</param>
  /// <param name="count">the number of danmaku to create. Must be less than or equal to the length of of danmaku.</param>
  public void Get(Danmaku[] danmaku, int count) {
    for (var i = 0; i < count; i++) {
      Times[ActiveCount + i] = 0f;
      danmaku[i] = new Danmaku(this, ActiveCount + i);
    }
    ActiveCount += count;
  }

  /// <summary>
  /// Destroys all danmaku in the pool.
  /// </summary>
  public void Clear() => ActiveCount = 0;

  public void Dispose() {
    Times.Dispose();

    Positions.Dispose();
    Rotations.Dispose();

    Speeds.Dispose();
    AngularSpeeds.Dispose();

    Colors.Dispose();

    Transforms.Dispose();

    OldPositions.Dispose();
    CollisionMasks.Dispose();
  }

  public Enumerator GetEnumerator() => new Enumerator(this, 0, ActiveCount);
  IEnumerator<Danmaku> IEnumerable<Danmaku>.GetEnumerator() => GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

  internal void Destroy(Danmaku deactivate) => Deactivated.Push(deactivate.Id);

  void Swap<T>(ref NativeArray<T> array, int a, int b) where T : struct {
    T temp = array[a];
    array[a] = array[b];
    array[b] = temp;
  }

  void DestroyInternal(int index) {
    ActiveCount--;
    Times[index] = Times[ActiveCount];
    Swap(ref Positions, index, ActiveCount);
    Swap(ref Rotations, index, ActiveCount);
    Swap(ref Speeds, index, ActiveCount);
    Swap(ref AngularSpeeds, index, ActiveCount);
    Swap(ref Colors, index, ActiveCount);
  }

  public struct Enumerator : IEnumerator<Danmaku> {

    readonly DanmakuPool pool;
    readonly int start;
    readonly int end;
    int index;

    public Danmaku Current => new Danmaku(pool, index);
    object IEnumerator.Current => Current;

    internal Enumerator(DanmakuPool pool, int start, int count) {
      this.pool =  pool;
      this.start = start;
      this.end = start + count;
      index = -1;
    }

    public bool MoveNext() {
      if (index < 0) {
        index = start;
      } else {
        index++;
      }
      return index < end;
    }

    public void Reset() => index = -1;
    public void Dispose() {}

  }
  
}

}