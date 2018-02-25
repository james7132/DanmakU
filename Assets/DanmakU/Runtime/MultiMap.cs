using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {
  
internal class MultiMap<TKey, TValue> : Dictionary<TKey, List<TValue>> {

  public void Add(TKey key, TValue value) {
    List<TValue> subset;
    if (!TryGetValue(key, out subset)) {
      subset = new List<TValue>();
      Add(key, subset);
    }
    subset.Add(value);
  }

  public void ClearSet(TKey key) {
    List<TValue> subset;
    if (TryGetValue(key, out subset)) {
      subset.Clear();
    }
  }

  public void RemoveElement(TKey key, TValue value) {
    List<TValue> subset;
    if (!TryGetValue(key, out subset))  return;
    subset.Remove(value);
    if (subset.Count <= 0) {
      Remove(key);
    }
  }

}

}
