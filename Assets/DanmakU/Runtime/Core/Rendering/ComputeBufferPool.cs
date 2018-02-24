using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

internal sealed class ComputeBufferPool : IDisposable {

  public struct Context {

    public readonly ComputeBufferPool Pool;
    readonly Queue<ComputeBuffer> usedBuffers;

    public Context(ComputeBufferPool pool) {
      Pool = pool;
      usedBuffers = new Queue<ComputeBuffer>();
    }

    public ComputeBuffer Rent(int count, int stride) {
      var buffer = Pool.Rent(count, stride);
      usedBuffers.Enqueue(buffer);
      return buffer;
    }

    public void Flush() {
      while (usedBuffers.Count > 0) {
        Pool.Return(usedBuffers.Dequeue());
      }
    }

  }

  struct ComputeBufferParams : IEquatable<ComputeBufferParams> {
    public readonly int Count;
    public readonly int Stride;

    public ComputeBufferParams(ComputeBuffer buffer) {
      Count = buffer.count;
      Stride = buffer.stride;
    }

    public ComputeBufferParams(int count, int stride) {
      Count = count;
      Stride = stride;
    }

    public ComputeBuffer CreateBuffer(ComputeBufferType type) {
      return new ComputeBuffer(Count, Stride, type);
    }

    public bool Equals(ComputeBufferParams bufferParams) {
      return Count == bufferParams.Count && Stride == bufferParams.Count;
    }
  }

  // To avoid GC on fetch.
  class ParamsEquality : IEqualityComparer<ComputeBufferParams> {
    public bool Equals(ComputeBufferParams a, ComputeBufferParams b) {
      return a.Count == b.Count && a.Stride == b.Stride;
    }
    public int GetHashCode(ComputeBufferParams bufferParams) {
      return bufferParams.Count * 7 + bufferParams.Stride;
    }
  }

  static Dictionary<ComputeBufferType, ComputeBufferPool> Pools;

  readonly Dictionary<ComputeBufferParams, Queue<ComputeBuffer>> queues;
  public ComputeBufferType Type { get; }

  public ComputeBufferPool(ComputeBufferType type = ComputeBufferType.Default) {
    queues = new Dictionary<ComputeBufferParams, Queue<ComputeBuffer>>(new ParamsEquality());
    Type = type;
  }

  public ComputeBuffer Rent(int count, int stride) {
    var bufferParams = new ComputeBufferParams(count, stride);
    Queue<ComputeBuffer> queue;
    if (!queues.TryGetValue(bufferParams, out queue)) {
      queue = new Queue<ComputeBuffer>();
      queues.Add(bufferParams, queue);
    }
    if (queue.Count <= 0) {
      return bufferParams.CreateBuffer(Type);
    } else {
      return queue.Dequeue();
    }
  }

  public void Return(ComputeBuffer buffer) {
    if (buffer == null) return;
    var bufferParams = new ComputeBufferParams(buffer);
    Queue<ComputeBuffer> queue;
    if (!queues.TryGetValue(bufferParams, out queue)) {
      queue = new Queue<ComputeBuffer>();
      queues.Add(bufferParams, queue);
    }
    queue.Enqueue(buffer);
  }

  public Context CreateContext() => new Context(this);

  public static void DisposeShared() {
    if (Pools == null) return;
    foreach (var pool in Pools.Values) {
      pool.Dispose();
    }
    Pools.Clear();
  }

  public static ComputeBufferPool GetShared(ComputeBufferType type = ComputeBufferType.Default) {
    Pools = Pools ?? new Dictionary<ComputeBufferType, ComputeBufferPool>();
    ComputeBufferPool pool;
    if (!Pools.TryGetValue(type, out pool)) {
      pool = new ComputeBufferPool(type);
      Pools.Add(type, pool);
    }
    return pool;
  }

  public void Dispose() {
    foreach (var queue in queues.Values) {
      while (queue.Count > 0) {
        queue.Dequeue().Dispose();
      }
    }
    queues.Clear();
  }

}

}