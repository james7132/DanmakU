using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace DanmakU {

/// <summary>
/// A pool of <see cref="DanmakU.Danmaku"/>.
/// </summary>
/// <remarks>
/// <para>
/// DanmakuPools manage the lifetimes and data of all of the the bullets they own. Creation of a bullet
/// requires calling <see cref="Get"/> and calling <see cref="DanmakU.Danmaku.Destroy"/> returns its data
/// to the pool.
/// </para>
/// <para>
/// As more Danmaku are created from the pool, the amount of unused capacity will shrink. 
/// If more bullets are requested than there is <see cref="Capacity"/>, the pool will be resized. 
/// This can be an expensive operation, especially on already large pools. Ensuring bullets are 
/// timely destroyed and returned to the pool can remedy this.
/// </para>
/// </remarks>
/// <example>
/// Implemented as a Structure of Arrays, DanmakuPools see two main modes of use: as a backing store of 
/// NativeArrays for Unity Jobs, and as an enumerable container for the managed bullets. The internal backing
/// arrays are exposed as fields to use in Unity Jobs:
/// <code>
/// using Unity.Jobs;
/// 
/// public struct CustomMoveDanmaku : IJobParallelFor {
///   public Vector2 Movement;
///   public NativeArray&lt;Vector2&gt; Positions;
/// 
///   public void Execute(int index) {
///     Positions[index] += Movement;
///   }
/// }  
/// 
/// void ProcessDanmakU(DanmakuPool pool) {
///   new CustomMoveDanmaku {
///     Movement = Vector2.up,
///     Positions = pool.Positions
///   }.Schedule();
/// }
/// </code>
/// </example>
/// <example>
/// The pool implements `IEnumerable&lt;Danmaku&gt; and can be used in `foreach` loops to iterate over 
/// all of the bullets in the pool. This does not generate any garbage when used directly with the pool as 
/// the enumerator returned is a mutable struct enumerator.
/// <code>
/// void ProcessPool(DanmakuPool pool) {
///   foreach (Danmaku danmaku in pool) {
///     // Move every bullet to the right.
///     danmaku.Position += Vector2.right;
///   }
/// }
/// </code>
/// </example>
/// <example>
/// This also makes it compatible with LINQ queries. However this is discouraged as the need to box
/// the enumerator as an enumerable generates garbage.
/// <code>
/// using System.Linq;
/// 
/// void ProcessPool(DanmakuPool pool) {
///   // Get all of the bullets older than 5 seconds
///   var oldDanmaku = pool.Where(danmaku => danmaku.Time > 5f);
///   // Destroy them
///   foreach (var bullet in oldDanmaku) {
///     bullet.Destroy(); 
///   }
/// }
/// </code>
/// </example>
public class DanmakuPool : IEnumerable<Danmaku>, IDisposable {

  /// <summary>
  /// The recommended batch size for processing Danmaku in parallelizable jobs.
  /// </summary>
  /// <seealso cref="Unity.Jobs.IJobParallelFor"/>
  public const int kBatchSize = 1024;
  const int kGrowthFactor = 2;

  /// <summary>
  /// Gets the total count of active bullets currently managed by the pool.
  /// </summary>
  public int ActiveCount => activeCountArray[0];

  /// <summary>
  /// Gets the total capacity of the pool. Strictly greater than or equal to <see cref="ActiveCount"/>.
  /// </summary>
  public int Capacity { get; private set; }

  /// <summary>
  /// The radius of the collider used to calculate collisions with the bullets in the pool.
  /// </summary>
  public float ColliderRadius;

  internal NativeArray<float> Times;
  internal NativeArray<DanmakuState> InitialStates;

  /// <summary>
  /// The array of all world positions of <see cref="DanmakU.Danmaku"/> in the pool.
  /// </summary>
  /// <seealso cref="DanmakU.Danmaku.Position"/>
  public NativeArray<Vector2> Positions;

  /// <summary>
  /// The array of all rotations of <see cref="DanmakU.Danmaku"/> in the pool.
  /// </summary>
  /// <seealso cref="DanmakU.Danmaku.Rotation"/>
  public NativeArray<float> Rotations;

  /// <summary>
  /// The array of all speeds of <see cref="DanmakU.Danmaku"/> in the pool.
  /// </summary>
  /// <seealso cref="DanmakU.Danmaku.Speed"/>
  public NativeArray<float> Speeds;

  /// <summary>
  /// The array of all angular speeds of <see cref="DanmakU.Danmaku"/> in the pool.
  /// </summary>
  /// <seealso cref="DanmakU.Danmaku.AngularSpeed"/>
  public NativeArray<float> AngularSpeeds;

  /// <summary>
  /// The array of all angular speeds of <see cref="DanmakU.Danmaku"/> in the pool.
  /// </summary>
  /// <remarks>
  /// For performance reasons, the RGBA colors are stored as Vector4s.
  /// </remarks>
  /// <seealso cref="DanmakU.Danmaku.AngularSpeed"/>
  public NativeArray<Vector4> Colors;

  internal NativeArray<int> activeCountArray;
  internal NativeArray<Vector2> OldPositions;
  internal NativeArray<int> CollisionMasks;

  internal DanmakuPool(int poolSize) {
    activeCountArray = new NativeArray<int>(1, Allocator.Persistent);
    Capacity = poolSize;
    InitialStates = new NativeArray<DanmakuState>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
    Times = new NativeArray<float>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
    Positions = new NativeArray<Vector2>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
    Rotations = new NativeArray<float>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
    Speeds = new NativeArray<float>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
    AngularSpeeds = new NativeArray<float>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
    Colors = new NativeArray<Vector4>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
    OldPositions = new NativeArray<Vector2>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
    CollisionMasks = new NativeArray<int>(poolSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
  }

  internal JobHandle Update(JobHandle dependency = default(JobHandle)) {
    var count = ActiveCount;
    if (count <= 0) return dependency;
    new NativeSlice<Vector2>(OldPositions, 0, count).CopyFrom(new NativeSlice<Vector2>(Positions, 0, count));
    var move = new MoveDanmaku(this).Schedule(count, kBatchSize, dependency);
    dependency = new BoundsCheckDanmaku(this).Schedule(count, kBatchSize, move);
    dependency = new DestroyDanmaku(this).Schedule(dependency);
    if (DanmakuCollider.ColliderCount > 0) {
      dependency = new CollideDanamku(this).Schedule(count, kBatchSize, dependency);
    }
    return dependency;
  }

  /// <summary>
  /// Creates a new Danmaku from the pool.
  /// </summary>
  public Danmaku Get(DanmakuConfig config) {
    CheckCapacity(1);
    var state = config.CreateState();
    InitialStates[ActiveCount] = state;
    Times[ActiveCount] = 0f;
    var danmaku = new Danmaku(this, activeCountArray[0]++);
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
    var activeCount = ActiveCount;
    for (var i = 0; i < count; i++) {
      Times[activeCount + i] = 0f;
      danmaku[i] = new Danmaku(this, activeCount + i);
    }
    activeCountArray[0] += count;
  }

  /// <summary>
  /// Destroys all danmaku in the pool.
  /// </summary>
  public void Clear() => activeCountArray[0] = 0;

  void CheckCapacity(int count) {
    if (ActiveCount + count > Capacity) {
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

  /// <summary>
  /// Disposes of the DanmakuPool. This destroys all of the bullets in the pool and
  /// makes any operation on the pool invalid. Best used only when the pool is no longer
  /// in use.
  /// </summary>  
  public void Dispose() {
    activeCountArray.Dispose();
    InitialStates.Dispose();
    Times.Dispose();
    Positions.Dispose();
    Rotations.Dispose();
    Speeds.Dispose();
    AngularSpeeds.Dispose();
    Colors.Dispose();
    OldPositions.Dispose();
    CollisionMasks.Dispose();
  }

  /// <inheritdoc/>
  public DanmakuEnumerator GetEnumerator() => new DanmakuEnumerator(this, 0, ActiveCount);
  /// <inheritdoc/>
  IEnumerator<Danmaku> IEnumerable<Danmaku>.GetEnumerator() => GetEnumerator();
  /// <inheritdoc/>
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

  internal void Destroy(Danmaku deactivate) => Times[deactivate.Id] = float.MinValue;

}

/// <summary>
/// An enumerator of <see cref="DanmakU.Danmaku"/>
/// </summary>
public struct DanmakuEnumerator : IEnumerator<Danmaku> {

  readonly DanmakuPool pool;
  readonly int start;
  readonly int end;
  int index;

  /// <inheritdoc/>
  public Danmaku Current => new Danmaku(pool, index);
  object IEnumerator.Current => Current;

  internal DanmakuEnumerator(DanmakuPool pool, int start, int count) {
    this.pool =  pool;
    this.start = start;
    this.end = start + count;
    index = -1;
  }

  /// <inheritdoc/>
  public bool MoveNext() {
    if (index < 0) {
      index = start;
    } else {
      index++;
    }
    return index < end;
  }

  /// <inheritdoc/>
  public void Reset() => index = -1;

  /// <inheritdoc/>
  public void Dispose() {}

}

}