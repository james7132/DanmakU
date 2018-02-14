using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;
using System.Runtime.CompilerServices;

public struct MoveDanmaku : IJobParallelFor {

  [ReadOnly] public NativeArray<Vector2> CurrentPositions;
  [ReadOnly] public NativeArray<float> CurrentRotations;

  [WriteOnly] public NativeArray<Vector2> NewPositions;
  [WriteOnly] public NativeArray<float> NewRotations;

  [ReadOnly] public NativeArray<float> Speeds;
  [ReadOnly] public NativeArray<float> AngularSpeeds;

  public void Execute(int index) {
    var rotation = (CurrentRotations[index] + AngularSpeeds[index]) % (Mathf.PI * 2);

    NewPositions[index] = CurrentPositions[index] + (Speeds[index] * RotationUtil.ToUnitVector(rotation));
    NewRotations[index] = rotation;
  }

}

public static class RotationUtil {

  internal const float kRotationAccuracy = Mathf.PI / 10000;
  internal const int kRotationCacheSize = (int)(Mathf.PI * 2 / kRotationAccuracy);
  internal static Vector2[] RotationCache;

  static RotationUtil() {
    RotationCache = new Vector2[kRotationCacheSize];
    for (var i = 0; i < RotationCache.Length; i++) {
      var angle = kRotationAccuracy * i;
      RotationCache[i] = new Vector2 {
        x = Mathf.Cos(angle),
        y = Mathf.Sin(angle)
      };
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector2 ToUnitVector(float rotation) {
    int index = (int)(rotation / kRotationAccuracy);
    index = (Mathf.Abs(index * kRotationCacheSize) + index) % kRotationCacheSize;
    return RotationCache[index];
  }

}

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

public struct Danmaku {

  internal readonly int Index;
  public readonly DanmakuPool Pool;

  internal Danmaku(DanmakuPool pool, int index) {
    Pool = pool;
    Index = index;
  }

  public Vector2 Position {
    get { return Pool.Positions[Index]; }
    set { Pool.Positions[Index] = value; }
  }

  public float Rotation {
    get { return Pool.Rotations[Index]; }
    set { Pool.Rotations[Index] = value; }
  }

  public float Speed {
    get { return Pool.Speeds[Index]; }
    set { Pool.Speeds[Index] = value; }
  }

  public float AngularSpeed {
    get { return Pool.AngularSpeeds[Index]; }
    set { Pool.AngularSpeeds[Index] = value; }
  }

  /// <summary>
  /// Destroys the danmaku object.
  /// </summary>
  /// <remarks>
  /// Calling this funciton simply queues the Danmaku for destruction. It will be
  /// recycled back into it's pool on the pool's next update cycle.
  /// 
  /// Destroying the same danmaku more than once or attempting to edit an already
  /// destroyed Danmaku will likely result in undefined behavior.
  /// </remarks>
  public void Destroy() => Pool.Destroy(this);

}
