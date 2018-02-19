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

  AtomicCounter activeCount;
  public int ActiveCount => activeCount.Value;
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
  internal NativeStack<int> Deactivated;

  public DanmakuPool(int poolSize) {
    activeCount = new AtomicCounter(0, Allocator.Persistent);
    Capacity = poolSize;
    Deactivated = new NativeStack<int>(poolSize, Allocator.Persistent);

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

  // TODO(james7132): Cleanup this mess
  internal JobHandle FlushDestroyed(JobHandle dependency = default(JobHandle)) {
    return new DestroyDanmaku {
      ActiveCount = activeCount,
      Destroyed = Deactivated,
      Times = Times, 
      InitialStates = InitialStates, 
      Positions = Positions,
      Rotations = Rotations,
      Speeds = Speeds,
      AngularSpeeds = AngularSpeeds,
      Colors = Colors
    }.Schedule(dependency);
  }

  internal JobHandle Update(JobHandle dependency = default(JobHandle)) {
    if (ActiveCount <= 0) return dependency;
    new NativeSlice<Vector2>(OldPositions, 0, ActiveCount).CopyFrom(
      new NativeSlice<Vector2>(Positions, 0, ActiveCount));
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
    var index = activeCount.Value;
    InitialStates[index] = state;
    Times[index] = 0f;
    var danmaku = new Danmaku(this, index);
    activeCount.Increment();
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
    var index = activeCount.Value;
    for (var i = 0; i < count; i++) {
      Times[index + i] = 0f;
      danmaku[i] = new Danmaku(this, index + i);
    }
    activeCount.Add(count);
  }

  /// <summary>
  /// Destroys all danmaku in the pool.
  /// </summary>
  public void Clear() => activeCount.Set(0);

  void CheckCapacity(int count) {
    if (activeCount.Value + count > Capacity) {
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
    Deactivated.Dispose();
    Deactivated = new NativeStack<int>(Capacity, Allocator.Persistent);
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
    Deactivated.Dispose();
  }

  public DanmakuEnumerator GetEnumerator() => new DanmakuEnumerator(this, 0, ActiveCount);
  IEnumerator<Danmaku> IEnumerable<Danmaku>.GetEnumerator() => GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

  internal void Destroy(Danmaku deactivate) => Deactivated.Push(deactivate.Id);

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