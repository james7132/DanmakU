using System;
using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace DanmakU {

[NativeContainer, NativeContainerSupportsMinMaxWriteRestriction]
public unsafe struct NativeStack<T> : IDisposable where T : struct {

  internal AtomicCounter counter;
  internal int capacity;
  internal IntPtr Stack;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
  internal int m_MinIndex;
  internal int m_MaxIndex;

  internal AtomicSafetyHandle m_Safety;
  internal DisposeSentinel m_DisposeSentinel;
#endif

  internal Allocator allocatorLabel;

  public NativeStack(int count, Allocator allocator) {
    capacity = count;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
    if (allocator <= Allocator.None)
        throw new ArgumentException("Allocator must be Temp, TempJob or Persistent", "allocator");
    if (count < 0)
        throw new ArgumentOutOfRangeException(nameof(count), "Count must be >= 0");
    if (!UnsafeUtility.IsBlittable<T>())
        throw new ArgumentException($"{typeof(T)} used in NativeCustomArray<{typeof(T)}> must be blittable");
#endif

    counter = new AtomicCounter(0, allocator);
    Stack = (IntPtr)UnsafeUtility.Malloc(count * UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), allocator);

    allocatorLabel = allocator;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
    m_MinIndex = 0;
    m_MaxIndex = count - 1;
    DisposeSentinel.Create(out m_Safety, out m_DisposeSentinel, 0);
#endif
  }

  public int Count => counter.Value;

  public void Push(T value) {
    var index = counter.Increment();
#if ENABLE_UNITY_COLLECTIONS_CHECKS
    if (index >= capacity) {
      throw new InvalidOperationException("NativeStack.Push went beyond capacity. Reallocate properly.");
    }
    AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
    AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif
    // Debug.Log($"Push {index}");
    UnsafeUtility.WriteArrayElement<T>((void*)Stack, index - 1, value);
  }

  public T Pop() {
    var index = counter.Decrement();
#if ENABLE_UNITY_COLLECTIONS_CHECKS
    if (index < 0) {
      throw new InvalidOperationException("NativeStack.Pop called on empty stack.");
    }
    AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
    AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif
    // Debug.Log($"Pop {index}");
    return UnsafeUtility.ReadArrayElement<T>((void*)Stack, index);
  }

  public void Dispose() {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
    DisposeSentinel.Dispose(m_Safety, ref m_DisposeSentinel);
#endif
    UnsafeUtility.Free((void*)Stack, allocatorLabel);
    Stack = IntPtr.Zero;
    counter.Dispose();
    capacity = 0;
  }

}

}