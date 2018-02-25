using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;

namespace DanmakU {

internal class MutableDanmakuCollisionList {

  const int kDefaultCapacity = 16;
  const int kGrowthFactor = 2;

  DanmakuCollision[] Array;
  public int Capacity => Array?.Length ?? kDefaultCapacity;
  public int Count { get; private set; }

  public DanmakuCollisionList AsReadOnly() => new DanmakuCollisionList(Array, Count);

  public MutableDanmakuCollisionList() {
    Array = new DanmakuCollision[kDefaultCapacity];
  }

  public void Add(DanmakuCollision obj) {
    if (Contains(obj)) return;
    CheckCapacity(1);
    Array[Count++] = obj;
  }

  public bool Contains(DanmakuCollision obj) {
    if (Count <= 1024) {
      bool contained = false;
      for (var i = 0; i < Count; i++) {
        contained |= Array[i].Danmaku == obj.Danmaku;
      }
      return contained;
    } else {
      for (var i = 0; i < Count; i++) {
        if (Array[i].Danmaku== obj.Danmaku) return true;
      }
      return false;
    }
  }

  public void Clear() => Count = 0;

  void CheckCapacity(int count) {
    if (Count + count <= Capacity) return;
    var newArray = new DanmakuCollision[Capacity * kGrowthFactor];
    System.Array.Copy(Array, 0, newArray, 0, Count);
    Array = newArray;
  }

}

}
