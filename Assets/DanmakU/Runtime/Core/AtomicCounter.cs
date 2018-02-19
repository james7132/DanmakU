using System;
using System.Threading;
using UnityEngine;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace DanmakU {

[NativeContainer, NativeContainerSupportsMinMaxWriteRestriction]
public unsafe struct AtomicCounter : IDisposable {

  IntPtr count;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
  internal int m_MinIndex;
  internal int m_MaxIndex;

  internal AtomicSafetyHandle m_Safety;
  internal DisposeSentinel m_DisposeSentinel;
#endif

  internal Allocator allocatorLabel;

  public AtomicCounter(int value, Allocator allocator) {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
    if (allocator <= Allocator.None)
        throw new ArgumentException("Allocator must be Temp, TempJob or Persistent", "allocator");
#endif

    this.count = (IntPtr)UnsafeUtility.Malloc(sizeof(long), UnsafeUtility.AlignOf<int>(), allocator);
    *((long*)count) = value;
    allocatorLabel = allocator;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
    m_MinIndex = 0;
    m_MaxIndex = 0;
    DisposeSentinel.Create(out m_Safety, out m_DisposeSentinel, 0);
#endif
  }

  public int Value => (int)Interlocked.Read(ref *((long*)count));
  public int Increment() => (int)Interlocked.Increment(ref *((long*)count));
  public int Decrement() => (int)Interlocked.Decrement(ref *((long*)count));
  public int Add(int value) => (int)Interlocked.Add(ref *((long*)count), value);
  public int Set(int value) => (int)Interlocked.Exchange(ref *((long*)count), value);

  public void Dispose() {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
    DisposeSentinel.Dispose(m_Safety, ref m_DisposeSentinel);
#endif
    UnsafeUtility.Free(count.ToPointer(), allocatorLabel);
    count = IntPtr.Zero;
  }

}

}