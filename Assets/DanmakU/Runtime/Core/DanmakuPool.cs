using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace DanmakU {

public class DanmakuPool : IEnumerable<Danmaku>, IDisposable {

  public const int kBatchSize = 32;
  const int kGrowthFactor = 2;

  int activeCount;
  public int ActiveCount => activeCount;
  public int Capacity { get; private set; }

  public float ColliderRadius;

  internal NativeArray<float> Times;

  public NativeArray<DanmakuState> InitialStates;

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
    activeCount = 0;
    Capacity = poolSize;
    Deactivated = new Stack<int>(poolSize);

    InitialStates = new NativeArray<DanmakuState>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
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

  internal void FlushDestroyed() {
    while (Deactivated.Count > 0) {
      DestroyInternal(Deactivated.Pop());
    }
  }

  internal JobHandle Update(JobHandle dependency = default(JobHandle)) {
    if (ActiveCount <= 0) return dependency;
    new NativeSlice<Vector2>(OldPositions, 0, activeCount).CopyFrom(
      new NativeSlice<Vector2>(Positions, 0, activeCount));
    float dt = Time.deltaTime;
    dependency = new MoveDanmaku {
      DeltaTime = Time.deltaTime,
      Positions = Positions,
      Rotations = Rotations,
      Times = Times,
      Speeds = Speeds,
      AngularSpeeds = AngularSpeeds,
      Transforms = Transforms
    }.Schedule(ActiveCount, kBatchSize, dependency);
    if (DanmakuCollider.ColliderCount > 0) {
      dependency = new CollideDanamku {
        Radius = ColliderRadius,
        Positions = Positions,
        Collisions = CollisionMasks
      }.Schedule(ActiveCount, kBatchSize, dependency);
    }
    return dependency;
  }

  /// <summary>
  /// Creates a new Danmaku from the pool.
  /// </summary>
  public Danmaku Get(DanamkuConfig config) {
    CheckCapacity(1);
    var state = config.CreateState();
    InitialStates[activeCount] = state;
    Times[activeCount] = 0f;
    var danmaku = new Danmaku(this, activeCount++);
    danmaku.ApplyState(state);
    return danmaku;
  } 

  /// <summary>
  /// Retrieves a batch of new Danmaku from the pool.
  /// </summary>
  /// <param name="danmaku">an array of danmaku to write the values to.</param>
  /// <param name="count">the number of danmaku to create. Must be less than or equal to the length of of danmaku.</param>
  public void Get(Danmaku[] danmaku, int count) {
    CheckCapacity(count);
    for (var i = 0; i < count; i++) {
      Times[activeCount + i] = 0f;
      danmaku[i] = new Danmaku(this, activeCount + i);
    }
    activeCount += count;
  }

  /// <summary>
  /// Destroys all danmaku in the pool.
  /// </summary>
  public void Clear() => activeCount = 0;

  void CheckCapacity(int count) {
    if (activeCount + count > Capacity) {
      Resize();
    }
  }

  void Resize() {
    Debug.LogWarning("A DanmakuPool is being resized due to inadequate capacity. This is expensive. Consider a higher starting pool size.");
    Capacity *= kGrowthFactor;
    Resize(ref InitialStates);
    Resize(ref Times);
    Resize(ref Positions);
    Resize(ref Rotations);
    Resize(ref Speeds);
    Resize(ref AngularSpeeds);
    Resize(ref Colors);
    Resize(ref Transforms);
    Resize(ref OldPositions);
    Resize(ref CollisionMasks);
  }

  static void Resize<T>(ref NativeArray<T> array) where T : struct {
    var newArray = new NativeArray<T>(array.Length * kGrowthFactor, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
    var oldArray = array;
    new NativeSlice<T>(newArray, 0, array.Length).CopyFrom(array);
    array = newArray;
    oldArray.Dispose();
  }

  public void Dispose() {
    InitialStates.Dispose();
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

  public DanmakuEnumerator GetEnumerator() => new DanmakuEnumerator(this, 0, ActiveCount);
  IEnumerator<Danmaku> IEnumerable<Danmaku>.GetEnumerator() => GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

  internal void Destroy(Danmaku deactivate) => Deactivated.Push(deactivate.Id);

  void DestroyInternal(int index) {
    activeCount--;
    InitialStates[index] = InitialStates[activeCount];
    Times[index] = Times[activeCount];
    Positions[index] = Positions[activeCount];
    Rotations[index] = Rotations[activeCount];
    Speeds[index] = Speeds[activeCount];
    AngularSpeeds[index] = AngularSpeeds[activeCount];
    Colors[index] = Colors[activeCount];
  }
  
}

public struct DanmakuEnumerator : IEnumerator<Danmaku> {

  readonly DanmakuPool pool;
  readonly int start;
  readonly int end;
  int index;

  public Danmaku Current => new Danmaku(pool, index);
  object IEnumerator.Current => Current;

  internal DanmakuEnumerator(DanmakuPool pool, int start, int count) {
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