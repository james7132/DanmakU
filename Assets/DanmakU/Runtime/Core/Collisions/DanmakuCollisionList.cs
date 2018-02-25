using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;

namespace DanmakU {

/// <summary>
/// A read-only list of Danmaku.
/// </summary>
public struct DanmakuCollisionList : IReadOnlyList<DanmakuCollision> {

  readonly DanmakuCollision[] Array;
  public int Count { get; }

  internal DanmakuCollisionList(DanmakuCollision[] array, int count) {
    Array = array;
    Count = count;
  }

  public DanmakuCollision this[int index] {
    get {
      if (index < 0 && index >= Count) {
        throw new IndexOutOfRangeException(nameof(index));
      }
      return Array[index];
    }
  }

  public Enumerator GetEnumerator() => new Enumerator(this);
  IEnumerator<DanmakuCollision> IEnumerable<DanmakuCollision>.GetEnumerator() => GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

  public struct Enumerator : IEnumerator<DanmakuCollision> {

    readonly DanmakuCollisionList list;
    int currentIndex;
    public DanmakuCollision Current => list[currentIndex];
    object IEnumerator.Current => Current;

    internal Enumerator(DanmakuCollisionList list) {
      this.list = list;
      currentIndex = -1;
    }

    public bool MoveNext() {
      currentIndex++;
      return currentIndex < list.Count;
    }

    public void Reset() => currentIndex = -1;

    public void Dispose() {}

  }

}

}
